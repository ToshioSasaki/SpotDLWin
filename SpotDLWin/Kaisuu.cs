using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicDLWin
{
    public partial class Kaisuu : Form
    {
        public Kaisuu()
        {
            InitializeComponent();
        }

        public int KaisuuValue { get; set; }
        public int TimeOutValue { get; set; }



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            IniData iniData = new IniData();
            iniData.SetKaisuu(GetKaisuu(textBox1.Text));
            //分をミリ秒に変換
            double dTimeOut = GetTimeOut(textTimeOut.Text) * 60000;
            iniData.SetTimeOut(int.Parse(dTimeOut.ToString("0")));

            MessageBox.Show("保存しました。", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kaisuu_Load(object sender, EventArgs e)
        {
            textBox1.Text = this.KaisuuValue.ToString();
            textTimeOut.Text = this.TimeOutValue.ToString();
        }

        /// <summary>
        /// フォームクローズイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kaisuu_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        /// <summary>
        /// フォームクローズ処理
        /// </summary>
        private void CloseForm()
        {
            this.DialogResult = DialogResult.OK;
            this.KaisuuValue = int.Parse(textBox1.Text);
            this.TimeOutValue = GetTimeOut(textTimeOut.Text) * 60000;
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// 試行回数をかえします。
        /// </summary>
        /// <param name="kaisuu"></param>
        /// <returns>試行回数</returns>
        private int GetKaisuu(string kaisuu) {

            kaisuu = kaisuu.Trim();
            if (string.IsNullOrEmpty(kaisuu))
            {
                textBox1.Text = "0";
                return 0;
            } else
            {
                textBox1.Text = kaisuu;
                int iKaisu = int.Parse(kaisuu);
                if (iKaisu<0 || iKaisu>5)
                {
                    MessageBox.Show("試行回数は0以上6以下を設定してください", "試行回数", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "1";
                    Application.DoEvents();
                    return 1;
                }
                return int.Parse(kaisuu);
            }
        
        }

        /// <summary>
        /// タイムアウトをかえします。
        /// </summary>
        /// <param name="kaisuu"></param>
        /// <returns>タイムアウト</returns>
        private int GetTimeOut(string timeOut)
        {

            timeOut = timeOut.Trim();
            if (string.IsNullOrEmpty(timeOut))
            {
                int iTimeOut = int.Parse(timeOut);
                iTimeOut = 1;
                textTimeOut.Text = iTimeOut.ToString();
                return iTimeOut;
            }
            else
            {
                textTimeOut.Text = timeOut;
                int iTimeOut = int.Parse(timeOut);
                if (iTimeOut < 1 || iTimeOut > 30)
                {
                    MessageBox.Show("タイムアウトは0分以上31分以下を設定してください", "タイムアウト", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textTimeOut.Text = "1";
                    Application.DoEvents();
                    return 1;
                }
                return int.Parse(timeOut);
            }

        }


    }
}
