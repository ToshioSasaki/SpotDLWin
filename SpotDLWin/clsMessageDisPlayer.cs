using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Forms.Application;

namespace MusicDLWin
{
    public class MessageDisplayer
    {
        private RichTextBox RichTextBox { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="richTextBox">フォーム画面のRichTextBox</param>
        public MessageDisplayer(RichTextBox richTextBox)
        {
            RichTextBox = richTextBox;
        }

        /// <summary>
        /// RichTextBoxにメッセージを表示します。
        /// </summary>
        /// <param name="message">表示するメッセージ</param>
        /// <param name="Thread">スレッド形式のものならTrue</param>
        public void UpdateRichTextBox(string Message,bool Thread)
        {
            if (Message != null)
            {
                
                if (Thread)
                {
                    try
                    {
                        // スレッドセーフな方法でRichTextBoxを更新
                        RichTextBox.Invoke(new Action(() =>
                        {
                            RichTextBox.AppendText(Message + "\n");
                            RichTextBox.ScrollToCaret();
                        }));
                    }
                    catch (Exception ex)
                    {
                        UpdateRichTextBox(ex.Message.ToString());
                    }
                }
                else
                {
                    //通常のシングルスレッドの場合
                    UpdateRichTextBox(Message);
                }
            }
        }

        /// <summary>
        /// リッチテキストボックスのシングルスレッドバージョン
        /// </summary>
        /// <param name="Message">表示する文字列</param>
        public void UpdateRichTextBox(string Message)
        {
            
            if (RichTextBox.InvokeRequired)
            {
                RichTextBox.Invoke(new Action<string>(UpdateRichTextBox), Message);
            }
            else
            {
                Message = GetNowDate() + Message;
                RichTextBox.AppendText(Message + "\n");
                RichTextBox.ScrollToCaret();
            }
        }

        /// <summary>
        /// 現在の日付と時刻を取得します。
        /// </summary>
        /// <returns></returns>
        private string GetNowDate()
        {
            string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            return now + "　";
        }
    }
}
