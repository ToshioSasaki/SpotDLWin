using Raven.Abstractions.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Hosting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicDLWin.Models;
using TagLib.NonContainer;

namespace MusicDLWin
{
    public partial class frmMusicDL : Form
    {

        #region "各プロパティ"
        /// <summary>
        /// 各プロパティ
        /// </summary
        private MessageDisplayer messageDisplayer { get; set; } = null;
        private bool CloseFlg { get; set; } = false; // フォームのクローズフラグ
        private bool _isDownloading { get; set; } = false; // ダウンロード処理中フラグ
        #endregion

        #region "コンストラクタ"
        public frmMusicDL()
        {
            InitializeComponent();
            messageDisplayer = new MessageDisplayer(this.ResultText);
            AddUrlTypeLabel();
            this.Shown += SpotDL_Shown;
        }

        /// <summary>
        /// URLタイプ表示ラベルを追加
        /// </summary>
        private void AddUrlTypeLabel()
        {
            Label urlTypeLabel = new Label();
            urlTypeLabel.Text = "Youtube/Spotify";
            urlTypeLabel.Font = new System.Drawing.Font("游ゴシック", 7F);
            urlTypeLabel.ForeColor = System.Drawing.Color.DarkGreen;
            urlTypeLabel.BackColor = System.Drawing.Color.YellowGreen;
            urlTypeLabel.AutoSize = true;
            urlTypeLabel.Location = new System.Drawing.Point(3, 163);
            this.Controls.Add(urlTypeLabel);
        }
        #endregion

        #region "初期化処理"
        /// <summary>
        /// プログレスバーの初期化
        /// </summary>
        private void SetPrgBar()
        {
            PrgBar.Minimum = 0;
            PrgBar.Maximum = 100;
            PrgBar.Step = 10;
            PrgBar.Value = 0;
            PrgBar.Visible = true;
        }

        /// <summary>
        /// コンフィグデータ読込み
        /// </summary>
        private void ReadConfigData()
        {
            //コンフィグデータの読込み
            clsIniData objIni = new clsIniData();
            objIni.GetIniData();
            textAlbumName.Text = "";
            textOutDir.Text = objIni.getOutPath;
            checkBoxSpotifyAuth.Checked = MusicDLWin.Properties.Settings.Default.SpotifyUserAuth;
        }

        /// <summary>
        /// Spotify認証の利用設定を保存します。
        /// </summary>
        private void checkBoxSpotifyAuth_CheckedChanged(object sender, EventArgs e)
        {
            MusicDLWin.Properties.Settings.Default.SpotifyUserAuth = checkBoxSpotifyAuth.Checked;
            MusicDLWin.Properties.Settings.Default.Save();
        }
        #endregion

        #region "起動しているプロセスの強制終了"
        /// <summary>
        /// 起動プロセスの終了
        /// <param name="exename">プロセス名</param>
        /// <param name="stop">ダウンロード停止＝trueの時</param>
        /// </summary>
        private void ProcessKills(string exename,bool stop = false)
        {
            // 対象のexeファイル名
            string exeFileName = exename;// "ffmpeg";
            // 実行中のすべてのプロセスを取得
            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                // プロセス名が目的のexeファイル名と一致するか確認
                if (process.ProcessName == exeFileName)
                {
                    // プロセスを終了
                    process.Kill();
                    process.Close();
                    process.Dispose();
                    if (stop == false)
                    {
                        messageDisplayer.UpdateRichTextBox("プロセスファイルが起動していた為終了させました。");
                    }
                }
            }
        }
        #endregion

