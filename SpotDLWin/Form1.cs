using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
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
                //URLを表示
                UpdateRichTextBox(Url);

                //MP3ダウンロード実行
                UpdateRichTextBox(ExecuteCommand(Url));

                

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
        private string ExecuteCommand(string command)
        {
            try
            {
                //プロセスの準備とコマンドの実行
                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                Process process = new Process
                {
                    StartInfo = startInfo,
                    EnableRaisingEvents = true
                };
               
                //コマンドの出力(UIスレッドで行う)
                process.OutputDataReceived += (s, _e) =>
                {
                   if (!String.IsNullOrEmpty(_e.Data))
                    {
                        UpdateRichTextBox(_e.Data);
                    }
                    //ファイルコピー
                    if (CopyMp3File() == false)
                    {
                        UpdateRichTextBox("Error :FileCopy");
                    }
                    else
                    {
                        UpdateRichTextBox("Download Ok , FileCopy Ok");
                    }
                };
                process.Start();
                process.BeginOutputReadLine();

                return "";
                
            }
            catch (Exception ex)
            {
                return "Error : Command Error";
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
                        Application.DoEvents();
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

        /// <summary>
        /// リッチテキストに表示します。
        /// </summary>
        /// <param name="text">表示する文字</param>
        private void UpdateRichTextBox(string text)
        {
            if (ResultText.InvokeRequired)
            {
                ResultText.Invoke(new Action<string>(UpdateRichTextBox), new object[] { text });
            }
            else
            {
                ResultText.AppendText(text + Environment.NewLine);
                ResultText.ScrollToCaret();
            }
        }


    }
}
