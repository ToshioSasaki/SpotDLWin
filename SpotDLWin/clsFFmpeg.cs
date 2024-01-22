using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class clsFFmpeg : System.IDisposable
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="richTextBox">frmMusicDLのRickTextBox</param>
        public clsFFmpeg(RichTextBox richTextBox){

            messageDisplayer = new MessageDisplayer(richTextBox);

        }

        /// <summary>
        /// ffmpegのリンク先
        /// </summary>
        private readonly string ffmpegUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";

        /// <summary>
        /// ffmpegのダウンロード先
        /// </summary>
        private readonly string downloadPath = "c:\\temp\\extract\\ffmpeg.zip";

        /// <summary>
        /// ffmpegの解凍するパス
        /// </summary>
        private readonly string ffmpegZipPath = "c:\\temp\\extract\\";
        
        /// <summary>
        /// ffmpegのインストール先
        /// </summary>
        private readonly string ffmpegInstallPath = "C:\\Program Files\\";

        /// <summary>
        /// frmMusicDLのRickTextBox
        /// </summary>
        public RichTextBox UpdateRichTextBox { get; set; }

        #region "メッセージ表示クラス"
        private MessageDisplayer messageDisplayer;
        #endregion

        #region "ffmpegのインストール"
        /// <summary>
        /// ffmpegのダウンロードとインストール
        /// </summary>
        public async Task DownloadAndInstallFFmpeg()
        {
            //ダウンロードffmpeg
            await DownloadFile();
        }

        /// <summary>
        /// FFmpegのインストール
        /// </summary>
        private async Task InstallFFmpeg()
        {
            //既存ffmpegフォルダの削除
            DeleteFFmpegFolder();

            //ダウンロードしたffmpegファイルの解凍
            await ExtractFfmpeg(downloadPath, ffmpegZipPath);

            //解凍したファイルをインストールフォルダ先へコピー
            MoveFFmpegExecutable(ffmpegZipPath, ffmpegInstallPath);
        }

        /// <summary>
        /// ffmpegのダウンロード
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="outputPath">ダウンロード先</param>
        /// <returns></returns>
        private async Task DownloadFile()
        {
            //ディレクトリが存在するかしないか確認
            if (!Directory.Exists(ffmpegZipPath))
            {
                // フォルダが存在しない場合、フォルダを作成
                Directory.CreateDirectory(ffmpegZipPath);
            }
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.DownloadFileCompletedCallback);
                webClient.DownloadFileAsync(new Uri(ffmpegUrl), downloadPath);
            }
        }

        /// <summary>
        /// pythonダウンロードコールバック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DownloadFileCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                messageDisplayer.UpdateRichTextBox("FFmpegのダウンロード完了しました。", true);
                messageDisplayer.UpdateRichTextBox("FFmpegをインストール中・・", true);
                this.InstallFFmpeg(); // ダウンロードが完了したらインストールを開始
            }
            else
            {
                // エラーハンドリング
                messageDisplayer.UpdateRichTextBox("ダウンロード中にエラーが発生しました: " + e.Error.Message, true);
            }
        }

        /// <summary>
        /// ffmpegフォルダが即に存在すれば削除します。
        /// </summary>
        /// <returns>メッセージ</returns>
        private string DeleteFFmpegFolder()
        {
            string message = "";
            // フォルダが存在するかチェック
            if (Directory.Exists(ffmpegInstallPath))
            {
                try
                {
                    // フォルダを削除:true=サブディレクトリも削除する。
                    messageDisplayer.UpdateRichTextBox("ffmpegフォルダを削除します。");
                    clsFileFolder clsFile = new clsFileFolder();
                    clsFile.DeleteFolderContents(ffmpegInstallPath);
                    messageDisplayer.UpdateRichTextBox("ffmpegが即に存在するので削除しました。");
                }
                catch (IOException e)
                {
                    messageDisplayer.UpdateRichTextBox("エラーが発生しました: {e.Message}");
                }
            }
            else
            {
                messageDisplayer.UpdateRichTextBox("ffmpegは存在しませんでした。");
            }
            return message;
        }

        /// <summary>
        /// ffmpegの解凍処理
        /// </summary>
        /// <param name="zipPath">Zipファイルの場所</param>
        /// <param name="extractPath">解凍後のパス</param>
        private async Task ExtractFfmpeg(string zipPath, string extractPath)
        {
            Application.DoEvents();
            if (!Directory.Exists(extractPath))
            {
                messageDisplayer.UpdateRichTextBox(extractPath + "にffmpegフォルダを作成しました。");
                Directory.CreateDirectory(extractPath);
            }
            messageDisplayer.UpdateRichTextBox("■FFmpeg解凍処理中・・■");
            messageDisplayer.UpdateRichTextBox(zipPath + "を" + extractPath + "ヘ解凍します。");
            try
            {
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                messageDisplayer.UpdateRichTextBox(zipPath + "を" + extractPath + "ヘ解凍しました。");
            }
            catch (Exception e)
            {
                messageDisplayer.UpdateRichTextBox("解凍中にエラーが発生しました。\n" + e.Message.ToString());
            }finally
            {
                messageDisplayer.UpdateRichTextBox("■FFmpeg解凍処理終了■");
            }
        }

        /// <summary>
        /// ffmpegを移動
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        private void MoveFFmpegExecutable(string sourcePath, string destinationPath)
        {
            var sourceFile = Path.Combine(sourcePath, "ffmpeg.exe");
            var destinationFile = Path.Combine(destinationPath, "ffmpeg.exe");
            clsFileFolder clsFile = new clsFileFolder();
            clsFile.MoveFolder(sourceFile, destinationFile);
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
