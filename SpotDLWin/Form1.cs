using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SpotDLWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(textOutDir.Text.Trim()))
            {
                // フォルダが存在しない場合、フォルダを作成
                Directory.CreateDirectory(textOutDir.Text.Trim());
            }
        }

        /// <summary>
        /// ダウンロードクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Download_Click(object sender, EventArgs e)
        {
            string Url = "spotdl " + inputTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(Url))
            {

                ResultText.AppendText(Url);

         
                //MP3ダウンロード実行
                string Result = ExecuteCommand(Url);
                //結果をリッチテキストボックスに表示
                ResultText.AppendText(Result);

                //ファイルコピー
                if (CopyMp3File() == false)
                {
                    ResultText.AppendText("Error :FileCopy");
                } else
                {
                    ResultText.AppendText("Download Ok , FileCopy Ok");
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
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    textOutDir.Text = selectedFolder;
                }
            }
        }

        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="command">spotdl + URL</param>
        /// <returns>コマンドライン</returns>
        private string ExecuteCommand(string command)
        {
            try
            {
                // コマンドを実行して結果を取得
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.WriteLine("exit");
                    return process.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "error: " + ex.Message;
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
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
