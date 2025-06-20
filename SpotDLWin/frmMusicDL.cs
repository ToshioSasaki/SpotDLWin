﻿using Raven.Abstractions.Util;
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

        #region "メッセージ表示クラス"
        private MessageDisplayer messageDisplayer;
        #endregion

        #region"各メンバ"
        /// <summary>
        /// 各メンバプロパティ
        /// </summary>
        ProgressBar progressBar { get; set; } = new ProgressBar();
        #endregion

        #region "コンストラクタ"
        public frmMusicDL()
        {
            InitializeComponent();
            SetProgressBar();
            ReadConfigData();
            messageDisplayer = new MessageDisplayer(this.ResultText);
        }
        #endregion

        /// <summary>
        /// プログレスバーの初期化
        /// </summary>
        private void SetProgressBar()
        {
            progressBar = this.progressBar1;
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Step = 10;
            progressBar.Value = 0;
            progressBar.Visible = true;
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
            timer1.Enabled = true;
            timer1.Start();
            progressBar1.Value = 0;
        }
        #endregion

        #region "ストップ･タイマー＆プログレスバー"
        /// <summary>
        /// ストップ･タイマー＆プログレスバー
        /// </summary>
        private void StopTimerProgresBar()
        {
            timer1.Stop();
            progressBar1.Value = 0;
            timer1.Enabled = false;
        }
        #endregion

        #region "メイン処理"
        /// <summary>
        /// 作業開始・メイン処理
        /// </summary>
        private async void WorksFiles()
        {
            this.StartTimerProgresBar();
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
        /// SPotDL読込み処理：YouTubeから該当ファイルをダウンロード
        /// </summary>
        /// <param name="playlistUrl">リンク</param>
        /// <param name="outputFolder">MP3アウトプットフォルダ</param>
        /// <returns>タスク</returns>
        private async Task DownloadPlaylistAsync(string playlistUrl, string outputFolder)
        {
            try
            {
                Directory.CreateDirectory(outputFolder);

                int ffmpegErrorCount = 0;

                clsIniData objIni = new clsIniData();
                objIni.GetIniData();
                string PythonPath = objIni.getPythonPath;
                string FFmpegPath = objIni.getFFmpegPath;
                string pythonExe = Path.Combine(PythonPath, "python.exe");
                string FFmpegExe = Path.Combine(FFmpegPath, "ffmpeg.exe");

                if (!System.IO.File.Exists(pythonExe))
                {
                    messageDisplayer.UpdateRichTextBox("Pythonが見つかりません。\nパス設定を確認してください。");
                    return;
                }

                if (!System.IO.File.Exists(FFmpegExe))
                {
                    messageDisplayer.UpdateRichTextBox("FFmpegが見つかりません。\nパス設定を確認してください。");
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

                ProcessKills("ffmpeg");  // ffmpeg はそのままでOK
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
                messageDisplayer.UpdateRichTextBox($"❌ エラー発生: {ex.Message}");
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

        #region "現在未使用"
        #region "SpotDLの一括インストール"
        /// <summary>
        /// SpotDLの一括ｲﾝｽﾄｰﾙ
        /// </summary>
        private void InstallSpotDL()
        {
            Application.DoEvents();
            clsInstall install = new clsInstall(this.ResultText,this.progressBar1);
            bool okngPython = install.InstallPython();
            if (okngPython)
            {
                messageDisplayer.UpdateRichTextBox("Pythonのインストールが完了しました。");
            }
            bool okngFFmpeg = install.InstallFFmpeg();
            if (okngFFmpeg)
            {
                messageDisplayer.UpdateRichTextBox("FFmpegのインストールが完了しました。");
            }
            install.Dispose();
        }
        #endregion

        /// <summary>
        /// Pythonアップデート処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //管理者権限かどうかの取得
            clsAdmin admin = new clsAdmin();
            admin.IsAdministrator();
            bool adm = admin.GetAdmin;

            try
            {
                // Pythonがインストールされているか確認
                if (!IsPythonInstalled())
                {
                    DialogResult result = MessageBox.Show("Pythonがインストールされていません。インストールしますか？", "Pythonのインストール", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        InstallPython();
                    }
                    else
                    {
                        return;
                    }
                }

                // Pythonのアップデートコマンドを実行
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "cmd.exe"; // コマンドプロンプトを使用
                psi.Arguments = "/c python -m pip install --upgrade pip"; // Pythonのアップデートコマンド
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();

                    // 結果を表示
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    DateTime now = DateTime.Now;
                    string sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                    string sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                    if (!string.IsNullOrEmpty(error))
                    {
                        messageDisplayer.UpdateRichTextBox("■Pythonインストール中にエラーが発生しました。(" + sYmd + sHms + ")■");
                    }
                    else
                    {
                        messageDisplayer.UpdateRichTextBox("■Pythonインストールに成功しました。(" + sYmd + sHms + ")■");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("例外が発生しました: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region "Pythonアップデート処理"
        /// <summary>
        /// Pythonがインストールされているかチェック
        /// </summary>
        /// <returns>True：インストールされている</returns>
        private bool IsPythonInstalled()
        {
            try
            {
                // Pythonのバージョンを取得してインストールを確認
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "cmd.exe";
                psi.Arguments = "/c python --version";
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // エラーメッセージがない場合はインストール済みと判断
                    if (string.IsNullOrEmpty(error))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // エラーが発生した場合はインストールされていないと判断
                return false;
            }
            return false;
        }

        /// <summary>
        /// Pythonが入ってない場合新規インストール
        /// </summary>
        private void InstallPython()
        {
            try
            {
                // Pythonのインストーラをダウンロード
                string pythonInstallerUrl = "https://www.python.org/ftp/python/3.10.0/python-3.10.0-amd64.exe";
                string installerPath = Path.Combine(Path.GetTempPath(), "python-installer.exe");

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(pythonInstallerUrl, installerPath);
                }

                // Pythonインストーラを実行
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = installerPath;
                psi.Arguments = "/quiet InstallAllUsers=1 PrependPath=1"; // サイレントインストールでPATHを追加
                psi.UseShellExecute = true;

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit(); // インストールが完了するまで待機
                }

                MessageBox.Show("Pythonのインストールが完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pythonのインストールに失敗しました: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #endregion

        #region "イベント関係"
        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpotDL_Load(object sender, EventArgs e)
        {

            //OutPutフォルダの削除
            CheckOutPutDeleteFiles();

            //タイマーを非活性にセット
            timer1.Enabled = false;

            //RichTextBoxをメッセージプレイヤーにセット
            MessageDisplayer messageDisplayer = new MessageDisplayer(this.ResultText);
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
        /// <param name="sender"></param>
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
            if (MessageBox.Show("MusicDLを強制終了させます。", "終了", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                ProcessKills("ffmpeg");
                ProcessKills("spotdl");
                this.Dispose();
                Application.Exit();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = progressBar1.Value > 99 ? progressBar1.Minimum : ++progressBar1.Value;
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
        private void pythonパス設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PythonFinder pythonFinder = new PythonFinder(this.ResultText);
            var pythonDirs = pythonFinder.GetPythonFinder();

            clsIniData objIni = new clsIniData();
            objIni.GetIniData();
            ShowPathSetting(pythonDirs,"Python");
        }

        /// <summary>
        /// SpotDL(ffmpeg、pythonのインストールも行う)のインストール
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 一括ｲﾝｽﾄｰﾙToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("SpotDLをインストールします。よろしいですか？\nPythonとffmpegのインストールも行います。", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //管理者かどうかの確認
                clsAdmin admin = new clsAdmin();
                admin.IsAdministrator();
                bool adm = admin.GetAdmin;

                //管理者権限かどうかのチェック
                if (adm == false)
                {
                    MessageBox.Show("Phytonとffmpegのインストールは管理者権限で行ってください。\n管理者権限に立ち上げ直して下さい。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //再確認を実施
                    if (MessageBox.Show("一括インストールはセキュリティリスクを伴いますが実行しますか？\n「復元ポイントの作成」の実施をお勧め致します。", "再確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {
                        //一括インストール
                        this.SetProgressBar();
                        this.InstallSpotDL();
                    }
                }
            }
        }

        /// <summary>
        /// FFmpegパス設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fFmpegパス設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FFmpegFinder FFmpegFinder = new FFmpegFinder(this.ResultText);
            var FFmpegDirs = FFmpegFinder.GetFFmpegFinder();

            clsIniData objIni = new clsIniData();
            objIni.GetIniData();
            ShowPathSetting(FFmpegDirs, "FFmpeg");
        }
    }
    #endregion


}
