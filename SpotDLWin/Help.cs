using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            Result += "※このプログラム及びSpotDLを使ったトラブル等は一切法的な措置等は関与致しません。\n";
            Result += "・本GitHub上のプログラムからブランチを切っての改造は許可いたしますがコピーでの改編や改造は許可いたしません。\n";
            Result += "\n";
            Result += "■使い方など■\n";
            Result += "・プレイリストを入力するとMP3のアルバム名称が自動で入力されます。\n";
            Result += "・MP3のプロパティのトラックNoがダウンロード順に自動で設定されます。\n";
            Result += "・上記2点の内容によりitunesにドラッグすると１つのアルバムとして管理しやすくなると思います。\n";
            Result += "・ダウンロード曲数が多すぎると再リトライが何度もかかり始めダウンロードが開始されなくなります。\n";
            Result += "・設定より再リトライ回数が設定できますのでダウンロードに時間がかかる場合は値を小さくしてください。1が最小です。\n";
            Result += "\n";
            Result += "･タイムアウト回数について。タイムアウトを1分～10分設定できます。\n";
            Result += "･所定の時間にダウンロードできないものはダウンロードを強制的に終了します。\n";
            Result += "\n";
            Result += "■アップデートについて■\n";
            Result += "・随時ファイルメニューより行ってください。\n";
            Result += "※アップデート時には本アプリケーションを管理者権限で立ち上げる必要があります。\n";
            Result += "\n";
            Result += "■Phytonのアップデートについて■\n";
            Result += "・Chocolateyを使用します。\n";
            Result += "・ChocolateyをWindows 11にインストールするには、以下の手順を実行します：\n";
            Result += "\n";
            Result += "・PowerShellを管理者権限で開く:\n";
            Result += "\n";
            Result += "・Windows + R キーを押して「Run」を開き、「PowerShell」と入力して Ctrl+Shift + Enter を押して管理者権限でPowerShellを開きます。\n";
            Result += "・または、スタートメニューを右クリックして「Terminal(Admin)」オプションを選択します。\n";
            Result += "・実行ポリシーの確認:\n";
            Result += "\n";
            Result += "・PowerShellで Get-ExecutionPolicy コマンドを実行して実行ポリシーの状態を確認します。\n";
            Result += "・もし「Restricted」と表示された場合は、Set - ExecutionPolicy AllSigned または Set - ExecutionPolicy Bypass - Scope Process コマンドを実行してポリシーを変更します。\n";
            Result += "・Chocolateyのインストールコマンドの実行:\n";
            Result += "\n";
            Result += "・以下のコマンドをPowerShellにコピー＆ペーストして実行します：\n";
            Result += "less\n";
            Result += "Copy code\n";
            Result += "Set - ExecutionPolicy Bypass - Scope Process - Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol - bor 3072; iex((New - Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))\n";
            Result += "・このコマンドは、Chocolateyのインストールスクリプトをダウンロードして実行します。インストールが完了すると、Chocolateyが使用可能になります。\n";
            Result += "・Chocolateyの使用:\n";
            Result += "\n";
            Result += "・インストールが完了したら、choco または choco -? コマンドを入力して、Chocolateyの使用を開始できます。\n";
            Result += "\n";
            Result += "■その他■\n";
            Result += "　Copyright © 2024-1. All rights reserved. Toshiharu Sasaki \n";
            Result += "  How to Reference SpotDL https://github.com/spotDL/spotify-downloadern \n";
            return Result;
        }

        private void 戻るToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void HelpRichText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
