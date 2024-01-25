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
        private const string ffmpegUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";

        /// <summary>
        /// ffmpegのダウンロード先
        /// </summary>
        private const string ffmepgDownloadPath = "c:\\temp\\extract\\ffmpeg.zip";

        /// <summary>
        /// ffmpegの解凍するパス
        /// </summary>
        private const string ffmpegZipPath = "c:\\temp\\extract\\";
        
        /// <summary>
        /// ffmpegのインストール先
        /// </summary>
        private const string ffmpegInstallPath = "C:\\Program Files\\";

        /// <summary>
        /// ffmpegのインストール先ファイル名
        /// </summary>
        private const string ffmpegFileName = "ffmpeg";

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
            this.DeleteFFmpegFolder();

            //ダウンロードしたffmpegファイルの解凍
            await this.ExtractFfmpeg();

            //解凍したファイルをインストールフォルダ先へコピー
            this.MoveFFmpegExecutable();

            //環境設定パスの設定
            this.SetEnvironment();
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
                webClient.DownloadFileAsync(new Uri(ffmpegUrl), ffmepgDownloadPath);
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
            if (Directory.Exists(ffmpegInstallPath + ffmpegFileName))
            {
                try
                {
                    // フォルダを削除:true=サブディレクトリも削除する。
                    messageDisplayer.UpdateRichTextBox(ffmpegInstallPath + ffmpegFileName + "フォルダを削除します。");
                    clsFileFolder clsFile = new clsFileFolder();
                    clsFile.DeleteFolderContents(ffmpegInstallPath + ffmpegFileName);
                    messageDisplayer.UpdateRichTextBox("ffmpegが即に存在するので削除しました。");
                }
                catch (Exception e)
                {
                    messageDisplayer.UpdateRichTextBox("エラーが発生しました: " + e.Message);
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
        private async Task ExtractFfmpeg()
        {
            Application.DoEvents();
            if (!Directory.Exists(ffmpegZipPath))
            {
                messageDisplayer.UpdateRichTextBox(ffmpegZipPath + "にffmpegフォルダを作成しました。",true);
                Directory.CreateDirectory(ffmpegZipPath);
            }
            messageDisplayer.UpdateRichTextBox("■FFmpeg解凍処理中・・■",true);
            messageDisplayer.UpdateRichTextBox(ffmepgDownloadPath + "を" + ffmpegZipPath + "ヘ解凍します。", true);
            try
            {
                //解凍フォルダが即に存在しているか確認
                clsFileFolder file = new clsFileFolder();
                string extractFullPath = file.GetFindDirectory(ffmpegZipPath, "ffmpeg");
                if (extractFullPath != null)
                {
                    messageDisplayer.UpdateRichTextBox(extractFullPath + "フォルダを削除します。。", true);
                    //解凍フォルダが既に存在していれば削除
                    Directory.Delete(extractFullPath);
                }
                //解凍処理
                ZipFile.ExtractToDirectory(ffmepgDownloadPath, ffmpegZipPath);
                messageDisplayer.UpdateRichTextBox(ffmepgDownloadPath + "を" + ffmpegZipPath + "ヘ解凍しました。", true);
            }
            catch (Exception e)
            {
                messageDisplayer.UpdateRichTextBox("解凍中にエラーが発生しました。\n" + e.Message.ToString(), true);
            }finally
            {
                messageDisplayer.UpdateRichTextBox("■FFmpeg解凍処理終了■", true);
            }
        }

        /// <summary>
        /// ffmpegを移動
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        private void MoveFFmpegExecutable()
        {
            //インストール先パス
            var destinationFile = ffmpegInstallPath + ffmpegFileName;
            //解凍先パスのフルパスを取得
            clsFileFolder file = new clsFileFolder();
            string extractFullPath = file.GetFindDirectory(ffmpegZipPath, "ffmpeg");
            //ディレクトリの移動
            file.MoveFolder(extractFullPath, destinationFile);
            messageDisplayer.UpdateRichTextBox("■FFmpeg：" + extractFullPath + "→" + destinationFile + "へコピー処理終了■");
        }

        /// <summary>
        /// 環境パスの設定
        /// </summary>
        private void SetEnvironment()
        {
            // 環境変数を更新 (Pythonのインストールパスを追加)
            clsFileFolder file = new clsFileFolder();
            file.SetEnvironment(ffmpegInstallPath + @"\" + ffmpegFileName + @"\bin");
            messageDisplayer.UpdateRichTextBox("■FFmpeg環境パス設定終了・FFmpegの全ての作業が終了しました。■");
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
