using MusicDLWin;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class clsInstall : IDisposable
{

    /// <summary>
    /// メッセージを表示するためのクラス
    /// </summary>

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
            messageDisplayer.UpdateRichTextBox("Python仮想環境を作成しています...");
            var psi = new ProcessStartInfo("cmd.exe", "/c python -m venv .venv")
            {
                WorkingDirectory = appBasePath,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(psi)?.WaitForExit();
        }

        if (!File.Exists(pythonExe))
        {
            messageDisplayer.UpdateRichTextBox("❌Pythonの仮想環境が作成できませんでした。");
            return await Task.FromResult(false);
        }

        messageDisplayer.UpdateRichTextBox("✅Python仮想環境の作成に成功しました。");

        messageDisplayer.UpdateRichTextBox("▶pipをアップグレード中...");
        await RunCommandAsync(pythonExe, "-m pip install --upgrade pip");

        //依存パッケージも明示的にインストール
        messageDisplayer.UpdateRichTextBox("▶spotdlと依存パッケージ(spotipy, requests, urllib3)をインストール中...");
        await RunCommandAsync(pythonExe, "-m pip install spotdl spotipy requests urllib3");

        messageDisplayer.UpdateRichTextBox("✅spotdlと依存パッケージのインストールが完了しました。");

        //.venv/Scriptsのパスをiniに保存 ---
        try
        {
            clsIniData objIni = new clsIniData();
            objIni.SetPythonPath(Path.Combine(appBasePath, ".venv", "Scripts"));
        }
        catch (Exception ex)
        {
            messageDisplayer.UpdateRichTextBox("Pythonパスの保存に失敗: " + ex.Message);
        }


        return await Task.FromResult(true);
    }

    /// <summary>
    /// FFmpegをダウンロードして仮想環境に展開します。
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
            await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    messageDisplayer.UpdateRichTextBox("▶ FFmpegのダウンロード中です。");
                    client.DownloadFile(ffmpegUrl, zipPath);
                }
            });

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

                // ffmpegディレクトリのパスをiniに保存
                try
                {
                    clsIniData objIni = new clsIniData();
                    objIni.SetFFmpegPath(ffmpegExtractPath);
                }
                catch (Exception ex)
                {
                    messageDisplayer.UpdateRichTextBox("FFmpegパスの保存に失敗: " + ex.Message);
                }

                return true;
            }
            else
            {
                messageDisplayer.UpdateRichTextBox("❌FFmpeg.exeが見つかりませんでした。");
                return false;
            }
        }
        catch (Exception ex)
        {
            messageDisplayer.UpdateRichTextBox("❌FFmpegのインストールに失敗しました: " + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// コマンドを実行して、標準出力とエラー出力をRichTextBoxに表示します。
    /// </summary>
    /// <param name="exePath"></param>
    /// <param name="args"></param>
    /// <summary>
    private async Task RunCommandAsync(string exePath, string args)
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
                else if (!string.IsNullOrWhiteSpace(e.Data)) messageDisplayer.UpdateRichTextBox(e.Data);
            };

            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data == null) errorTcs.TrySetResult(true);
                else if (!string.IsNullOrWhiteSpace(e.Data)) messageDisplayer.UpdateRichTextBox(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await Task.WhenAll(outputTcs.Task, errorTcs.Task, Task.Run(() => process.WaitForExit()));
        }
    }


    /// <summary>
    /// リソースを解放します。
    /// </summary>
    public void Dispose()
    {
    }
}
