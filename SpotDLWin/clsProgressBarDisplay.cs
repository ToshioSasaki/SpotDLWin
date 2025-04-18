using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Forms.Application;

namespace MusicDLWin
{
    public class clsProgressBarDisplay : System.IDisposable
    {
        private ProgressBar progressBar { get; set; } = new ProgressBar();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="richTextBox">フォーム画面のRichTextBox</param>
        public clsProgressBarDisplay(ProgressBar progress)
        {
            progressBar = progress;
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Step = 0;
            progressBar.Value = 30;
            progressBar.Visible = true;
        }

        /// <summary>
        /// ProgressBarの進捗を表示します。
        /// </summary>
        /// <param name="value">進捗値</param>
        public void UpdateProgress(int value, bool Thread)
        {
            Application.DoEvents();
            if (value != 0)
            {
                if (Thread)
                {
                    try
                    {
                        // スレッドセーフな方法でProgressBarを更新
                        progressBar.Invoke(new Action(() =>
                        {
                            progressBar.Value = progressBar.Value + value;
                        }));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                }
                else
                {
                    //通常のシングルスレッドの場合
                    UpdateProgress(value);
                }
            }
        }

        /// <summary>
        /// シングルスレッドバージョン
        /// </summary>
        /// <param name="value">進捗値</param>
        public void UpdateProgress(int value)
        {
            Application.DoEvents();
            //通常のシングルスレッドの場合
            progressBar.Value = progressBar.Value + value;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