        #region "ディレクトリをチェックします。"
        /// <summary>
        /// インプット、アウトプット、パスのチェック
        /// </summary>
        /// <returns>True：成功、False：エラー</returns>
        private bool CheckDIr()
        {
            DialogResult Result = MessageBox.Show("この出力先パスでよいですか？" + textOutDir.Text.Trim(), "出力先確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (Result == DialogResult.OK)
            {
                if (Directory.Exists(textOutDir.Text.Trim()) == false)
                {
                    MessageBox.Show("出力先パスが存在しません。", "出力先エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (string.IsNullOrEmpty(inputTextBox.Text.Trim()))
                {
                    MessageBox.Show("URLエラーです", "URLエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            } else
            {
                return false;
            }
            return true;
        }
        #endregion

        #region "作業フォルダにあるMP3ファイルのみ全て削除します。"
        /// <summary>
        /// ローカルフォルダにあるMP3ファイルを全て削除します。
        /// </summary>
        private void DeleteMP3LocalFile()
        {
            string AppPath = Application.StartupPath + "\\";
            string[] mp3Files = Directory.GetFiles(AppPath, "*.mp3");
            // 各MP3ファイルを削除
            foreach (string file in mp3Files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (IOException e)
                {
                    messageDisplayer.UpdateRichTextBox($"ローカルファイルを削除できませんでした。削除できなかったファイル: {file}. Error: {e.Message}");
                }
            }
        }
        #endregion

        #region "アルバムフォルダ内のMP3ファイルを削除"
        /// <summary>
        /// アルバムフォルダ内のMP3ファイルを削除します。
        /// </summary>
        private void DeleteMP3InAlbumFolder(string albumFolder)
        {
            if (Directory.Exists(albumFolder))
            {
                string[] mp3Files = Directory.GetFiles(albumFolder, "*.mp3");
                // 各MP3ファイルを削除
                foreach (string file in mp3Files)
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (IOException e)
                    {
                        messageDisplayer.UpdateRichTextBox($"アルバムフォルダのファイルを削除できませんでした。削除できなかったファイル: {file}. Error: {e.Message}");
                    }
                }
            }
        }
        #endregion

        #region "フォルダのダイアログを開きパスを指定します。"
        /// <summary>
        /// ダイアログを開きパスを指定します。
        /// </summary>
        private void MoveMakeFolder()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = textOutDir.Text;
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //ディレクトリ変更
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    textOutDir.Text = selectedFolder;
                    textOutDir.Text = textOutDir.Text + "\\";
                    //設定ファイルの変更
                    clsIniData objIni = new clsIniData();
                    objIni.SetIniData(textOutDir.Text.Trim());
                }
            }
        }
        #endregion

        #region "パス設定ダイアログを開きます。"
        /// <summary>
        /// パス設定ダイアログを開きます。
        /// </summary>
        /// <param name="dirs">リストのディレクトリ</param>
        /// <param name="title">Python、FFmpeg</param>
        private void ShowPathSetting(List<string> dirs,string title)
        {
            clsIniData objIni = new clsIniData();
            objIni.GetIniData();
        
            using (frmPathSetting pathSettingForm = new frmPathSetting(dirs,title))
            {
                // ダイアログとして開く
                if (pathSettingForm.ShowDialog() == DialogResult.OK)
                {
                    // 選択されたパスを受け取る
                    if (!string.IsNullOrEmpty(pathSettingForm.SelectedPath))
                    {   
                        if (System.IO.Directory.Exists(pathSettingForm.SelectedPath))
                        {
                            if (title == "Python")
                            {
                                // Pythonのパスを設定
                                objIni.SetPythonPath(pathSettingForm.SelectedPath);
                                objIni.GetIniData();
                                messageDisplayer.UpdateRichTextBox("Pythonのパスを設定しました。PythonPath：" + objIni.getPythonPath);
                            }
                            else if (title == "FFmpeg")
                            {
                                // FFmpegのパスを設定
                                objIni.SetFFmpegPath(pathSettingForm.SelectedPath);
                                objIni.GetIniData();
                                messageDisplayer.UpdateRichTextBox("FFmpegのパスを設定しました。FFmpegPath：" + objIni.getFFmpegPath);
                            }
                        }
                        else
                        {
                            messageDisplayer.UpdateRichTextBox("選択されたパスが存在しません。Path：" + pathSettingForm.SelectedPath);
                        }
                    }  
                }
            }
        }
        #endregion


        #region "フォルダ内部削除"
        /// <summary>
        /// OutPutフォルダの中身の削除
        /// </summary>
        private void CheckOutPutDeleteFiles()
        {
            //ディレクトリが存在するかしないか確認
            if (!Directory.Exists(textOutDir.Text.Trim()))
            {
                // フォルダが存在しない場合、フォルダを作成
                Directory.CreateDirectory(textOutDir.Text.Trim());
            }
            else
            {
                //　フォルダが存在している場合、フォルダの中を削除
                string destinationFolder = textOutDir.Text.Trim();
                string[] mp3Files = Directory.GetFiles(destinationFolder, "*.mp3");

                foreach (string mp3File in mp3Files)
                {
                    string fileName = Path.GetFileName(mp3File);
                    string destinationPath = Path.Combine(destinationFolder, fileName);

                    if (System.IO.File.Exists(destinationPath) == true)
                    {
                        try
                        {
                            System.IO.File.Delete(destinationPath);
                        }
                        catch (IOException e)
                        {
                            messageDisplayer.UpdateRichTextBox($"出力先のファイルを削除できませんでした。削除できなかったファイル: {fileName}. Error: {e.Message}");
                        }
                    }
                }

            }
        }
        #endregion

        #region "スタート･タイマー＆プログレスバー"
        /// <summary>
        /// スタート･タイマー＆プログレスバー
        /// </summary>
        private void StartTimerProgresBar()
        {
            Time.Enabled = true;
            Time.Start();
            PrgBar.Value = 0;
        }
        #endregion

        #region "ストップ･タイマー＆プログレスバー"
        /// <summary>
        /// ストップ･タイマー＆プログレスバー
        /// </summary>
        private void StopTimerProgresBar()
        {
            Time.Stop();
            PrgBar.Value = 0;
            Time.Enabled = false;
        }
        #endregion

        #region "URL判定"
        /// <summary>
        /// URLの種別を判定します
        /// </summary>
        /// <param name="url">判定するURL</param>
        /// <returns>"spotify" または "youtube" または "unknown"</returns>
        private string GetUrlType(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return "unknown";

            url = url.ToLower();

            if (url.Contains("spotify.com"))
                return "spotify";

            if (url.Contains("youtube.com") || url.Contains("youtu.be"))
                return "youtube";

            return "unknown";
        }
        #endregion

        #region "メイン処理"
        /// <summary>
        /// 作業開始・メイン処理
        /// </summary>
        private async void WorksFiles()
        {
            if (_isDownloading)
            {
                messageDisplayer.UpdateRichTextBox("⚠ ダウンロード処理が実行中です。完了をお待ちください。");
                return;
            }

            if (!EnsureSpotdlLimitIsAvailable())
            {
                _isDownloading = false;
                this.Download.Enabled = true;
                this.StopTimerProgresBar();
                return;
            }

            _isDownloading = true;
            this.Download.Enabled = false;
            try
            {
                this.StartTimerProgresBar();

                // PythonとFFmpegのインストールチェック
                await CheckPythonAndFFmpegAsync();

                //ディレクトリチェック
                if (CheckDIr())
                {
                    //ローカルファイルのMP3ファイルをすべて消去する
                    DeleteMP3LocalFile();

                    //作業開始
                    DateTime now = DateTime.Now;
                    string sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                    string sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                    messageDisplayer.UpdateRichTextBox("■ダウンロード作業を開始します。(" + sYmd + sHms + ")■");

                    // SpotDLのインストールを確認
                    string playlistTitle = this.textAlbumName.Text.Trim();
                    string playlistUrl = this.inputTextBox.Text.Trim();
                    string outputFolder = textOutDir.Text.Trim().Replace('¥', '\\').TrimEnd('\\');

                    if (string.IsNullOrWhiteSpace(playlistUrl) || string.IsNullOrWhiteSpace(outputFolder))
                    {
                        messageDisplayer.UpdateRichTextBox("プレイリストURLと出力先フォルダを入力してください。");
                        return;
                    }

                    // アルバム名フォルダを作成
                    string albumFolder = Path.Combine(outputFolder, textAlbumName.Text.Trim());

                    // アルバム名フォルダ内のMP3ファイルを削除
                    DeleteMP3InAlbumFolder(albumFolder);

                    //YouTubeから該当ファイルをダウンロード
                    bool downloadCompleted = await DownloadPlaylistAsync(playlistUrl, albumFolder);

                    //テンプフォルダから正式なフォルダへコピー
                    //bool success = CopyMp3File();
                    string updateMessage = downloadCompleted && UpdateMp3Properties(albumFolder, textAlbumName.Text.Trim()) == "" ? "成功" : "失敗";

                    //終了時間の打刻
                    now = DateTime.Now;
                    sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                    sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                    string Argument = "■ダウンロード終了 ファイルアップデート(" + updateMessage + ")" + sYmd + sHms;
                    messageDisplayer.UpdateRichTextBox(Argument);
                }
            } catch (Exception ex) {
                messageDisplayer.UpdateRichTextBox($"❌ エラー発生: {ex.Message}\n{ex.StackTrace}");
            } finally {
                _isDownloading = false;
                this.Download.Enabled = true;
                this.StopTimerProgresBar();
            }
        }

        /// <summary>
        /// ダウンロード読込み処理：URLの種別に応じたダウンロード実行
        /// </summary>
        /// <param name="playlistUrl">リンク</param>
        /// <param name="outputFolder">MP3アウトプットフォルダ</param>
        /// <returns>タスク</returns>
        private async Task<bool> DownloadPlaylistAsync(string playlistUrl, string outputFolder)
        {
            try
            {
                Directory.CreateDirectory(outputFolder);

                string urlType = GetUrlType(playlistUrl);

                if (urlType == "spotify")
                {
                    messageDisplayer.UpdateRichTextBox("▶ SpotifyのURLを検出しました。SpotDLでダウンロードします...");
                    return await DownloadSpotifyAsync(playlistUrl, outputFolder);
                }
                else if (urlType == "youtube")
                {
                    messageDisplayer.UpdateRichTextBox("▶ YouTubeのURLを検出しました。yt-dlpでダウンロードします...");
                    return await DownloadYouTubeAsync(playlistUrl, outputFolder);
                }
                else
                {
                    messageDisplayer.UpdateRichTextBox("❌ サポートされていないURLです。SpotifyまたはYouTubeのURLを入力してください。");
                    return false;
                }
            }
            catch (Exception ex)
            {
                messageDisplayer.UpdateRichTextBox($"❌ エラー発生: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// SpotifyのURLをSpotDLでダウンロードします。
        /// </summary>
        /// <param name="playlistUrl">プレイリストURL</param>
        /// <param name="outputFolder">出力先フォルダ</param>
        /// <returns></returns>
        private async Task<bool> DownloadSpotifyAsync(string playlistUrl, string outputFolder)
        {
            try
            {
                int ffmpegErrorCount = 0;
                bool rateLimitError = false;
                bool downloadSuccess = false;

                clsIniData objIni = new clsIniData();
                objIni.GetIniData();

                string PythonPath = objIni.getPythonPath;
                string FFmpegPath = objIni.getFFmpegPath;
                string pythonExe = Path.Combine(PythonPath, "python.exe");
                string FFmpegExe = Path.Combine(FFmpegPath, "ffmpeg.exe");
                bool useSpotifyAuth = checkBoxSpotifyAuth.Checked;

                int installFlg = 0;

                if (!System.IO.File.Exists(pythonExe))
                {
                    messageDisplayer.UpdateRichTextBox("Pythonが見つかりません。\nパス設定を確認してください。");
                    installFlg++;
                }

                if (!System.IO.File.Exists(FFmpegExe))
                {
                    messageDisplayer.UpdateRichTextBox("FFmpegが見つかりません。\nパス設定を確認してください。");
                    installFlg++;
                }

                if (installFlg > 0)
                {
                    messageDisplayer.UpdateRichTextBox("❌ ダウンロード実行前に必要なツールをインストールしてください。");

                    DialogResult result = MessageBox.Show(
                        "PythonまたはFFmpegが見つかりません。一括インストールを実施しますか？",
                        "確認",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.OK)
                    {
                        InstallSpotDL();
                    }

                    return false;
                }

                // SpotDLのCLIに存在する引数だけを渡す。
                // 認証が有効な場合だけ OAuth ログインを付ける。
                string spotifyArguments = "";
                if (useSpotifyAuth)
                {
                    spotifyArguments += " --user-auth";
                }

                string arguments =
                    $"/c chcp 65001 > nul && \"{pythonExe}\" -m spotdl download \"{playlistUrl}\" " +
                    $"--output \"{outputFolder}\" --ffmpeg \"{FFmpegExe}\" {spotifyArguments}";

                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                };

                psi.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";

                if (useSpotifyAuth)
                {
                    messageDisplayer.UpdateRichTextBox("▶ Spotify認証を使って実行します。初回はブラウザ認証が開くことがあります。", true);
                }
                else
                {
                    messageDisplayer.UpdateRichTextBox("▶ Spotify認証なしで実行します。レート制限に当たりやすい場合があります。", true);
                }

                var outputCompletion = new TaskCompletionSource<bool>();
                var errorCompletion = new TaskCompletionSource<bool>();

                int exitCode = -1;

                using (var process = new Process { StartInfo = psi })
                {
                    process.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputCompletion.TrySetResult(true);
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(e.Data))
                        {
                            messageDisplayer.UpdateRichTextBox(e.Data);
                        }
                    };

                    process.ErrorDataReceived += (s, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorCompletion.TrySetResult(true);
                            return;
                        }

                        string data = e.Data;

                        if (data.Contains("rate/request limit") || data.Contains("Retry will occur after"))
                        {
                            rateLimitError = true;

                            string waitMsg = ParseRetryWaitMessage(data);
                            SaveSpotdlLimitTimeFromErrorLine(data);
                            messageDisplayer.UpdateRichTextBox(
                                $"⚠ SpotDL APIのレート制限に達しました。{waitMsg}",
                                true);

                            messageDisplayer.UpdateRichTextBox(
                                "短時間で再実行すると制限が長引く可能性があります。時間を空けてから再試行してください。",
                                true);
                        }

                        if (data.Contains("FFmpegError"))
                        {
                            ffmpegErrorCount++;
                        }

                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            messageDisplayer.UpdateRichTextBox(data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await Task.WhenAll(
                        Task.Run(() => process.WaitForExit()),
                        outputCompletion.Task,
                        errorCompletion.Task
                    );

                    exitCode = process.ExitCode;
                }

                ProcessKills("ffmpeg");

                if (rateLimitError)
                {
                    messageDisplayer.UpdateRichTextBox("");
                    messageDisplayer.UpdateRichTextBox("❌ ダウンロードを中止しました。SpotDL側の取得制限が解除されてから再実行してください。", true);
                    return false;
                }

                if (exitCode == 0)
                {
                    downloadSuccess = true;
                }

                if (downloadSuccess)
                {
                    messageDisplayer.UpdateRichTextBox("✅ ダウンロード完了しました。");
                }
                else
                {
                    messageDisplayer.UpdateRichTextBox($"❌ ダウンロードに失敗しました。終了コード: {exitCode}", true);
                }

                if (ffmpegErrorCount > 0)
                {
                    DialogResult result = MessageBox.Show(
                        "FFmpegエラーが発生しています。今直ぐにFFmpegのキャッシュをクリアしますか？",
                        "確認",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Exclamation);

                    if (result == DialogResult.OK)
                    {
                        ClearffmpegError();
                    }
                }

                return downloadSuccess;
            }
            catch (Exception ex)
            {
                messageDisplayer.UpdateRichTextBox($"❌ Spotifyダウンロードエラー: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// </summary>
        private async Task<bool> DownloadYouTubeAsync(string videoUrl, string outputFolder)
        {
            try
            {
                clsIniData objIni = new clsIniData();
                objIni.GetIniData();
                string PythonPath = objIni.getPythonPath;
                string pythonExe = Path.Combine(PythonPath, "python.exe");
                int InstallFlg = 0;

                if (!System.IO.File.Exists(pythonExe))
                {
                    messageDisplayer.UpdateRichTextBox("Pythonが見つかりません。\nパス設定を確認してください。");
                    InstallFlg++;
                }

                if (InstallFlg > 0)
                {
                    messageDisplayer.UpdateRichTextBox("❌ ダウンロード実行前に必要なツールをインストールしてください。");
                    DialogResult result = MessageBox.Show("Pythonが見つかりません。一括インストールを実施しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        InstallSpotDL();
                    }
                    return false;
                }

                string arguments = $"/c chcp 65001 > nul && \"{pythonExe}\" -m yt_dlp -f \"bestaudio/best\" --extract-audio --audio-format mp3 --audio-quality 192K -o \"{outputFolder}/%(title)s.%(ext)s\" \"{videoUrl}\"";
                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                };
                psi.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";

                var outputCompletion = new TaskCompletionSource<bool>();
                var errorCompletion = new TaskCompletionSource<bool>();

                using (var process = new Process { StartInfo = psi })
                {
                    process.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputCompletion.TrySetResult(true);
                            return;
                        }

                        if (e.Data.Contains("http"))
                        {
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(e.Data))
                            messageDisplayer.UpdateRichTextBox(e.Data);
                    };

                    process.ErrorDataReceived += (s, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorCompletion.TrySetResult(true);
                            return;
                        }

                        if (e.Data.Contains("http"))
                        {
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(e.Data))
                            messageDisplayer.UpdateRichTextBox(e.Data);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await Task.WhenAll(
                        Task.Run(() => process.WaitForExit()),
                        outputCompletion.Task,
                        errorCompletion.Task
                    );
                }

                messageDisplayer.UpdateRichTextBox("✅ ダウンロード完了しました。");
                return true;
            }
            catch (Exception ex)
            {
                messageDisplayer.UpdateRichTextBox($"❌ YouTubeダウンロードエラー: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// SpotDL、FFmpegキャッシュクリア
        /// </summary>
        private void ClearffmpegError()
        {
            string spotdlFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".spotdl"
            );

            if (Directory.Exists(spotdlFolder))
            {
                try
                {
                    Directory.Delete(spotdlFolder, true);
                    messageDisplayer.UpdateRichTextBox("⚠️ SpotDLキャッシュフォルダを削除しました。", true);
                }
                catch (Exception ex)
                {
                    messageDisplayer.UpdateRichTextBox($"❌ SpotDLフォルダ削除失敗: {ex.Message}", true);
                }
            }
        }

        /// <summary>
        /// Retry will occur after: XXX s から待機時間を抽出して日本語メッセージに変換
        /// </summary>
        private string ParseRetryWaitMessage(string errorLine)
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                errorLine, @"Retry will occur after:\s*(\d+)\s*s");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int seconds))
            {
                int hours = seconds / 3600;
                int mins = (seconds % 3600) / 60;
                if (hours > 0) return $"約{hours}時間後に再試行できます。";
                if (mins > 0) return $"約{mins}分後に再試行できます。";
                return $"{seconds}秒後に再試行できます。";
            }
            return "しばらく時間をおいてから再試行してください。";
        }

        /// <summary>
        /// 保存済みの制限時刻が有効か確認し、残っているなら実行を止めます。
        /// </summary>
        private bool EnsureSpotdlLimitIsAvailable()
        {
            if (!TryGetStoredSpotdlLimitState(out SpotdlLimitState limitState))
            {
                return true;
            }

            if (!limitState.IsActive)
            {
                ClearStoredSpotdlLimitTime();
                return true;
            }

            messageDisplayer.UpdateRichTextBox($"⚠ 次の{limitState.ToDisplayText()}まで使用できません。", true);
            messageDisplayer.UpdateRichTextBox("SpotDLの取得制限が解除されるまでお待ちください。", true);
            return false;
        }

        /// <summary>
        /// 保存済みの制限時刻を読み込みます。
        /// </summary>
        private bool TryGetStoredSpotdlLimitState(out SpotdlLimitState limitState)
        {
            limitState = SpotdlLimitState.Empty();

            string storedLimitTime = MusicDLWin.Properties.Settings.Default.LimitTime;
            if (string.IsNullOrWhiteSpace(storedLimitTime))
            {
                return false;
            }

            limitState = SpotdlLimitState.FromStorage(storedLimitTime);
            if (!limitState.HasLimit)
            {
                ClearStoredSpotdlLimitTime();
                return false;
            }

            return true;
        }

        /// <summary>
        /// レート制限の解除予定時刻を保存します。
        /// </summary>
        private void SaveSpotdlLimitTimeFromErrorLine(string errorLine)
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                errorLine, @"Retry will occur after:\s*(\d+)\s*s");

