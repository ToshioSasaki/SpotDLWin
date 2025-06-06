using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MusicDLWin
{
    public partial class frmPathSetting : Form
    {
        public string SelectedPath { get; private set; } = "";
        private string title { get; set; } = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Paths">パス(List)</param>
        /// <param name="title">Python、FFmpeg</param>
        public frmPathSetting(List<string> Paths,string title)
        {
            InitializeComponent();

            // 左の空白列を削除（非表示）
            dataGridView1.RowHeadersVisible = false;

            // 列幅を自動調整
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 行全体選択
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            // パスのデータをバインド
            dataGridView1.DataSource = Paths.Select(x => new { パス = x }).ToList();

            // フォームの×ボタン押下時のイベント登録
            this.FormClosing += FrmPathSetting_FormClosing;

            this.title = title;
        }

        /// <summary>
        /// 「選択」ボタン押下時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SetSelectedPathAndClose();
        }

        /// <summary>
        /// フォームの閉じるボタン（×）が押されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPathSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 「選択」以外で閉じた場合はキャンセル扱いに
            if (this.DialogResult != DialogResult.OK)
            {
                this.DialogResult = DialogResult.No;
            }
        }

        /// <summary>
        /// 「戻る」ボタン押下時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        /// <summary>
        /// フォームロード時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPathSetting_Load(object sender, EventArgs e)
        {
            // データグリッドビューの初期設定
            if (dataGridView1.Rows.Count > 0)
            {
                // すべての選択をクリア
                dataGridView1.ClearSelection();

                // 1行目を選択状態（青色）にする
                dataGridView1.Rows[0].Selected = true;

                // カレントセルも1行目1列目にする（キーボード操作も安定）
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
            }

            // パスをConfigファイルから取得して表示
            clsIniData objIniData = new clsIniData();
            objIniData.GetIniData();
            if (title == "Python")
            {
                this.Text = "Pythonのパス設定";
                lblTitle.Text = "Pythonのパスを設定してください";
                lblPath.Text = objIniData.getPythonPath;
            }
            else if (title == "FFmpeg")
            {
                this.Text = "FFmpegのパス設定";
                lblTitle.Text = "FFmpegのパスを設定してください";
                lblPath.Text = objIniData.getFFmpegPath;
            }
        }

        /// <summary>
        /// DataGridViewのセルがクリックされたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // すべての選択を解除
                dataGridView1.ClearSelection();

                // クリックされた行を選択状態（青色）にする
                dataGridView1.Rows[e.RowIndex].Selected = true;

                // カレントセルも設定（必要に応じて）
                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        /// <summary>
        /// DataGridViewのセルをダブルクリックしたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // セルがクリックされたときの処理を呼び出す
            dataGridView1_CellClick(sender, e);
            
            // ヘッダー行でないか確認
            if (e.RowIndex >= 0)
            {
                // もしその行が選択されていなければ何もしない（スルー）
                if (!dataGridView1.Rows[e.RowIndex].Selected)
                {
                    return;
                }

                // （ここ以降は"青色選択状態"のときだけ実行される）
                string path = dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                this.SelectedPath = path.TrimEnd('\\') + @"\";

                Debug.WriteLine($"Selected Path: {SelectedPath}");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 選択されたパスを取得してフォームを閉じるメソッド
        /// </summary>
        private void SetSelectedPathAndClose()
        {
            if (dataGridView1.CurrentRow != null)
            {
                SelectedPath = dataGridView1.CurrentRow.Cells[0].Value?.ToString() + @"\";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
