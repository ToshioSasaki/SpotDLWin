using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;


namespace MusicDLWin
{
    public partial class MusicDL : Form
    {
        private bool stop = false;
        ProcessStartInfo processStartInfo;
        private Process process;
        private int Kaisuu;

        public MusicDL()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 起動プロセスの終了
        /// <param name="stop">ダウンロード停止＝trueの時</param>
        /// </summary>
        private void ProcessKills(bool stop=false)
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
                        UpdateRichTextBox("プロセスファイルが起動していた為終了させました。");
                    }
                }
            }
        }

        /// <summary>
        /// アップデート
        /// </summary>
        private async void Update()
        {
            //最新バージョンの更新
            string command1 = "pip install --upgrade spotdl";
            await downloadExecuteCommand(command1, EnumWork.NONE);
        }

        /// <summary>
        /// 作業開始・メイン処理
        /// </summary>
        private async void WorksFiles()
        {
            
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
                UpdateRichTextBox("■ダウンロード作業を開始します。(" + sYmd + sHms + ")■");

                //URLダウンロード
                string command2 = "spotdl download " + inputTextBox.Text.Trim() + " --max-retries " + this.Kaisuu;
                await downloadExecuteCommand(command2,EnumWork.DOWNLOAD);

                //ディレクトリ表示
                int iDirctory = textOutDir.Text.Trim().LastIndexOf("\\");
                string Directory = textOutDir.Text.Trim().Substring(0, iDirctory);
                string command3 = "cd " + Directory + " && " + "cd " + textOutDir.Text.Trim() + " && dir *.mp3 /O-D";
                await ExecuteCommand(command3);

                //終了ステートメント
                DeleteMP3OutPutFile();
                bool success = CopyMp3File();
                string updateMessage = UpdateMp3Properties(textOutDir.Text.Trim(), textAlbumName.Text.Trim()) == "" ? "成功" : "失敗";
                now = DateTime.Now;
                sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                string message = (success) ? "ファイルコピー成功" : "ファイルコピー失敗";
                string command4 = "echo ■ダウンロード終了 " + message  + " ファイルアップデート(" + updateMessage + ")"　+ sYmd + sHms;
                await ExecuteCommand(command4);

                //プログレスバーとタイマーを初期状態に戻す
                progressBar1.Value = 0;
                timer1.Stop();
            }
        }

        /// <summary>
        /// MP3のプロパティにアルバム名とトラック番号を付け加える
        /// </summary>
        /// <param name="filePath">MP3ファイルの読込先</param>
        /// <param name="newAlbum">アルバム名</param>
        /// <param name="newTrackNumber">トラック番号</param>
        private string  UpdateMp3Properties(string filePaths, string newAlbum)
        {
            // MP3ファイルを読み込む
            string directoryPath = filePaths.Trim();
            string ErrorMsg = "";
            uint newTrackNumber=0;
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

        /// <summary>
        /// インプット、アウトプット、パスのチェック
        /// </summary>
        /// <returns>True：成功、False：エラー</returns>
        private bool CheckDIr()
        {
            if (Directory.Exists(textOutDir.Text.Trim())==false)
            {
                MessageBox.Show("出力先パスが存在しません。","出力先エラー",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return false;
            }
            if (string.IsNullOrEmpty(inputTextBox.Text.Trim()))
            {
                MessageBox.Show("URLエラーです", "URLエラー",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

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
                    UpdateRichTextBox($"ローカルファイルを削除できませんでした。削除できなかったファイル: {file}. Error: {e.Message}");
                }
            }
        }

        /// <summary>
        /// 外部出力先にあるMP3ファイルを全て削除します。
        /// </summary>
        private void DeleteMP3OutPutFile()
        {
            string[] mp3Files = Directory.GetFiles(textOutDir.Text.Trim(),"*mp3");
            // 各MP3ファイルを削除
            foreach (string file in mp3Files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (IOException e)
                {
                    UpdateRichTextBox($"出力先のファイルを削除できませんでした。削除できなかったファイル: {file}. Error: {e.Message}");
                }
            }
        }

        /// <summary>
        /// フォルダダイアログを開きパスを指定します。
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
                    //設定ファイルの変更
                    IniData objIni = new IniData();
                    objIni.SetIniData(textOutDir.Text.Trim());
                }
            }
        }

        /// <summary>
        /// その他コマンドの実行
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <return>コマンド実行メッセージ</return>>
        private async Task ExecuteCommand(string command)
        {
            //プロセススタートInfoのインスタンスを作成＆各プロパティを設定
            ProcessStartInfo　processStartInfos = new ProcessStartInfo("cmd.exe", "/c " + command);
            processStartInfos.RedirectStandardOutput = true;
            processStartInfos.RedirectStandardOutput = true;
            processStartInfos.RedirectStandardError = true;
            processStartInfos.UseShellExecute = false;
            processStartInfos.CreateNoWindow = true;

            //コマンドプロンプトを実行させる為のインスタンスを作成する
            using (Process processes = new Process())
            {
                processes.StartInfo = processStartInfos;
                processes.OutputDataReceived += (sender, e) =>
                {
                    //プロセスコマンドの通常ログ
                    UpdateRichTextBox(e.Data, true);
                };
                processes.ErrorDataReceived += (sender, e) =>
                {
                    //プロセスコマンドのエラーログイベント
                    UpdateRichTextBox(e.Data, true);
                };
                try
                {
                    //プロセスを起動させる
                    processes.Start();
                    processes.BeginOutputReadLine();
                    processes.BeginErrorReadLine();

                    //進行状況の表示
                    UpdateRichTextBox("コマンドの実行を開始しました。", true);
                    //プロセスが起動している状態
                    await Task.Run(() => process.WaitForExit());

                    if (processes.HasExited)
                    {
                        //プロセスが終了している場合
                        UpdateRichTextBox("プロセスが終了しました。", true);
                    }
                    else
                    {
                        //プロセスが終了していない場合
                        UpdateRichTextBox("プロセスがまだ処理中です・・。", true);
                    }
                }
                catch (Exception ex)
                {
                    //エラーがあった場合のログを表示
                    //UpdateRichTextBox("エラー：" + ex.ToString(), true);
                }
                //プロセスの終了と解放
                processes.Close();
                processes.Dispose();
                

                
            }
        }

        /// <summary>
        /// ダウンロードコマンドの実行(スレッド処理)
        /// </summary>
        /// <param name="command">spotdl + URL</param>
        /// <param name="EnumWork">実行パターン</param>
        /// <return>コマンド実行メッセージ</return>
        private async Task downloadExecuteCommand(string command, EnumWork Work)
        {
            //プロセススタートInfoのインスタンスを作成＆各プロパティを設定
            processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;

            //コマンドプロンプトを実行させる為のインスタンスを作成する
            using (process = new Process())
            {

                process.StartInfo = processStartInfo;
                process.OutputDataReceived += (sender, e) =>
                {
                    //プロセスコマンドの通常ログ
                    UpdateRichTextBox(e.Data,true);
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    //プロセスコマンドのエラーログイベント
                    UpdateRichTextBox(e.Data,true);
                };

                try
                {
                    //プロセスを起動させる
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    //進行状況の表示
                    UpdateRichTextBox("コマンドの実行を開始しました。",true);

                    //プロセスの実行を終了迄、待機させる
                    if (EnumWork.DOWNLOAD == Work)
                    {
                        progressBar1.Value = 0;
                        timer1.Enabled = true;
                        timer1.Interval = 1000;
                        timer1.Start();
                    }

                    if (stop && EnumWork.DOWNLOAD == Work)
                    {
                        //プロセス停止ボタンを押下した時
                        stop = false;
                        timer1.Stop();
                    }
                    else
                    {
                        //プロセスが起動している状態
                        await Task.Run(() => process.WaitForExit());

                        if (process.HasExited)
                        {
                            //プロセスが終了している場合
                            if (EnumWork.DOWNLOAD == Work)
                            {
                                stop = false;
                                timer1.Stop();
                                UpdateRichTextBox("ダウンロードが終了しました。", true);
                            } else
                            {
                                UpdateRichTextBox("アップデートが終了しました。", true);
                            }

                        }
                        else
                        {
                            //プロセスが終了していない場合
                            UpdateRichTextBox("プロセスがまだ処理中です・・。", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //エラーがあった場合のログを表示
                    UpdateRichTextBox("エラー：" + ex.ToString(),true);
                }
                //プロセスの終了と解放
                process.Close();
                process.Dispose();
                

                
            }
        }

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

        /// <summary>
        /// スレッドセーフにリッチテキストに表示します
        /// </summary>
        /// <param name="text">表示する文字</param>
        /// <param name="Thread">スレッド時：True</param>
        private void UpdateRichTextBox(string text,bool Thread=false)
        {
            if (text != null)
            {

                if (Thread)
                {
                    try
                    {
                        // スレッドセーフな方法でRichTextBoxを更新
                        Invoke((MethodInvoker)(() =>
                        {
                            ResultText.AppendText(text + "\n");
                            ResultText.ScrollToCaret();
                        }));
                    }
                    catch (Exception ex)
                    {
                        UpdateRichText(ex.Message.ToString());
                    }
                }
                else
                {
                    //通常のシングルスレッドの場合
                    UpdateRichText(text);
                }
            }
        }

        /// <summary>
        /// リッチテキストボックスのシングルスレッドバージョン
        /// </summary>
        /// <param name="text">表示する文字列</param>
        private void UpdateRichText(string text)
        {
            //通常のシングルスレッドの場合
            Application.DoEvents();
            ResultText.AppendText(text + "\n");
            ResultText.ScrollToCaret();
        }

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

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpotDL_Load(object sender, EventArgs e)
        {
            //設定ファイルからディレクトリを読込み
            IniData objIni = new IniData();
            objIni.GetIniData();
            textOutDir.Text = objIni.getOutPath;
            this.Kaisuu = objIni.getKaisuu;

            //OutPutフォルダの削除
            this.CheckOutPutDeleteFiles();

            timer1.Enabled = false;

            //プログレスバー
            progressBar1.Value = 0;
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            progressBar1.Enabled = true;

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
            Help help = new Help();
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
            if (MessageBox.Show("MusicDLを強制終了させます。", "終了", MessageBoxButtons.OK, MessageBoxIcon.Information)==DialogResult.OK)
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
        /// アップデート処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("最新バージョンにアップデートします。よろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //最新バージョンの更新
                Update();
            }
        }

        /// <summary>
        /// Helpを表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Helpを表示
            Help help = new Help();
            help.ShowDialog();

        }

        /// <summary>
        /// 試行回数を表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 試行回数ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (Kaisuu kaisu = new Kaisuu())
            {
                kaisu.KaisuuValue = this.Kaisuu;
                if (kaisu.ShowDialog() == DialogResult.OK)
                {
                    this.Kaisuu = kaisu.KaisuuValue;
                }
            }

        }
    }

    /// <summary>
    /// 列挙ENUM型
    /// </summary>
    public enum EnumWork
    {
        NONE,
        DOWNLOAD,
        COPY,
    }

}
