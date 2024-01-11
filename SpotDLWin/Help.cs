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
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            HelpRichText.Text = HelpMessage();
        }

        private string HelpMessage()
        {
            string Result = "";
            Result += "GitHub上のSpotDLのWindows版です。\n";
            Result += "使い方・セットアップ等の詳細はSpotDL欄を参照してください。\n";
            Result += "(使用には予めPython・FFmpegのセットアップが必須になります。)\n";
            Result += "詳細URL：https://github.com/spotDL/spotify-downloadern\\";
            Result += "使用はご自身の判断とご自身の範囲内だけでお願い致します。\n";
            Result += "ダウンロードファイルを販売等の目的や・商の目的で使わないようにお願いします。\n";
            return Result;
        }

        private void 戻るToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
