using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicDLWin
{
    public class clsSpotDl : System.IDisposable
    {
        /// <summary>
        /// SpotDLバージョンアップコンストラクタ
        /// </summary>
        /// <param name="richTextBox"></param>
        public clsSpotDl(RichTextBox richTextBox) {
            messageDisplayer = new MessageDisplayer(richTextBox);
        }

        #region "メッセージ表示クラス"
        private MessageDisplayer messageDisplayer;
        #endregion

        #region "spotDLのインストール"
        /// <summary>
        /// SpotDLのインストール
        /// </summary>
        /// <returns></returns>
        public async Task InstallSpotDL()
        {
            clsIniData iniData = new clsIniData();
            iniData.GetIniData();
            string PythonPath = iniData.getPythonPath;
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = $"{PythonPath}\\python.exe",
                Arguments = "-m pip install --user spotdl",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.OutputDataReceived += (sender, args) => messageDisplayer.UpdateRichTextBox(args.Data, true);
                process.ErrorDataReceived += (sender, args) => messageDisplayer.UpdateRichTextBox(args.Data, true);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                messageDisplayer.UpdateRichTextBox("spotDLのインストールを開始します。", true);
                await Task.Run(() => process.WaitForExit());
                messageDisplayer.UpdateRichTextBox("spotDLのインストールが完了しました。", true);
            }
        }

        private void SetEnvironment()
        {
            // 環境変数を更新 (ffmpegのインストールパスを追加)
            //var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            //Environment.SetEnvironmentVariable("Path", path + ";" + ffmpegInstallPath + @"\" + ffmpegFileName + @"\bin\", EnvironmentVariableTarget.Machine);
            //messageDisplayer.UpdateRichTextBox("■FFmpeg環境パス設定終了・FFmpegの全ての作業が終了しました。■");
        }
        #endregion

        /// <summary>
        /// クラス内容をメモリから解放
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
        }

    }
}
