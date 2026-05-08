using MusicDLWin;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

public class clsInstall : IDisposable
{

    #region "メッセージを表示するためのクラス"
    /// <summary>
    /// メッセージを表示するためのクラス
    /// </summary>
    private MessageDisplayer messageDisplayer { get; set; } = null;
    #endregion

    #region "現在実行中のアプリケーション（.exe）が存在するディレクトリのパス"
    /// <summary>
    /// 現在実行中のアプリケーション（.exe）が存在するディレクトリのパス
    /// </summary>
    private string appBasePath { get; set; } = string.Empty;
    #endregion
    /// <summary>
    /// clsInstall クラスのコンストラクタ
    /// </summary>
    /// <param name="resultBox"></param>
    public clsInstall(RichTextBox resultBox)
    {
        messageDisplayer = new MessageDisplayer(resultBox);
        this.appBasePath = AppDomain.CurrentDomain.BaseDirectory;
    }

    /// <summary>
    /// Pythonとspotdlを仮想環境にインストールします。
    /// </summary>
    /// <returns>true：成功、false：失敗</returns>
    public async Task<bool> InstallPython()
    {
        string venvPath = Path.Combine(appBasePath, ".venv");
        string pythonExe = Path.Combine(venvPath, "Scripts", "python.exe");

        if (!Directory.Exists(venvPath))
        {
            messageDisplayer.UpdateRichTextBox("▶Pythonがシステムにインストールされているか確認中...");

            bool pythonFound = CheckSystemPython();
            string pythonCommand = "python";

            if (!pythonFound)
            {
                messageDisplayer.UpdateRichTextBox("❌ Pythonがシステムにインストールされていません。");
                messageDisplayer.UpdateRichTextBox("▶ Pythonを自動ダウンロード・インストールします...");

                if (!await DownloadAndInstallPython())
                {
                    return await Task.FromResult(false);
                }

                // インストール後、再度確認
                pythonFound = CheckSystemPython();
                if (!pythonFound)
                {
                    messageDisplayer.UpdateRichTextBox("❌ Pythonのインストールに失敗しました。");
                    return await Task.FromResult(false);
                }
            }

            messageDisplayer.UpdateRichTextBox("▶Python仮想環境を作成しています...");
            var psi = new ProcessStartInfo("cmd.exe", $"/c {pythonCommand} -m venv .venv")
            {
                WorkingDirectory = appBasePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            string output = "";
            string error = "";
            using (var process = new Process { StartInfo = psi })
            {
                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data)) output += e.Data + "\n";
                };
                process.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data)) error += e.Data + "\n";
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await Task.Run(() => process.WaitForExit());

                if (process.ExitCode != 0)
                {
                    messageDisplayer.UpdateRichTextBox("❌Python仮想環境の作成に失敗しました。");
                    messageDisplayer.UpdateRichTextBox("❌実行コマンド: " + pythonCommand + " -m venv .venv");
                    messageDisplayer.UpdateRichTextBox("❌実行場所: " + appBasePath);
                    if (!string.IsNullOrEmpty(error))
                    {
                        messageDisplayer.UpdateRichTextBox("❌詳細エラー: " + error);
                    }
                    if (!string.IsNullOrEmpty(output))
                    {
                        messageDisplayer.UpdateRichTextBox("❌出力内容: " + output);
                    }
                    return await Task.FromResult(false);
                }
            }
        }

        if (!File.Exists(pythonExe))
        {
            messageDisplayer.UpdateRichTextBox("❌Pythonの仮想環境が作成できませんでした。");
            messageDisplayer.UpdateRichTextBox("▶ " + venvPath + " が正しく作成されているか確認してください。");
            return await Task.FromResult(false);
        }

        messageDisplayer.UpdateRichTextBox("✅Python仮想環境の作成に成功しました。");

        messageDisplayer.UpdateRichTextBox("▶spotdlと依存パッケージ(spotipy, requests, urllib3)をインストール中...");
        messageDisplayer.UpdateRichTextBox("▶これには数分かかる場合があります。お待ちください...");
        await RunCommandAsync(pythonExe, "-m pip install spotdl spotipy requests urllib3", 600);

        messageDisplayer.UpdateRichTextBox("✅spotdlと依存パッケージのインストールが完了しました。");

        messageDisplayer.UpdateRichTextBox("▶yt-dlpをインストール中...");
        messageDisplayer.UpdateRichTextBox("▶これには数分かかる場合があります。お待ちください...");
        await RunCommandAsync(pythonExe, "-m pip install yt-dlp", 600);

        messageDisplayer.UpdateRichTextBox("✅yt-dlpのインストールが完了しました。");

        string pythonPath = Path.Combine(appBasePath, ".venv", "Scripts");
        SavePythonPath(pythonPath);

        messageDisplayer.UpdateRichTextBox("▶ PythonのパスをWindowsユーザーPATHに追加しています...");
        AddToUserPath(pythonPath);

        return await Task.FromResult(true);
    }

    /// <summary>
    /// システムに Python がインストールされているか確認
    /// </summary>
    private bool CheckSystemPython()
    {
        foreach (var cmd in new[] { "python", "python3" })
        {
            var checkPython = new ProcessStartInfo("cmd.exe", $"/c {cmd} --version")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(checkPython))
            {
                process?.WaitForExit();
                if (process?.ExitCode == 0)
                {
                    messageDisplayer.UpdateRichTextBox("✅ システムのPython: " + cmd);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Pythonをダウンロードして自動インストール
    /// </summary>
    private async Task<bool> DownloadAndInstallPython()
    {
        string pythonInstallerUrl = "https://www.python.org/ftp/python/3.11.8/python-3.11.8-amd64.exe";
        string tempDir = Path.Combine(Path.GetTempPath(), "MusicDLWin_Setup");
        string installerPath = Path.Combine(tempDir, "python-installer.exe");

        try
        {
            // 一時ディレクトリを作成
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            messageDisplayer.UpdateRichTextBox("▶ Pythonインストーラーをダウンロード中...");
            messageDisplayer.UpdateRichTextBox("▶ このプロセスには数分かかる場合があります。");

            using (WebClient client = new WebClient())
            {
                await Task.Run(() =>
                {
                    client.DownloadFile(pythonInstallerUrl, installerPath);
                });
            }

            if (!File.Exists(installerPath))
            {
                messageDisplayer.UpdateRichTextBox("❌ Pythonインストーラーのダウンロードに失敗しました。");
                return false;
            }

            messageDisplayer.UpdateRichTextBox("✅ Pythonインストーラーのダウンロードが完了しました。");
            messageDisplayer.UpdateRichTextBox("▶ Pythonをインストール中...");

            // Python を自動インストール（全ユーザー対象、PATH に追加）
            var psi = new ProcessStartInfo
            {
                FileName = installerPath,
                Arguments = "/quiet InstallAllUsers=1 PrependPath=1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = psi })
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await Task.Run(() => process.WaitForExit(600000)); // 10分タイムアウト

                if (process.ExitCode != 0)
                {
                    messageDisplayer.UpdateRichTextBox("❌ Pythonのインストールに失敗しました。");
                    messageDisplayer.UpdateRichTextBox("❌ エラーコード: " + process.ExitCode);
                    return false;
                }
            }

            messageDisplayer.UpdateRichTextBox("✅ Pythonのインストールが完了しました。");

            // インストーラーをクリーンアップ
            try
            {
                if (File.Exists(installerPath))
                {
                    File.Delete(installerPath);
                }
                if (Directory.Exists(tempDir) && Directory.GetFiles(tempDir).Length == 0)
                {
                    Directory.Delete(tempDir);
                }
            }
            catch
            {
                // クリーンアップ失敗は無視
            }

            return true;
        }
        catch (Exception ex)
        {
            messageDisplayer.UpdateRichTextBox("❌ Pythonのインストール処理でエラーが発生しました: " + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Pythonパスを設定ファイルに保存します
    /// </summary>
    private void SavePythonPath(string pythonPath)
    {
        try
        {
            clsIniData objIni = new clsIniData();
            objIni.SetPythonPath(pythonPath);
            messageDisplayer.UpdateRichTextBox("✅Pythonパスを保存しました: " + pythonPath);
        }
        catch (Exception ex)
        {
            messageDisplayer.UpdateRichTextBox("❌Pythonパスの保存に失敗: " + ex.Message);
        }
    }

    /// <summary>
    /// FFmpegをダウンロードして展開します。
    /// </summary>
    /// <returns>true：成功、false：失敗</returns>
    public async Task<bool> InstallFFmpeg()
    {
        string ffmpegUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";
        string zipPath = Path.Combine(appBasePath, "ffmpeg.zip");
        string ffmpegExtractPath = Path.Combine(appBasePath, "ffmpeg");

        try
        {
            messageDisplayer.UpdateRichTextBox("▶ FFmpegのダウンロードを開始します...");
            messageDisplayer.UpdateRichTextBox("▶ ダウンロード中です。少しお待ちください...");

            if (!await DownloadFFmpeg(ffmpegUrl, zipPath))
            {
                return false;
            }

            if (!File.Exists(zipPath))
            {
                messageDisplayer.UpdateRichTextBox("❌FFmpegのダウンロードファイルが見つかりません。");
                return await Task.FromResult(false);
            }

            if (Directory.Exists(ffmpegExtractPath))
                Directory.Delete(ffmpegExtractPath, true);

            messageDisplayer.UpdateRichTextBox("▶FFmpegを展開中...");
            await Task.Run(() => ZipFile.ExtractToDirectory(zipPath, ffmpegExtractPath));

            string[] ffmpegExePath = Directory.GetFiles(ffmpegExtractPath, "ffmpeg.exe", SearchOption.AllDirectories);
            if (ffmpegExePath.Length > 0)
            {
                string dest = Path.Combine(ffmpegExtractPath, "ffmpeg.exe");
                File.Copy(ffmpegExePath[0], dest, true);
                messageDisplayer.UpdateRichTextBox("✅FFmpegの展開が完了しました。");

                try
                {
                    if (File.Exists(zipPath))
                        File.Delete(zipPath);
                }
                catch
                {
                    // 削除失敗は無視
                }

                SaveFFmpegPath(ffmpegExtractPath);

                messageDisplayer.UpdateRichTextBox("▶ FFmpegのパスをWindowsユーザーPATHに追加しています...");
                AddToUserPath(ffmpegExtractPath);

                return await Task.FromResult(true);
            }
            else
            {
                messageDisplayer.UpdateRichTextBox("❌FFmpeg.exeが見つかりませんでした。");
                messageDisplayer.UpdateRichTextBox("▶ ダウンロードしたZIPファイルが破損している可能性があります。");
                return await Task.FromResult(false);
            }
        }
        catch (Exception ex)
        {
            messageDisplayer.UpdateRichTextBox("❌FFmpegのインストールに失敗しました: " + ex.Message);
            return await Task.FromResult(false);
        }
    }

    /// <summary>
    /// FFmpegをダウンロード
    /// </summary>
    private async Task<bool> DownloadFFmpeg(string url, string savePath)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                await Task.Run(() =>
                {
                    client.DownloadFile(url, savePath);
                });
            }
            messageDisplayer.UpdateRichTextBox("✅FFmpegのダウンロードが完了しました。");
            return true;
        }
        catch (WebException webEx)
        {
            messageDisplayer.UpdateRichTextBox("❌FFmpegのダウンロードに失敗しました。");
            messageDisplayer.UpdateRichTextBox("▶ エラー: " + webEx.Message);
            messageDisplayer.UpdateRichTextBox("▶ インターネット接続を確認してください。");
            return false;
        }
        catch (Exception dlEx)
        {
            messageDisplayer.UpdateRichTextBox("❌FFmpegのダウンロードに失敗しました。");
            messageDisplayer.UpdateRichTextBox("▶ エラー: " + dlEx.Message);
            return false;
        }
    }

    /// <summary>
    /// FFmpegパスを設定ファイルに保存します
    /// </summary>
    private void SaveFFmpegPath(string ffmpegPath)
    {
        try
        {
            clsIniData objIni = new clsIniData();
            objIni.SetFFmpegPath(ffmpegPath);
            messageDisplayer.UpdateRichTextBox("✅FFmpegパスを保存しました: " + ffmpegPath);
        }
        catch (Exception ex)
        {
            messageDisplayer.UpdateRichTextBox("❌FFmpegパスの保存に失敗: " + ex.Message);
        }
    }

    /// <summary>
    /// コマンドを実行して、標準出力とエラー出力をRichTextBoxに表示します。
    /// </summary>
    /// <param name="exePath"></param>
    /// <param name="args"></param>
    /// <param name="timeoutSeconds">タイムアウト時間（秒）。0 の場合は無制限</param>
    /// <summary>
    private async Task RunCommandAsync(string exePath, string args, int timeoutSeconds = 300)
    {
        var psi = new ProcessStartInfo("cmd.exe", $"/c \"{exePath}\" {args}")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = psi, EnableRaisingEvents = true })
        {
            var outputTcs = new TaskCompletionSource<bool>();
            var errorTcs = new TaskCompletionSource<bool>();

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data == null) outputTcs.TrySetResult(true);
                else if (!string.IsNullOrWhiteSpace(e.Data)) messageDisplayer.UpdateRichTextBox(e.Data, true);
            };

            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data == null) errorTcs.TrySetResult(true);
                else if (!string.IsNullOrWhiteSpace(e.Data)) messageDisplayer.UpdateRichTextBox(e.Data, true);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            try
            {
                if (timeoutSeconds > 0)
                {
                    using (var cts = new System.Threading.CancellationTokenSource(timeoutSeconds * 1000))
                    {
                        await Task.WhenAll(
                            outputTcs.Task,
                            errorTcs.Task,
                            Task.Run(() => process.WaitForExit(), cts.Token)
                        );
                    }
                }
                else
                {
                    await Task.WhenAll(outputTcs.Task, errorTcs.Task, Task.Run(() => process.WaitForExit()));
                }
            }
            catch (OperationCanceledException)
            {
                messageDisplayer.UpdateRichTextBox("⚠ コマンドがタイムアウト (" + timeoutSeconds + "秒) しました。プロセスを終了します...");
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
        }
    }

    /// <summary>
    /// ユーザー PATH に指定されたパスを追加します（存在しない場合のみ）
    /// </summary>
    /// <param name="pathToAdd">追加するパス</param>
    /// <returns>追加成功時 true、既に存在する場合も true、失敗時 false</returns>
    private bool AddToUserPath(string pathToAdd)
    {
        try
        {
            if (string.IsNullOrEmpty(pathToAdd) || !Directory.Exists(pathToAdd))
            {
                return false;
            }

            // ユーザー環境変数を取得
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? string.Empty;

            // 既に PATH に含まれているかチェック
            if (currentPath.Contains(pathToAdd))
            {
                messageDisplayer.UpdateRichTextBox("▶ " + pathToAdd + " は既にPATHに含まれています。");
                return true;
            }

            // PATH に追加
            string newPath = currentPath + ";" + pathToAdd;
            Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.User);

            messageDisplayer.UpdateRichTextBox("✅ PATHに追加しました: " + pathToAdd);
            return true;
        }
        catch (Exception ex)
        {
            messageDisplayer.UpdateRichTextBox("❌ PATHへの追加に失敗: " + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// リソースを解放します。
    /// </summary>
    public void Dispose()
    {
    }
}


