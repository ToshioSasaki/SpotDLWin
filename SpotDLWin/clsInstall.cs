using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicDLWin
{
    public class clsInstall : System.IDisposable
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="richTextBox">frmMusicDLのRickTextBox</param>
        public clsInstall(RichTextBox richTextBox,ProgressBar progressBar){

            messageDisplayer = new MessageDisplayer(richTextBox);
            progressDisplayer = new clsProgressBarDisplay(progressBar);
        }

        /// <summary>
        /// プロパティ
        /// </summary>
        public RichTextBox UpdateRichTextBox { get; set; }
        public ProgressBar ProgressBar { get; set; }

        #region "メッセージ表示クラス"
        private MessageDisplayer messageDisplayer;
        #endregion

        #region "プログレスバー表示クラス"
        private clsProgressBarDisplay progressDisplayer;
        #endregion

        #region "ffmpegのインストール"
        /// <summary>
        /// FFmpegのインストール
        /// </summary>
        /// <returns></returns>
        public bool InstallFFmpeg()
        {
            string downloadUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";
            string downloadPath = Path.GetTempPath();
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }
            downloadPath = Path.Combine(Path.GetTempPath(), "ffmpeg.zip");
            string extractPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ffmpeg");
            string binPath = Path.Combine(extractPath, "ffmpeg-6.0-essentials_build", "bin\\");
            try
            {
                messageDisplayer.UpdateRichTextBox("FFmpegの最新版をダウンロードしています...");
                progressDisplayer.UpdateProgress(10, true);
                using (var client = new WebClient())
                {
                    client.DownloadFile(downloadUrl, downloadPath);
                }

                messageDisplayer.UpdateRichTextBox("ダウンロード完了。展開中...");
                progressDisplayer.UpdateProgress(10, true);
                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }
                ZipFile.ExtractToDirectory(downloadPath, extractPath);
                progressDisplayer.UpdateProgress(10, true);
                messageDisplayer.UpdateRichTextBox("展開完了。");

                UpdateEnvironmentPath(binPath);
                return true;
            }
            catch (Exception ex)
            {
                messageDisplayer.UpdateRichTextBox("FFmpegインストール中にエラー: " + ex.Message);
                return false;
            }
            
        }
        #endregion

        #region "Pythonのダウンロードとインストール"
        /// <summary>
        /// Pythonのダウンロードとインストール
        /// </summary>
        /// <returns></returns>
        public bool InstallPython()
        {
            Application.DoEvents();
            string downloadUrl = "https://www.python.org/ftp/python/3.11.6/python-3.11.6-amd64.exe";
            string downloadPath = Path.GetTempPath();
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }
            downloadPath = Path.Combine(downloadPath, "python-installer.exe");
            string installPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Python311");
            string binPath = Path.Combine(installPath, "Scripts\\");

            try
            {
                messageDisplayer.UpdateRichTextBox("Pythonの最新版をダウンロードしています...");
                progressDisplayer.UpdateProgress(10, true);
                using (var client = new WebClient())
                {
                    client.DownloadFile(downloadUrl, downloadPath);
                }

                messageDisplayer.UpdateRichTextBox("インストール開始...");
                progressDisplayer.UpdateProgress(10, true);
                Process process = new Process();
                process.StartInfo.FileName = downloadPath;
                process.StartInfo.Arguments = $"/quiet InstallAllUsers=1 PrependPath=1 TargetDir={installPath}";
                process.StartInfo.UseShellExecute = true;
                process.Start();
                process.WaitForExit();

                messageDisplayer.UpdateRichTextBox("Pythonインストール完了。");
                progressDisplayer.UpdateProgress(10, true);
                UpdateEnvironmentPath(binPath);
                return true;
            }
            catch (Exception ex)
            {
                messageDisplayer.UpdateRichTextBox("Pythonインストール中にエラー: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region "環境変数の更新" 
        /// <summary>
        /// 環境変数 Path を更新
        /// </summary>
        /// <param name="newPath">パス</param>
        public void UpdateEnvironmentPath(string newPath)
        {
            Application.DoEvents();
            messageDisplayer.UpdateRichTextBox("環境変数 Path を更新中...");
            progressDisplayer.UpdateProgress(10, true);
            string currentPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            if (!currentPath.Contains(newPath))
            {
                string updatedPath = currentPath + ";" + newPath;
                Environment.SetEnvironmentVariable("Path", updatedPath, EnvironmentVariableTarget.Machine);
                messageDisplayer.UpdateRichTextBox("Path に追加しました: " + newPath);
                progressDisplayer.UpdateProgress(10, true);
            }
            else
            {
                messageDisplayer.UpdateRichTextBox("Path に既に追加されています: " + newPath);
                progressDisplayer.UpdateProgress(10, true);
            }
        }
        #endregion

        #region "終了処理"
        /// <summary>
        /// インストール処理終了
        /// </summary>
        /// <param name="errFlg">True:正常終了､False:異常終了</param>
        public void InstallEnd(bool errFlg) {
            if (errFlg)
            {
                messageDisplayer.UpdateRichTextBox("インストール処理を終了しました。");
                progressDisplayer.UpdateProgress(20, true);
                return;
            } else
            {
                messageDisplayer.UpdateRichTextBox("インストール処理に失敗しました。");
                return;
            }
        }
        /// <summary>
        /// クラス内容をメモリから解放
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
        }

        #endregion
    }
}
