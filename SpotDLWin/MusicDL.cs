using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Hosting;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib.NonContainer;

namespace MusicDLWin
{
    public partial class MusicDL : Form
    {
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
        public MusicDL()
        {
            InitializeComponent();
        }
        #endregion

        #region "メイン処理"
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
                string Argument = "spotdl download " + inputTextBox.Text.Trim() + " --max-retries " + this.Kaisuu;
                await ExecuteCommand(Argument);

                //ディレクトリ表示
                int iDirctory = textOutDir.Text.Trim().LastIndexOf("\\");
                string Directory = textOutDir.Text.Trim().Substring(0, iDirctory);
                Argument = "cd " + Directory + " && " + "cd " + textOutDir.Text.Trim() + " && dir *.mp3 /O-D";
                await ExecuteCommand(Argument);

                //終了ステートメント
                DeleteMP3OutPutFile();
                bool success = CopyMp3File();
                string updateMessage = UpdateMp3Properties(textOutDir.Text.Trim(), textAlbumName.Text.Trim()) == "" ? "成功" : "失敗";
                now = DateTime.Now;
                sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                string message = (success) ? "ファイルコピー成功" : "ファイルコピー失敗";
                Argument = "echo ■ダウンロード終了 " + message + " ファイルアップデート(" + updateMessage + ")" + sYmd + sHms;
                await ExecuteCommand(Argument);

                //プログレスバーとタイマーを初期状態に戻す
                progressBar1.Value = 0;
                timer1.Stop();
            }
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
                        UpdateRichTextBox("プロセスファイルが起動していた為終了させました。");
                    }
                }
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
        /// MusicDLのアップデート
        /// </summary>
        private async void MusicDLUpdate()
        {
            string Argument = "pip install --upgrade spotdl";
            await ExecuteCommand(Argument);
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
                    UpdateRichTextBox($"ローカルファイルを削除できませんでした。削除できなかったファイル: {file}. Error: {e.Message}");
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
                    UpdateRichTextBox($"出力先のファイルを削除できませんでした。削除できなかったファイル: {file}. Error: {e.Message}");
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
                    //設定ファイルの変更
                    IniData objIni = new IniData();
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

        #region "コマンドプロンプト動作処理"
        /// <summary>
        /// その他コマンドの実行
        /// </summary>
        /// <param name="Argument">コマンド</param>
        /// <return>コマンド実行メッセージ</return>>
        private async Task ExecuteCommand(string Argument)
        {
            //プロセススタートInfoのインスタンスを作成＆各プロパティを設定
            ProcessStartInfo processStartInfos = new ProcessStartInfo("cmd.exe", "/c " + Argument)
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
                    UpdateRichTextBox(e.Data, true);
                };
                processes.ErrorDataReceived += (sender, e) =>
                {
                    //プロセスコマンドのエラーログイベント
                    UpdateRichTextBox(e.Data, true);
                };

                //進行状況の表示
                UpdateRichTextBox("コマンドの実行を開始しました。", true);
                processes.Start();
                processes.BeginOutputReadLine();
                processes.BeginErrorReadLine();

                //プログレスバータイマー開始
                timer1.Enabled = true;
                timer1.Start();

                //プロセスの待機
                bool isCompleted = await Task.Run(() => processes.WaitForExit(this.TimeOut));

                if (processes.HasExited)
                {
                    //プロセスが終了している場合
                    UpdateRichTextBox("プロセスが終了しました。", true);
                    progressBar1.Value = 0;
                    timer1.Stop();
                } else if (!isCompleted)
                {
                    UpdateRichTextBox("プロセスがタイムアウト時間に達しました。", true);
                    progressBar1.Value = 0;
                    timer1.Stop();
                    processes.Kill();
                }
                else
                {
                    //プロセスが終了していない場合
                    UpdateRichTextBox("プロセスがまだ処理中です・・。", true);
                }
            }
            UpdateRichTextBox("ダウンロードプロセスを全て終了させました。", true);
            ProcessKills();
        }
        #endregion

        #region "リッチテキスト関係"
        /// <summary>
        /// スレッドセーフにリッチテキストに表示します
        /// </summary>
        /// <param name="text">表示する文字</param>
        /// <param name="Thread">スレッド時：True</param>
        private void UpdateRichTextBox(string text, bool Thread = false)
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
        #endregion

        #region "コンフィグファイルの読込み＆プログレスバー初期化"
        /// <summary>
        /// Configファイルデータの読込み
        /// </summary>
        private void ReadConfigData()
        {
            //設定ファイルからディレクトリを読込み
            IniData objIni = new IniData();
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
        /// アップデート処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //管理者権限かどうかの取得
            Admin admin = new Admin();
            admin.IsAdministrator();
            bool adm = admin.GetAdmin;
            if (adm==false)
            {
                MessageBox.Show("Phytonのアップデートは管理者権限で行ってください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Phytonをアップデートします。よろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //最新バージョンの更新
                    PhytonUpdate();
                }
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
    }
    #endregion


}
