﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicDLWin
{
    public  class clsPython : System.IDisposable
    {
        /// <summary>
        /// Pythonインストールコンストラクタ
        /// </summary>
        /// <param name="richTextBox"></param>
        public clsPython(RichTextBox richTextBox) {

            messageDisplayer = new MessageDisplayer(richTextBox);

        }

        #region "メッセージ表示クラス"
        private MessageDisplayer messageDisplayer;
        #endregion

        /// <summary>
        /// Pythonのダウンロード先
        /// </summary>
        private readonly string PythonDownloadPath = "C:\\temp\\extract\\";

        /// <summary>
        /// Pythonのファイル名
        /// </summary>
        private readonly string PythonFileName = "python-3.9.0.exe";

        /// <summary>
        /// PythonのURL先
        /// </summary>
        private readonly string PythonInstallerUrl = "https://www.python.org/ftp/python/3.9.0/python-3.9.0.exe";


        #region"pythonのダウンロードとインストール"

        /// <summary>
        /// Pythonのダウンロード
        /// </summary>
        public async Task DownloadPythonInstaller()
        {
            clsFileFolder file = new clsFileFolder();
            file.CreateDirectory(PythonDownloadPath);
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompletedCallback);
                webClient.DownloadFileAsync(new Uri(PythonInstallerUrl), PythonDownloadPath + PythonFileName);
            }
        }

        /// <summary>
        /// pythonインストール処理
        /// </summary>
        private async Task InstallPython()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = PythonDownloadPath + PythonFileName,
                Arguments = "/quiet InstallAllUsers=1 PrependPath=1", // サイレントインストールのためのオプション
                UseShellExecute = false
            };

            using (Process proc = Process.Start(startInfo))
            {
                // インストーラーの完了を待機
                messageDisplayer.UpdateRichTextBox("Pythonのインストール処理中・・", true);
                proc.OutputDataReceived += (sender, args) => messageDisplayer.UpdateRichTextBox(args.Data, true);
                proc.ErrorDataReceived += (sender, args) => messageDisplayer.UpdateRichTextBox(args.Data, true);
                await Task.Run(() => proc.WaitForExit());
                messageDisplayer.UpdateRichTextBox("Pythonのインストールが完了しました。", true);
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
                messageDisplayer.UpdateRichTextBox("Pythonのダウンロード完了しました。", true);
                await this.InstallPython(); // ダウンロードが完了したらインストールを開始
            }
            else
            {
                // エラーハンドリング
                messageDisplayer.UpdateRichTextBox("ダウンロード中にエラーが発生しました: " + e.Error.Message, true);
            }
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