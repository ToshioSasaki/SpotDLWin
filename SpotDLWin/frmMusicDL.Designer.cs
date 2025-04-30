using System;

namespace MusicDLWin
{
    partial class frmMusicDL
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMusicDL));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ダウンロードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.出力先ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.一括ｲﾝｽﾄｰﾙToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.アプリを終了しますToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.試行回数ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.試行回数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultText = new System.Windows.Forms.RichTextBox();
            this.Download = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textOutDir = new System.Windows.Forms.TextBox();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.textAlbumName = new System.Windows.Forms.TextBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.picturePlayList = new System.Windows.Forms.PictureBox();
            this.pictureLinkAddress = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePlayList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLinkAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.終了ToolStripMenuItem,
            this.ヘルプToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1171, 28);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ダウンロードToolStripMenuItem,
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem,
            this.musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem,
            this.出力先ToolStripMenuItem,
            this.一括ｲﾝｽﾄｰﾙToolStripMenuItem});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            this.ファイルToolStripMenuItem.Click += new System.EventHandler(this.ファイルToolStripMenuItem_Click);
            // 
            // ダウンロードToolStripMenuItem
            // 
            this.ダウンロードToolStripMenuItem.Name = "ダウンロードToolStripMenuItem";
            this.ダウンロードToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ダウンロードToolStripMenuItem.Text = "ﾀﾞｳﾝﾛｰﾄﾞ";
            this.ダウンロードToolStripMenuItem.Click += new System.EventHandler(this.ダウンロードToolStripMenuItem_Click);
            // 
            // ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem
            // 
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Enabled = false;
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Name = "ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem";
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Text = "Phytonｱｯﾌﾟﾃﾞｰﾄ";
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Click += new System.EventHandler(this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem_Click);
            // 
            // musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem
            // 
            this.musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Name = "musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem";
            this.musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Text = "MusilDLｱｯﾌﾟﾃﾞｰﾄ";
            this.musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Click += new System.EventHandler(this.musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem_Click);
            // 
            // 出力先ToolStripMenuItem
            // 
            this.出力先ToolStripMenuItem.Name = "出力先ToolStripMenuItem";
            this.出力先ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.出力先ToolStripMenuItem.Text = "出力先";
            this.出力先ToolStripMenuItem.Click += new System.EventHandler(this.出力先ToolStripMenuItem_Click);
            // 
            // 一括ｲﾝｽﾄｰﾙToolStripMenuItem
            // 
            this.一括ｲﾝｽﾄｰﾙToolStripMenuItem.Enabled = false;
            this.一括ｲﾝｽﾄｰﾙToolStripMenuItem.Name = "一括ｲﾝｽﾄｰﾙToolStripMenuItem";
            this.一括ｲﾝｽﾄｰﾙToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.一括ｲﾝｽﾄｰﾙToolStripMenuItem.Text = "一括ｲﾝｽﾄｰﾙ";
            this.一括ｲﾝｽﾄｰﾙToolStripMenuItem.Click += new System.EventHandler(this.一括ｲﾝｽﾄｰﾙToolStripMenuItem_Click);
            // 
            // 終了ToolStripMenuItem
            // 
            this.終了ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.アプリを終了しますToolStripMenuItem});
            this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
            this.終了ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.終了ToolStripMenuItem.Text = "終了";
            // 
            // アプリを終了しますToolStripMenuItem
            // 
            this.アプリを終了しますToolStripMenuItem.Name = "アプリを終了しますToolStripMenuItem";
            this.アプリを終了しますToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.アプリを終了しますToolStripMenuItem.Text = "アプリを終了します";
            this.アプリを終了しますToolStripMenuItem.Click += new System.EventHandler(this.アプリを終了しますToolStripMenuItem_Click);
            // 
            // ヘルプToolStripMenuItem
            // 
            this.ヘルプToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.試行回数ToolStripMenuItem1,
            this.試行回数ToolStripMenuItem});
            this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.ヘルプToolStripMenuItem.Text = "設定";
            // 
            // 試行回数ToolStripMenuItem1
            // 
            this.試行回数ToolStripMenuItem1.Name = "試行回数ToolStripMenuItem1";
            this.試行回数ToolStripMenuItem1.Size = new System.Drawing.Size(215, 26);
            this.試行回数ToolStripMenuItem1.Text = "TimeOut/試行回数";
            this.試行回数ToolStripMenuItem1.Click += new System.EventHandler(this.試行回数ToolStripMenuItem1_Click);
            // 
            // 試行回数ToolStripMenuItem
            // 
            this.試行回数ToolStripMenuItem.Name = "試行回数ToolStripMenuItem";
            this.試行回数ToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(58, 24);
            this.toolStripMenuItem1.Text = "ヘルプ";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // ResultText
            // 
            this.ResultText.BackColor = System.Drawing.Color.YellowGreen;
            this.ResultText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResultText.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ResultText.Location = new System.Drawing.Point(0, 534);
            this.ResultText.Margin = new System.Windows.Forms.Padding(4);
            this.ResultText.Name = "ResultText";
            this.ResultText.Size = new System.Drawing.Size(1169, 316);
            this.ResultText.TabIndex = 23;
            this.ResultText.Text = "";
            // 
            // Download
            // 
            this.Download.BackColor = System.Drawing.SystemColors.Control;
            this.Download.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Download.BackgroundImage")));
            this.Download.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Download.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Download.ForeColor = System.Drawing.Color.Transparent;
            this.Download.Location = new System.Drawing.Point(1083, 449);
            this.Download.Margin = new System.Windows.Forms.Padding(4);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(87, 84);
            this.Download.TabIndex = 5;
            this.Download.UseVisualStyleBackColor = false;
            this.Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.CausesValidation = false;
            this.button1.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(853, 319);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 86);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textOutDir
            // 
            this.textOutDir.BackColor = System.Drawing.Color.YellowGreen;
            this.textOutDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textOutDir.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textOutDir.Location = new System.Drawing.Point(4, 350);
            this.textOutDir.Margin = new System.Windows.Forms.Padding(4);
            this.textOutDir.Name = "textOutDir";
            this.textOutDir.Size = new System.Drawing.Size(838, 34);
            this.textOutDir.TabIndex = 2;
            this.textOutDir.Text = "c:\\spotdl\\";
            // 
            // inputTextBox
            // 
            this.inputTextBox.BackColor = System.Drawing.Color.YellowGreen;
            this.inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputTextBox.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.inputTextBox.Location = new System.Drawing.Point(4, 224);
            this.inputTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(837, 34);
            this.inputTextBox.TabIndex = 1;
            // 
            // textAlbumName
            // 
            this.textAlbumName.BackColor = System.Drawing.Color.YellowGreen;
            this.textAlbumName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textAlbumName.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textAlbumName.Location = new System.Drawing.Point(1, 91);
            this.textAlbumName.Margin = new System.Windows.Forms.Padding(4);
            this.textAlbumName.Name = "textAlbumName";
            this.textAlbumName.Size = new System.Drawing.Size(743, 34);
            this.textAlbumName.TabIndex = 0;
            // 
            // StopButton
            // 
            this.StopButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("StopButton.BackgroundImage")));
            this.StopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.StopButton.Location = new System.Drawing.Point(984, 446);
            this.StopButton.Margin = new System.Windows.Forms.Padding(4);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(91, 86);
            this.StopButton.TabIndex = 4;
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Visible = false;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.progressBar1.ForeColor = System.Drawing.Color.Chartreuse;
            this.progressBar1.Location = new System.Drawing.Point(5, 506);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1069, 26);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 30;
            this.progressBar1.Value = 1;
            // 
            // picturePlayList
            // 
            this.picturePlayList.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picturePlayList.BackgroundImage")));
            this.picturePlayList.Location = new System.Drawing.Point(4, 59);
            this.picturePlayList.Margin = new System.Windows.Forms.Padding(4);
            this.picturePlayList.Name = "picturePlayList";
            this.picturePlayList.Size = new System.Drawing.Size(203, 31);
            this.picturePlayList.TabIndex = 31;
            this.picturePlayList.TabStop = false;
            // 
            // pictureLinkAddress
            // 
            this.pictureLinkAddress.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureLinkAddress.BackgroundImage")));
            this.pictureLinkAddress.Location = new System.Drawing.Point(7, 190);
            this.pictureLinkAddress.Margin = new System.Windows.Forms.Padding(4);
            this.pictureLinkAddress.Name = "pictureLinkAddress";
            this.pictureLinkAddress.Size = new System.Drawing.Size(203, 32);
            this.pictureLinkAddress.TabIndex = 32;
            this.pictureLinkAddress.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 315);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(203, 35);
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // frmMusicDL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::MusicDLWin.Properties.Resources.MusicDL1;
            this.ClientSize = new System.Drawing.Size(1171, 846);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureLinkAddress);
            this.Controls.Add(this.picturePlayList);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.ResultText);
            this.Controls.Add(this.Download);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textOutDir);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.textAlbumName);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMusicDL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MusicDL Windows";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MusicDL_FormClosed);
            this.Load += new System.EventHandler(this.SpotDL_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePlayList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLinkAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ダウンロードToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 出力先ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem アプリを終了しますToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.RichTextBox ResultText;
        private System.Windows.Forms.Button Download;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textOutDir;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.TextBox textAlbumName;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem;
        private System.Windows.Forms.PictureBox picturePlayList;
        private System.Windows.Forms.PictureBox pictureLinkAddress;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem 試行回数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 試行回数ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem musilDLｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 一括ｲﾝｽﾄｰﾙToolStripMenuItem;
        public System.Windows.Forms.ProgressBar progressBar1;
    }
}

