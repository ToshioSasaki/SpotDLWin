using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SpotDLWin
{
    public partial class SpotDL : Form
    {
        private ManualResetEvent mre1 = new ManualResetEvent(false);
        private ManualResetEvent mre2 = new ManualResetEvent(false);

        public SpotDL()
        {
            InitializeComponent();
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
            //ディレクトリが存在するかしないか確認
            if (!Directory.Exists(textOutDir.Text.Trim()))
            {
                // フォルダが存在しない場合、フォルダを作成
                Directory.CreateDirectory(textOutDir.Text.Trim());
            }
            ResultText.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            groupCommand.Anchor = AnchorStyles.Top | AnchorStyles.Left;
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
                DeleteMP3File();

                //作業開始
                DateTime now = DateTime.Now;
                string sYmd = now.Year + "/" + now.Month + "/" + now.Day + " ";
                string sHms = now.Hour + ":" + now.Minute + ":" + now.Second;
                UpdateRichTextBox("--All Downloads Work Start --: StartTime(" + sYmd + sHms + ")");

                //最新バージョンの更新
                string command1 = "pip install --upgrade spotdl";
                await ExecuteCommand(command1,Jobdata.NONE);

                //URLダウンロード
                string command2 = "spotdl " + inputTextBox.Text.Trim();
                await ExecuteCommand(command2,Jobdata.NONE);

                //ローカルファイルをコピー
                //string command3 = "cd " + textOutDir.Text.Trim() + " & dir";
                //await ExecuteCommand(command3,Jobdata.COPY);
                CopyMp3File();
                UpdateRichTextBox("Copy OutPut AllFiles Complete");
            }
        }

        /// <summary>
        /// インプット、アウトプット、パスのチェック
        /// </summary>
        /// <returns>True：成功、False：エラー</returns>
        private bool CheckDIr()
        {
            if (Directory.Exists(textOutDir.Text.Trim())==false)
            {
                MessageBox.Show("OutPut Path Error !!");
                return false;
            }
            if (string.IsNullOrEmpty(inputTextBox.Text.Trim()))
            {
                MessageBox.Show("Input URL Empty !!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// ローカルフォルダにあるMP3ファイルを全て削除します。
        /// </summary>
        private void DeleteMP3File()
        {
            string AppPath = Application.StartupPath + "\\";
            string[] mp3Files = Directory.GetFiles(AppPath, "*.mp3");
            // 各MP3ファイルを削除
            foreach (string file in mp3Files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (IOException e)
                {
                    UpdateRichTextBox($"Could not delete file: {file}. Error: {e.Message}");
                }
            }
        }

        /// <summary>
        /// フォルダボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
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
        private async Task ExecuteCommand(string command,Jobdata job)
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
                process.ErrorDataReceived += (sender, e) =>
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
            string destinationFolder = textOutDir.Text.Trim(); // コピー先フォルダ

            try
            {
                if (Directory.Exists(AppPath) && Directory.Exists(destinationFolder))
                {
                    string[] mp3Files = Directory.GetFiles(AppPath, "*.mp3");

                    foreach (string mp3File in mp3Files)
                    {
                        string fileName = Path.GetFileName(mp3File);
                        string destinationPath = Path.Combine(destinationFolder, fileName);

                        if (File.Exists(destinationPath)==true)
                        {
                            File.Delete(destinationPath);
                        }
                        // MP3ファイルをコピー
                        File.Copy(mp3File, destinationPath);

                        // コピー元のMP3ファイルを削除
                        File.Delete(mp3File);
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

        private void Download_Click(object sender, EventArgs e)
        {
            WorksFiles();
        }

        private void ResultText_TextChanged(object sender, EventArgs e)
        {

        }

        private void ResultText_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void groupCommand_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    /// <summary>
    /// 列挙型のEnumデータ
    /// </summary>
    public enum Jobdata
    {
        NONE,
        COPY,
    }
}