            if (!match.Success || !int.TryParse(match.Groups[1].Value, out int seconds) || seconds <= 0)
            {
                return;
            }

            SpotdlLimitState limitState = SpotdlLimitState.FromRetryAfterSeconds(seconds);
            MusicDLWin.Properties.Settings.Default.LimitTime = limitState.ToStorageValue();
            MusicDLWin.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 保存済みの制限時刻を空にします。
        /// </summary>
        private void ClearStoredSpotdlLimitTime()
        {
            if (string.IsNullOrWhiteSpace(MusicDLWin.Properties.Settings.Default.LimitTime))
            {
                return;
            }

            MusicDLWin.Properties.Settings.Default.LimitTime = string.Empty;
            MusicDLWin.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Spotify認証のキャッシュが見つからないときに案内ダイアログを出します。
        /// </summary>
        private bool ShowSpotifyAuthenticationGuideIfNeeded()
        {
            string authCachePath = GetSpotifyAuthCachePath();
            if (System.IO.File.Exists(authCachePath) || Directory.Exists(authCachePath))
            {
                return false;
            }

            using (var dialog = new SpotifyAuthGuideDialog(authCachePath))
            {
                return dialog.ShowDialog(this) == DialogResult.OK;
            }
        }

        /// <summary>
        /// SpotDLのSpotify認証キャッシュの保存先を返します。
        /// </summary>
        private string GetSpotifyAuthCachePath()
        {
            return Path.Combine(Application.StartupPath, ".spotdl-cache");
        }

        /// <summary>
        /// SpotDLの認証をブラウザ付きで開始します。
        /// </summary>
        private void StartSpotDlAuthenticationFlow()
        {
            clsIniData objIni = new clsIniData();
            objIni.GetIniData();

            string pythonPath = objIni.getPythonPath;
            string pythonExe = Path.Combine(pythonPath, "python.exe");
            if (!System.IO.File.Exists(pythonExe))
            {
                messageDisplayer.UpdateRichTextBox("❌ Pythonが見つかりません。SpotDL認証を開けません。", true);
                return;
            }

            string clientId;
            string clientSecret;
            GetSpotdlAuthenticationCredentials(out clientId, out clientSecret);

            string arguments =
                "-c \"from spotdl.search import SpotifyClient; " +
                "SpotifyClient.init(client_id='" + clientId + "', client_secret='" + clientSecret + "', user_auth=True)\"";

            var psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                WorkingDirectory = Application.StartupPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };
            psi.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";

            messageDisplayer.UpdateRichTextBox("▶ SpotDL認証を開始します。ブラウザでSpotifyへログインして許可してください。", true);

            _ = Task.Run(async () =>
            {
                try
                {
                    using (var process = new Process { StartInfo = psi })
                    {
                        var outputCompletion = new TaskCompletionSource<bool>();
                        var errorCompletion = new TaskCompletionSource<bool>();

                        process.OutputDataReceived += (s, e) =>
                        {
                            if (e.Data == null)
                            {
                                outputCompletion.TrySetResult(true);
                                return;
                            }

                            if (!string.IsNullOrWhiteSpace(e.Data))
                            {
                                messageDisplayer.UpdateRichTextBox(e.Data);
                            }
                        };

                        process.ErrorDataReceived += (s, e) =>
                        {
                            if (e.Data == null)
                            {
                                errorCompletion.TrySetResult(true);
                                return;
                            }

                            if (!string.IsNullOrWhiteSpace(e.Data))
                            {
                                messageDisplayer.UpdateRichTextBox(e.Data);
                            }
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        await Task.WhenAll(
                            Task.Run(() => process.WaitForExit()),
                            outputCompletion.Task,
                            errorCompletion.Task
                        );
                    }

                    messageDisplayer.UpdateRichTextBox("✅ SpotDL認証プロセスが終了しました。", true);
                }
                catch (Exception ex)
                {
                    messageDisplayer.UpdateRichTextBox("❌ SpotDL認証の開始に失敗しました: " + ex.Message, true);
                }
            });
        }

        /// <summary>
        /// SpotDLの認証に使う client_id / client_secret を返します。
        /// config.json があれば優先し、なければ SpotDL 既定値を使います。
        /// </summary>
        private void GetSpotdlAuthenticationCredentials(out string clientId, out string clientSecret)
        {
            // spotDL の既定値は公式ドキュメントの default config に合わせる。
            clientId = "5f573c9620494bae87890c0f08a60293";
            clientSecret = "212476d9b0f3472eaa762d90b19b0ba8";

            string configFilePath = Path.Combine(Application.StartupPath, ".spotdl", "config.json");

            if (!System.IO.File.Exists(configFilePath))
            {
                return;
            }

            try
            {
                string json = System.IO.File.ReadAllText(configFilePath);
                string configClientId = ExtractJsonStringValue(json, "client_id");
                string configClientSecret = ExtractJsonStringValue(json, "client_secret");

                if (!string.IsNullOrWhiteSpace(configClientId))
                {
                    clientId = configClientId;
                }

                if (!string.IsNullOrWhiteSpace(configClientSecret))
                {
                    clientSecret = configClientSecret;
                }
            }
            catch
            {
                // 認証案内は既定値で続行する。
            }
        }

        /// <summary>
        /// JSON文字列から指定キーの文字列値を取り出します。
        /// </summary>
        private string ExtractJsonStringValue(string json, string key)
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                json,
                "\"" + System.Text.RegularExpressions.Regex.Escape(key) + "\"\\s*:\\s*\"([^\"]+)\"");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        /// <summary>
        /// Spotify認証の案内ダイアログです。
        /// </summary>
        private sealed class SpotifyAuthGuideDialog : Form
        {
            public SpotifyAuthGuideDialog(string authCachePath)
            {
                this.Text = "Spotify認証案内";
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MinimizeBox = false;
                this.MaximizeBox = false;
                this.ClientSize = new System.Drawing.Size(640, 310);
                this.BackColor = System.Drawing.Color.FromArgb(30, 44, 38);
                this.ForeColor = System.Drawing.Color.WhiteSmoke;

                var messageLabel = new Label
                {
                    AutoSize = false,
                    Location = new System.Drawing.Point(16, 16),
                    Size = new System.Drawing.Size(604, 118),
                    Text =
                        "Spotify認証は、SpotDLが起動する認証フローで完了します。\n" +
                        "SpotDL認証を開く を押すと、ブラウザで許可画面が開きます。\n" +
                        "Spotifyへログインして許可を押したあと、127.0.0.1:9900 に戻れば認証完了です。\n" +
                        "もしすでに『24時間待ち』が出ている場合は、この操作だけではすぐ解除されません。"
                };

                var cacheLabel = new Label
                {
                    AutoSize = false,
                    Location = new System.Drawing.Point(16, 140),
                    Size = new System.Drawing.Size(604, 36),
                    Text = "認証キャッシュの保存先: " + authCachePath
                };

                var cautionLabel = new Label
                {
                    AutoSize = false,
                    Location = new System.Drawing.Point(16, 180),
                    Size = new System.Drawing.Size(604, 42),
                    Text = "SpotDL認証を開くボタンを押したあと、ブラウザ側でSpotifyの許可を最後まで進めてください。"
                };

                var openAuthButton = new Button
                {
                    Location = new System.Drawing.Point(356, 236),
                    Size = new System.Drawing.Size(140, 30),
                    Text = "SpotDL認証を開く",
                    DialogResult = DialogResult.OK
                };

                var closeButton = new Button
                {
                    Location = new System.Drawing.Point(510, 236),
                    Size = new System.Drawing.Size(86, 30),
                    Text = "閉じる",
                    DialogResult = DialogResult.Cancel
                };

                this.AcceptButton = openAuthButton;
                this.CancelButton = closeButton;
                this.Controls.Add(messageLabel);
                this.Controls.Add(cacheLabel);
                this.Controls.Add(cautionLabel);
                this.Controls.Add(openAuthButton);
                this.Controls.Add(closeButton);
            }
        }

