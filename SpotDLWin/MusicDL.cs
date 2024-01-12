using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;


namespace MusicDLWin
{
    public partial class MusicDL : Form
    {

        public MusicDL()
        {
            InitializeComponent();
        }


        
        /// <summary>
        /// 作業開始
        /// </summary>
        private async void WorksFiles()
        {
            //ディレクトリチェック
            if (CheckDIr())
            {
                //ローカルファイルのMP3ファイルをすべて消去する
                DeleteMP3LocalFile();

                //作業開始
                DateTime now = DateTime.Now;
                string sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                string sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                UpdateRichTextBox("■ダウンロード作業を開始します。(" + sYmd + sHms + ")■");

                //最新バージョンの更新
                string command1 = "pip install --upgrade spotdl";
                await ExecuteCommand(command1);

                //URLダウンロード
                string command2 = "spotdl " + inputTextBox.Text.Trim();
                await ExecuteCommand(command2);

                //ローカルファイルをコピー
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
        /// コマンドの実行
        /// </summary>
        /// <param name="command">spotdl + URL</param>
        /// <param name="upgrade">upgrade時:true</param>
        /// <param name="sYmd">開始日付</param>
        /// <param name="sHms">開始時間</param>
        /// <param name="boolFiles">ファイルコピー：true=ファイルコピーする、false:何もしない</param>
        /// <return>コマンド実行メッセージ</return>>
        private async Task ExecuteCommand(string command)
        {
            
             ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
               
                    process.StartInfo = processStartInfo;
                    process.OutputDataReceived += (sender, e) => 
                    { 
                        UpdateRichTextBox(e.Data);
                    };
                    process.ErrorDataReceived +=(sender, e) =>
                    {
                        UpdateRichTextBox(e.Data);
                    };

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await Task.Run(() => process.WaitForExit());
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
        /// スレッドセーフにリッチテキストに表示します。
        /// </summary>
        /// <param name="text">表示する文字</param>
        private void UpdateRichTextBox(string text)
        {
            if (text != null)
            {
                // スレッドセーフな方法でRichTextBoxを更新
                Invoke((MethodInvoker)(() =>
                {
                    ResultText.AppendText(text + "\n");
                    ResultText.ScrollToCaret();
                }));
            }
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
            textOutDir.Text = objIni.OutPath;

            //OutPutフォルダの削除
            this.CheckOutPutDeleteFiles();

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
        /// ダウンロードボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Download_Click_1(object sender, EventArgs e)
        {
            WorksFiles();
        }

        /// <summary>
        /// 出力先ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            MoveMakeFolder();
        }
    }

}
