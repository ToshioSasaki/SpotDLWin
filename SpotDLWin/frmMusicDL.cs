using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Hosting;
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
        ProcessStartInfo processStartInfo { get; set; } = null;
        private Process process { get; set; } = null;
        private bool stop { get; set; }
        private int Kaisuu { get; set; }
        private int TimeOut { get; set; }
        #endregion

        #region "コンストラクタ"
        public frmMusicDL()
        {
            InitializeComponent();
            messageDisplayer = new MessageDisplayer(this.ResultText);
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
                ProcessKills();
                //ローカルファイルのMP3ファイルをすべて消去する
                DeleteMP3LocalFile();

                //作業開始
                DateTime now = DateTime.Now;
                string sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                string sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                messageDisplayer.UpdateRichTextBox("■ダウンロード作業を開始します。(" + sYmd + sHms + ")■");

                //SpotDLからのタイトル名取得時のPythonの文字化けエラーを防ぐ
                SetCodePageToUTF8();

                //URLダウンロード
                string Argument = "python -m spotdl download " + inputTextBox.Text.Trim() + " --max-retries " + this.Kaisuu;
                await ExecuteCommand(Argument);

                //ディレクトリ表示
                int iDirctory = textOutDir.Text.Trim().LastIndexOf("\\");
                string Directory = textOutDir.Text.Trim().Substring(0, iDirctory);
                Argument = "cd " + Directory;
                await ExecuteCommand(Argument);
                Argument = "dir *.mp3";
                await ExecuteCommand(Argument);

                //終了ステートメント
                DeleteMP3OutPutFile();
                bool success = CopyMp3File();
                string updateMessage = UpdateMp3Properties(textOutDir.Text.Trim(), textAlbumName.Text.Trim()) == "" ? "成功" : "失敗";
                now = DateTime.Now;
                sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                string message = (success) ? "ファイルコピー成功" : "ファイルコピー失敗";
                Argument = "■ダウンロード終了 " + message + " ファイルアップデート(" + updateMessage + ")" + sYmd + sHms;
                messageDisplayer.UpdateRichTextBox(Argument);
            }
            this.StopTimerProgresBar();
        }
        #endregion


        #region "起動しているプロセスの強制終了"
        /// <summary>
        /// 起動プロセスの終了
        /// <param name="stop">ダウンロード停止＝trueの時</param>
        /// </summary>
        private void ProcessKills(bool stop = false)
        {
            // 対象のexeファイル名
            string exeFileName = "ffmpeg";
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

        #region "文字化け対策"
        /// <summary>
        /// SpotDLからのタイトル名取得時のPythonの文字化けエラーを防ぐ
        /// </summary>
        /// <returns></returns>
        private void SetCodePageToUTF8()
        {
            ProcessStartInfo processStartInfos = new ProcessStartInfo("cmd.exe", "/c chcp 65001")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process processes = new Process())
            {
                processes.StartInfo = processStartInfos;

                processes.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        // コードページ変更の出力をキャプチャする場合、ここで処理
                        Console.WriteLine(e.Data);
                    }
                };

                processes.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        // エラーメッセージを表示
                        messageDisplayer.UpdateRichTextBox("Error: " + e.Data);
                    }
                };

                processes.Start();
                processes.BeginOutputReadLine();
                processes.BeginErrorReadLine();

                processes.WaitForExit();
            }
        }
        #endregion


        #region "Phytonのアップデート処理"
        /// <summary>
        /// Phytonのアップデート
        /// </summary>
        private async void PhytonUpdate()
        {
            //最新バージョンの更新
            string Argument = "choco upgrade python";
            await ExecuteCommand(Argument);
        }
        #endregion

        #region "MusicDLのアップデート処理"
        /// <summary>
        /// SpotDLのアップデート
        /// </summary>
        private async void MusicDLUpdate()
        {
            // SpotDL がインストールされているか確認
            if (!await IsSpotDLInstalled())
            {
                if (MessageBox.Show("SpotDLがインストールされていません。インストールしますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // SpotDL のインストールコマンドを実行
                    string installCommand = "pip install spotdl";
                    await ExecuteCommand(installCommand);
                }
                else
                {
                    // SpotDL の公式サイトへリダイレクトする処理（ブラウザを開く）
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://github.com/spotDL/spotify-downloader",
                        UseShellExecute = true
                    });
                    return;
                }
            }

            // SpotDL のアップデートコマンド
            string updateCommand = "pip upgrade spotdl";
            await ExecuteCommand(updateCommand);
        }

        /// <summary>
        /// SpotDLのインストールを実施するか否か
        /// </summary>
        /// <returns>True：実施する</returns>
        private async Task<bool> IsSpotDLInstalled()
        {
            // SpotDL がインストールされているかを確認するために `pip show spotdl` を実行
            string checkCommand = "pip install spotdl";
            bool isInstalled = true;

            ProcessStartInfo processStartInfos = new ProcessStartInfo("cmd.exe", "/c " + checkCommand)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process processes = new Process())
            {
                processes.StartInfo = processStartInfos;
                processes.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        isInstalled = false; // 出力があれば SpotDL がインストールされていると判断
                    }
                };
                processes.Start();
                processes.BeginOutputReadLine();
                await Task.Run(() => processes.WaitForExit());
            }

            return isInstalled;
        }
        #endregion

        #region "MP3ファイルにアルバム名とトラック番号を付ける"
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
            uint newTrackNumber = 0;
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
        #endregion

        #region "フォルダのダイアログを開きパスを指定します。"
        /// <summary>
        /// ダイアログを開きパスを指定します。
        /// </summary>
        private void MoveMakeFolder()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
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

        #region "ファイルコピー処理"
        /// <summary>
        /// OUTPUT_DIRにMP3ファイルをコピーします。
        /// </summary>
        /// <returns>True:成功、False：失敗</returns>
        private bool CopyMp3File()
        {
            string AppPath = Application.StartupPath + "\\";
            //コピー先フォルダ
            string destinationFolder = textOutDir.Text.Trim();
            //コピー先フォルダの削除

            try
            {
                if (Directory.Exists(AppPath) && Directory.Exists(destinationFolder))
                {
                    //カレントディレクトリのフォルダのMP3を取得
                    string[] mp3Files = Directory.GetFiles(AppPath, "*.mp3");

                    foreach (string mp3File in mp3Files)
                    {
                        string fileName = Path.GetFileName(mp3File);
                        string destinationPath = Path.Combine(destinationFolder, fileName);

                        // MP3ファイルをコピー
                        System.IO.File.Copy(mp3File, destinationPath);

                        // コピー元のMP3ファイルを削除
                        System.IO.File.Delete(mp3File);
                    }

                }
                return true;
            }
            catch (Exception)
            {
                return false;
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
                        System.IO.File.Delete(destinationPath);
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

        #region "コマンドプロンプト動作処理"
        /// <summary>
        /// その他コマンドの実行
        /// </summary>
        /// <param name="Argument">コマンド</param>
        /// <return>コマンド実行メッセージ</return>
     
        private async Task ExecuteCommand(string Argument)
        {
            //プロセススタートInfoのインスタンスを作成＆各プロパティを設定
            ProcessStartInfo processStartInfos = new ProcessStartInfo("powershell.exe", $"-Command {Argument}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            //コマンドプロンプトを実行させる為のインスタンスを作成する
            using (Process processes = new Process())
            {
                processes.StartInfo = processStartInfos;

                //ログの表示
                processes.OutputDataReceived += (sender, e) =>
                {
                    //プロセスコマンドの通常ログ
                    if (string.IsNullOrEmpty(e.Data) == false) {
                        string message = e.Data.Contains("youtube") || e.Data.Contains("Skipping") ? "" : e.Data.ToString();
                        if (message != "")
                        {
                            messageDisplayer.UpdateRichTextBox(message, true);
                        }
                    }
                };
                processes.ErrorDataReceived += (sender, e) =>
                {
                    //プロセスコマンドのエラーログイベント
                    messageDisplayer.UpdateRichTextBox(e.Data, true);
                };

                //進行状況の表示
                messageDisplayer.UpdateRichTextBox("コマンドの実行を開始しました。", true);
                processes.Start();
                processes.BeginOutputReadLine();
                processes.BeginErrorReadLine();

                //プロセスの待機
                bool isCompleted = await Task.Run(() => processes.WaitForExit(this.TimeOut));

                if (processes.HasExited)
                {
                    //プロセスが終了している場合
                    messageDisplayer.UpdateRichTextBox("プロセスが終了しました。", true);

                } else if (!isCompleted)
                {
                    messageDisplayer.UpdateRichTextBox("プロセスがタイムアウト時間に達しました。", true);
                    processes.Kill();
                }
                else
                {
                    //プロセスが終了していない場合
                    messageDisplayer.UpdateRichTextBox("プロセスがまだ処理中です・・。", true);
                }
            }
            messageDisplayer.UpdateRichTextBox("ダウンロードプロセスを全て終了させました。", true);
            ProcessKills();
        }
        #endregion

        #region "コンフィグファイルの読込み＆プログレスバー初期化"
        /// <summary>
        /// Configファイルデータの読込み
        /// </summary>
        private void ReadConfigData()
        {
            //設定ファイルからディレクトリを読込み
            clsIniData objIni = new clsIniData();
            objIni.GetIniData();
            textOutDir.Text = objIni.getOutPath;
            this.Kaisuu = objIni.getKaisuu;
            this.TimeOut = int.Parse(objIni.getTimeOut.ToString());
        }

        /// <summary>
        /// プログレスバーの初期値セット
        /// </summary>
        private void SetProgressBar()
        {
            //プログレスバー
            progressBar1.Value = 0;
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            progressBar1.Enabled = true;
        }
        #endregion

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

        #region "イベント関係"
        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpotDL_Load(object sender, EventArgs e)
        {

            //コンフィグデータの読込み
            ReadConfigData();

            //OutPutフォルダの削除
            this.CheckOutPutDeleteFiles();

            //プログレスバー初期値セット
            SetProgressBar();

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
                ProcessKills();
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
                    stop = true;
                    process.Close();
                    process.Dispose();
                    process.Kill();
                }
                catch (Exception ex)
                {
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
        /// 試行回数を表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 試行回数ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmKaisuu kaisu = new frmKaisuu())
            {
                //コンフィグデータの読込み
                //ReadConfigData();
                kaisu.KaisuuValue = this.Kaisuu;
                kaisu.TimeOutValue = this.TimeOut / 60000;
                if (kaisu.ShowDialog() == DialogResult.OK)
                {
                    this.TimeOut = kaisu.TimeOutValue;
                    this.Kaisuu = kaisu.KaisuuValue;
                }
            }

        }

        /// <summary>
        /// 本ソフトアップデート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("MusicDLをアップデートします。よろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //最新バージョンの更新
                MusicDLUpdate();
            }
        }

        /// <summary>
        /// SpotDL(ffmpeg、pythonのインストールも行う)のインストール
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void 一括ｲﾝｽﾄｰﾙToolStripMenuItem_Click(object sender, EventArgs e)
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
    }
    #endregion


}
