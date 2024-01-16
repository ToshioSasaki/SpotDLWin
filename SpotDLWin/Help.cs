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
            Result += "■GitHub上のSpotDLのWindowsGUI版です。■\n";
            Result += "・使い方・セットアップ等の詳細はSpotDL欄を参照してください。\n";
            Result += "※使用には予めPython・FFmpegのセットアップが必須になります。\n";
            Result += "\n";
            Result += "■セットアップ手順以下参照■\n";
            Result += "・https://github.com/spotDL/spotify-downloadern\n";
            Result += "・https://self-development.info/%E3%80%90%E7%84%A1%E6%96%99%E3%83%BB%E5%AE%89%E5%85%A8%E3%80%91spotify%E3%81%AE%E9%9F%B3%E6%A5%BD%E3%82%92%E3%83%80%E3%82%A6%E3%83%B3%E3%83%AD%E3%83%BC%E3%83%89%E3%81%99%E3%82%8B%E6%96%B9%E6%B3%95/\n";
            Result += "※著作権等もありますので使用はご自身の目的と判断とご自身の範囲内だけでお願い致します。\n";
            Result += "※ダウンロードファイルを販売等の目的で使わないようにお願いします。\n";
            Result += "\n";
            Result += "■このプログラムについて・規約等について■\n";
            Result += "・ダウンロード曲数が多すぎると再リトライが何度もかかり始めダウンロードが開始されなくなります。\n";
            Result += "・設定より再リトライ回数が設定できますのでダウンロードに時間がかかる場合は値を小さくしてください。\n";
            Result += "※このプログラム及びSpotDLを使ったトラブル等は一切法的な措置等は関与致しません。\n";
            Result += "・本GitHub上のプログラムからブランチを切っての改造は許可いたしますがコピーでの改編や改造は許可いたしません。\n";
            Result += "\n";
            Result += "　Copyright © 2024-1. All rights reserved. Toshiharu Sasaki \n";
            Result += "  How to Reference SpotDL https://github.com/spotDL/spotify-downloadern \n";
            return Result;
        }

        private void 戻るToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