        /// <summary>
        /// MP3のプロパティにアルバム名とトラック番号を付け加える
        /// </summary>
        /// <param name="filePath">MP3ファイルの読込先</param>
        /// <param name="newAlbum">アルバム名</param>
        /// <param name="newTrackNumber">トラック番号</param>
        private string UpdateMp3Properties(string filePaths, string newAlbum)
        {
            // MP3ファイルを読み込む
            string directoryPath = filePaths.Trim();
            string ErrorMsg = "";
            uint newTrackNumber = 1;
            foreach (string filePath in Directory.EnumerateFiles(directoryPath, "*.mp3"))
            {
                try
                {
                    TagLib.File mp3File = TagLib.File.Create(filePath);
                    mp3File.Tag.Album = textAlbumName.Text.Trim();
                    mp3File.Tag.Track = newTrackNumber;
                    mp3File.Save();
                    newTrackNumber++;
                }
                catch (TagLib.UnsupportedFormatException)
                {
                    ErrorMsg = $"サポートされていない形式: {filePath}";
                    break;
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message.ToString();
                    break;
                }
            }
            return ErrorMsg;
        }
        #endregion

        #region "PythonとSpotDLAPIとFFmpegの一括インストール(Pythonの仮想化環境にインストール)"
        /// <summary>
        /// PythonとSpotDLAPIとFFmpegの一括インストール(Pythonの仮想化環境にインストール
        /// </summary>        
        private async void InstallSpotDL()
        {
            this.Invoke(new Action(() =>
            {
                this.StopButton.Enabled = false;
                StartTimerProgresBar();
            }));
            clsInstall install = new clsInstall(this.ResultText);
            bool okngPython = await install.InstallPython();
            if (okngPython)
            {
               messageDisplayer.UpdateRichTextBox("PythonとSpotDLAPIのインストールが完了しました。");
            }

            bool okngFFmpeg = await install.InstallFFmpeg();
            if (okngFFmpeg)
            {
                messageDisplayer.UpdateRichTextBox("FFmpegのインストールが完了しました。");
            }
            this.Invoke(new Action(() => StopTimerProgresBar()));
            install.Dispose();

            // インストール完了後にMusicDLWin.exeを再起動する
            if ((okngPython || okngFFmpeg) && MessageBox.Show("インストールが完了しました。アプリケーションを再起動しますか？", "再起動確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                // 設定ファイルが確実に保存されるよう待機
                System.Threading.Thread.Sleep(2000);

                this.CloseFlg = true;
                string exePath = Application.ExecutablePath;
                Process.Start(exePath);
                this.Close();
                Application.Exit();
                return;  // 再起動時は以下の処理をスキップ
            }

            // フォームがまだ有効な場合のみ UI 更新
            if (!this.IsDisposed)
            {
                this.Invoke(new Action(() => this.StopButton.Enabled = true));
            }
        }
        #endregion

        #region "PythonとFFmpegのインストールチェック"
        /// <summary>
        /// PythonとFFmpegのインストールチェック
        /// </summary>
        /// <returns></returns>
        private async Task CheckPythonAndFFmpegAsync()
        {
            clsIniData objIni = new clsIniData();
            objIni.GetIniData();

            messageDisplayer.UpdateRichTextBox("【依存ツール確認中】");

            // Pythonのパスチェック（.venv自動検出）
            string pythonPath = objIni.getPythonPath;
            bool pythonInstalled = false;

            if (string.IsNullOrEmpty(pythonPath) || !Directory.Exists(pythonPath))
            {
                string defaultVenvPath = Path.Combine(Application.StartupPath, ".venv", "Scripts");
                if (Directory.Exists(defaultVenvPath))
                {
                    messageDisplayer.UpdateRichTextBox("▶ .venv を自動検出しました。パスを設定しています...");
                    pythonPath = defaultVenvPath;
                    objIni.SetPythonPath(pythonPath);
                    pythonInstalled = true;
                }
            }
            else
            {
                pythonInstalled = true;
            }

            if (pythonInstalled)
            {
                messageDisplayer.UpdateRichTextBox("✅ Pythonのパス: " + pythonPath);
            }
            else
            {
                messageDisplayer.UpdateRichTextBox("❌ Pythonがインストールされていません。");
            }

            // FFmpegのパスチェック
            bool ffmpegInstalled = false;
            if (!string.IsNullOrEmpty(objIni.getFFmpegPath) && Directory.Exists(objIni.getFFmpegPath))
            {
                messageDisplayer.UpdateRichTextBox("✅ FFmpegのパス: " + objIni.getFFmpegPath);
                ffmpegInstalled = true;
            }
            else
            {
                messageDisplayer.UpdateRichTextBox("❌ FFmpegがインストールされていません。");
            }

            // spotdlのインストール確認
            bool spotdlInstalled = false;
            if (pythonInstalled)
            {
                string spotdlExe = Path.Combine(pythonPath, "spotdl.exe");
                if (System.IO.File.Exists(spotdlExe))
                {
                    messageDisplayer.UpdateRichTextBox("✅ SpotDLがインストールされています。");
                    spotdlInstalled = true;
                }
            }

            // yt-dlpのインストール確認
            bool ytdlpInstalled = false;
            if (pythonInstalled)
            {
                string pythonExe = Path.Combine(pythonPath, "python.exe");
                var psi = new ProcessStartInfo("cmd.exe", $"/c \"{pythonExe}\" -m yt_dlp --version")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(psi))
                {
                    process?.WaitForExit();
                    if (process?.ExitCode == 0)
                    {
                        messageDisplayer.UpdateRichTextBox("✅ yt-dlpがインストールされています。");
                        ytdlpInstalled = true;
                    }
                }
            }

            if (pythonInstalled && ffmpegInstalled && spotdlInstalled && ytdlpInstalled)
            {
                messageDisplayer.UpdateRichTextBox("✅ すべての依存ツールが確認できました。");
            }
            else
            {
                // 欠けているツールを表示
                var missingTools = new List<string>();
                if (!pythonInstalled) missingTools.Add("Python");
                if (!ffmpegInstalled) missingTools.Add("FFmpeg");
                if (!spotdlInstalled) missingTools.Add("SpotDL");
                if (!ytdlpInstalled) missingTools.Add("yt-dlp");

                messageDisplayer.UpdateRichTextBox("");
                messageDisplayer.UpdateRichTextBox("【インストールが必要なツール】");
                foreach (var tool in missingTools)
                {
                    messageDisplayer.UpdateRichTextBox("▶ " + tool);
                }
                messageDisplayer.UpdateRichTextBox("");

                string missingToolsStr = string.Join("、", missingTools);
                DialogResult result = MessageBox.Show(missingToolsStr + " をインストールします。よろしいですか？", "インストール確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    ProcessKills("ffmpeg");
                    ProcessKills("spotdl");
                    InstallSpotDL();
                }
                else
                {
                    messageDisplayer.UpdateRichTextBox("⚠ インストール手順をスキップしました。");
                }
                // spotdlのバージョンチェック（すべてのツールがインストール済みの場合のみ）
                if (pythonInstalled && ffmpegInstalled && spotdlInstalled && ytdlpInstalled)
                {
                    messageDisplayer.UpdateRichTextBox("");
                    messageDisplayer.UpdateRichTextBox("【spotdlバージョン確認】");

                    clsInstall install = new clsInstall(this.ResultText);
                    bool versionCheckResult = await install.CheckSpotdlVersionAsync();
                    install.Dispose();

                    if (versionCheckResult)
                    {
                        // spotdlの最新版をインストール確認
                        DialogResult updateResult = MessageBox.Show("spotdlに最新版があります。インストールしますか？", "spotdlアップデート確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (updateResult == DialogResult.OK)
                        {
                            messageDisplayer.UpdateRichTextBox("");
                            messageDisplayer.UpdateRichTextBox("▶spotdlを最新版にアップデートしています...");
                            ProcessKills("spotdl");

                            clsInstall updateInstall = new clsInstall(this.ResultText);
                            string pythonPathvenv = Path.Combine(Application.StartupPath, ".venv", "Scripts");
                            string pythonExe = Path.Combine(pythonPathvenv, "python.exe");

                            if (System.IO.File.Exists(pythonExe))
                            {
                                await updateInstall.UpdateSpotdl(pythonExe);
                                messageDisplayer.UpdateRichTextBox("✅spotdlをアップデートしました。");
                            }
                            updateInstall.Dispose();
                        }
                    }
                    else
                    {
                        messageDisplayer.UpdateRichTextBox("⚠ spotdlのバージョン確認ができませんでした。");
                    }
                }
            }
        }
        #endregion

        #region "イベント関係"
        /// <summary>
        /// SpotDLのロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SpotDL_Load(object sender, EventArgs e)
        {
            //フォームの初期化
            SetPrgBar();
            ReadConfigData();

            //OutPutフォルダの削除
            CheckOutPutDeleteFiles();

            //タイマーを非活性にセット
            Time.Enabled = false;

            //RichTextBoxをメッセージプレイヤーにセット
            MessageDisplayer messageDisplayer = new MessageDisplayer(this.ResultText);

            // デバッグ情報
            messageDisplayer.UpdateRichTextBox("【アプリケーション情報】");
            messageDisplayer.UpdateRichTextBox("実行ディレクトリ: " + Application.StartupPath);
            messageDisplayer.UpdateRichTextBox("");

            await CheckPythonAndFFmpegAsync();
        }

        /// <summary>
        /// フォーム表示後にSpotify認証案内を出します。
        /// </summary>
        private void SpotDL_Shown(object sender, EventArgs e)
        {
            if (ShowSpotifyAuthenticationGuideIfNeeded())
            {
                StartSpotDlAuthenticationFlow();
            }

            if (TryGetStoredSpotdlLimitState(out SpotdlLimitState limitState) && !limitState.IsActive)
            {
                ClearStoredSpotdlLimitTime();
            }
        }

        /// <summary>
        /// メニューダウンロードボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ダウンロードToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorksFiles();
        }

