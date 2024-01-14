using System;
using System.Windows.Forms;

namespace MusicDLWin
{
    public partial class title : Form
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public title()
        {
            InitializeComponent();
            // タイマーの設定
            Timer timer = new Timer();
            timer.Interval = 1500; // 1.5秒後にメイン画面に遷移
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        /// <summary>
        /// タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // タイマーを停止
            ((Timer)sender).Stop();

            // メイン画面を表示
            MusicDL mainForm = new MusicDL();
            mainForm.Show();
        }
    }
}
