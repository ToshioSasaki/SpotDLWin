using System;
using System.Windows.Forms;

namespace MusicDLWin
{
    public partial class frmTitle : Form
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmTitle()
        {
            InitializeComponent();
            // タイマーの設定
            Timer timer = new Timer();
            timer.Interval = 2000; // 2秒後にメイン画面に遷移
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
            frmMusicDL mainForm = new frmMusicDL();
            mainForm.Show();

            this.Visible = false;
        }

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTitle_Load(object sender, EventArgs e)
        {
            this.Visible = true;
        }

    }
}