        /// <summary>
        /// メニュー出力先ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 出力先ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveMakeFolder();
        }

        /// <summary>
        /// メニュー終了ボタン押下
        /// </summary>
        /// <param sender="sender"></param>
        /// <param name="e"></param>
        private void アプリを終了しますToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("MusicDLWindowsを終了しますか？", "MusicDLWindows", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                this.Close();
                this.Dispose();
            }
        }

        /// <summary>
        /// ヘルプボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ヘルプToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHelp help = new frmHelp();
            help.ShowDialog();
        }

        /// <summary>
        /// ダウンロードボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Download_Click(object sender, EventArgs e)
        {
            WorksFiles();
        }

        /// <summary>
        /// 出力先ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            MoveMakeFolder();
        }

        /// <summary>
        /// MusicDLクローズイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MusicDL_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.CloseFlg) {
                //再起動時
                ProcessKills("ffmpeg");
                ProcessKills("spotdl");
                this.Dispose();
                Application.Exit();
            } else {
                //通常終了時
                DialogResult result = MessageBox.Show("MusicDLを強制終了させます。", "終了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result==DialogResult.OK) {
                    ProcessKills("ffmpeg");
                    ProcessKills("spotdl");
                    this.Dispose();
                    Application.Exit();
                }
            };
        }

        /// <summary>
        /// ダウンロード停止ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ダウンロードを停止させます。よろしいですか？。", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    ProcessKills("ffmpeg");
                    ProcessKills("spotdl");
                    StopTimerProgresBar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                MessageBox.Show("ダウンロード停止させました。", "ダウンロード停止", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Download.Enabled = true;
            }
            ;
        }

        /// <summary>
        /// タイマーイベント
        /// </summary>
        /// <param sender="sender"></param>
        /// <param name="e"></param>
        private void Time_Tick(object sender, EventArgs e)
        {
            PrgBar.Value = PrgBar.Value > 99 ? PrgBar.Minimum : ++PrgBar.Value;
            Application.DoEvents();
        }

        /// <summary>
        /// Helpを表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Helpを表示
            frmHelp help = new frmHelp();
            help.ShowDialog();

        }

        /// <summary>
        /// Pythonパス設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pythonパス設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PythonFinder pythonFinder = new PythonFinder(this.ResultText);
            var pythonDirs = pythonFinder.GetPythonFinder();

            clsIniData objIni = new clsIniData();
            objIni.GetIniData();
            ShowPathSetting(pythonDirs,"Python");
        }

        /// <summary>
        /// FFmpegパス設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FFmpegパス設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FFmpegFinder FFmpegFinder = new FFmpegFinder(this.ResultText);
            var FFmpegDirs = FFmpegFinder.GetFFmpegFinder();

            clsIniData objIni = new clsIniData();
            objIni.GetIniData();
            ShowPathSetting(FFmpegDirs, "FFmpeg");
        }

        /// <summary>
        /// PythonとSpotDLAPIとFFmpegの一括インストール
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 一括ｲﾝｽﾄｰﾙToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // PythonとSpotDLAPIとFFmpegとyt-dlpの一括インストール
            if (MessageBox.Show("Python、FFmpeg、SpotDL、yt-dlpの一括インストールを行います。よろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ProcessKills("ffmpeg");
                ProcessKills("spotdl");
                InstallSpotDL();
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}
