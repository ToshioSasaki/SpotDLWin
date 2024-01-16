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

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            IniData iniData = new IniData();
            iniData.SetKaisuu(GetKaisuu(textBox1.Text));
            MessageBox.Show("保存しました。", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.KaisuuValue = int.Parse(textBox1.Text);
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kaisuu_Load(object sender, EventArgs e)
        {
            textBox1.Text = this.KaisuuValue.ToString();
        }

        /// <summary>
        /// 設定回数をかえします。
        /// </summary>
        /// <param name="kaisuu"></param>
        /// <returns>数値型</returns>
        private int GetKaisuu(string kaisuu) {

            kaisuu = kaisuu.Trim();
            if (string.IsNullOrEmpty(kaisuu))
            {
                textBox1.Text = "1";
                return 1;
            } else
            {
                textBox1.Text = kaisuu;
                int iKaisu = int.Parse(kaisuu);
                if (iKaisu<1 && iKaisu>5)
                {
                    MessageBox.Show("試行回数は0以上6以下を設定してください", "試行回数", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "1";
                    return 1;
                }
                return int.Parse(kaisuu);
            }
        
        }

 
    }
}
