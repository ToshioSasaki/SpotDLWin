using Raven.Abstractions.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Hosting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        #endregion

        #region "コンストラクタ"
        public frmMusicDL()
        {
            InitializeComponent();
            messageDisplayer = new MessageDisplayer(this.ResultText);
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

        #region "MP3ファイルのみ全て削除します。"
        /// <summary>
        /// MP3ファイルを全て削除します。
        /// </summary>
        private void DeleteMP3OutPutFile()
        {
            if (Directory.Exists(textOutDir.Text.Trim()))
            {
                string[] mp3Files = Directory.GetFiles(textOutDir.Text.Trim(), "*mp3");
                // 各MP3ファイルを削除
                foreach (string file in mp3Files)
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (IOException e)
                    {
                        messageDisplayer.UpdateRichTextBox($"出力先のファイルを削除できませんでした。削除できなかったファイル: {file}. Error: {e.Message}");
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

                //MP3ファイルを削除
                DeleteMP3OutPutFile();

                //YouTubeから該当ファイルをダウンロード
                await DownloadPlaylistAsync(playlistUrl, outputFolder);

                //テンプフォルダから正式なフォルダへコピー
                //bool success = CopyMp3File();
                string updateMessage = UpdateMp3Properties(textOutDir.Text.Trim(), textAlbumName.Text.Trim()) == "" ? "成功" : "失敗";

                //終了時間の打刻
                now = DateTime.Now;
                sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                string Argument = "■ダウンロード終了 ファイルアップデート(" + updateMessage + ")" + sYmd + sHms;
                messageDisplayer.UpdateRichTextBox(Argument);
            }
            this.StopTimerProgresBar();
        }

        /// <summary>
        /// ダウンロード読込み処理：URLの種別に応じたダウンロード実行
        /// </summary>
        /// <param name="playlistUrl">リンク</param>
        /// <param name="outputFolder">MP3アウトプットフォルダ</param>
        /// <returns>タスク</returns>
        private async Task DownloadPlaylistAsync(string playlistUrl, string outputFolder)
        {
            try
            {
                Directory.CreateDirectory(outputFolder);

                string urlType = GetUrlType(playlistUrl);

                if (urlType == "spotify")
                {
                    messageDisplayer.UpdateRichTextBox("▶ SpotifyのURLを検出しました。SpotDLでダウンロードします...");
                    await DownloadSpotifyAsync(playlistUrl, outputFolder);
                }
                else if (urlType == "youtube")
                {
                    messageDisplayer.UpdateRichTextBox("▶ YouTubeのURLを検出しました。yt-dlpでダウンロードします...");
                    await DownloadYouTubeAsync(playlistUrl, outputFolder);
                }
                else
                {
                    messageDisplayer.UpdateRichTextBox("❌ サポートされていないURLです。SpotifyまたはYouTubeのURLを入力してください。");
                }
            }
            catch (Exception ex)
            {
                messageDisplayer.UpdateRichTextBox($"❌ エラー発生: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Spotify ダウンロード処理
        /// </summary>
        private async Task DownloadSpotifyAsync(string playlistUrl, string outputFolder)
        {
            try
            {
                int ffmpegErrorCount = 0;

                clsIniData objIni = new clsIniData();
                objIni.GetIniData();
                string PythonPath = objIni.getPythonPath;
                string FFmpegPath = objIni.getFFmpegPath;
                string pythonExe = Path.Combine(PythonPath, "python.exe");
                string FFmpegExe = Path.Combine(FFmpegPath, "ffmpeg.exe");
                int InstallFlg = 0;

                if (!System.IO.File.Exists(pythonExe))
                {
                    messageDisplayer.UpdateRichTextBox("Pythonが見つかりません。\nパス設定を確認してください。");
                    InstallFlg++;
                }

                if (!System.IO.File.Exists(FFmpegExe))
                {
                    messageDisplayer.UpdateRichTextBox("FFmpegが見つかりません。\nパス設定を確認してください。");
                    InstallFlg++;
                }

                if (InstallFlg > 0)
                {
                    messageDisplayer.UpdateRichTextBox("❌ ダウンロード実行前に必要なツールをインストールしてください。");
                    DialogResult result = MessageBox.Show("PythonまたはFFmpegが見つかりません。一括インストールを実施しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        InstallSpotDL();
                    }
                    return;
                }

                string arguments = $"/c chcp 65001 > nul && \"{pythonExe}\" -m spotdl \"{playlistUrl}\" --output \"{outputFolder}\" --ffmpeg \"{FFmpegExe}\" --bitrate 192k";
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

                        if (e.Data.Contains("FFmpegError"))
                        {
                            ffmpegErrorCount++;
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

                ProcessKills("ffmpeg");
                messageDisplayer.UpdateRichTextBox("✅ ダウンロード完了しました。");

                if (ffmpegErrorCount > 0)
                {
                    DialogResult result = MessageBox.Show("FFmpegエラーが発生しています。今直ぐにFFmpegのキャッシュをクリアしますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (DialogResult.OK == result)
                    {
                        ClearffmpegError();
                    }
                }
            }
            catch (Exception ex)
            {
                messageDisplayer.UpdateRichTextBox($"❌ Spotifyダウンロードエラー: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// YouTube ダウンロード処理
        /// </summary>
        private async Task DownloadYouTubeAsync(string videoUrl, string outputFolder)
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
                    return;
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
            }
            catch (Exception ex)
            {
                messageDisplayer.UpdateRichTextBox($"❌ YouTubeダウンロードエラー: {ex.Message}\n{ex.StackTrace}");
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
                    InstallSpotDL();
                }
                else
                {
                    messageDisplayer.UpdateRichTextBox("⚠ インストール手順をスキップしました。");
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
            };
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
            // PythonとSpotDLAPIとFFmpegの一括インストール
            if (MessageBox.Show("PythonとSpotDLAPIとFFmpegの一括インストールを行います。よろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ProcessKills("ffmpeg");
                ProcessKills("spotdl");
            }
            else
            {
                return;
            }
            InstallSpotDL();
        }
        #endregion
    }
}
