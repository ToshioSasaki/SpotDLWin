using System;

namespace MusicDLWin
{
    partial class MusicDL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MusicDL));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ダウンロードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.出力先ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.アプリを終了しますToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultText = new System.Windows.Forms.RichTextBox();
            this.Download = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textOutDir = new System.Windows.Forms.TextBox();
            this.URL = new System.Windows.Forms.Label();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.textAlbumName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.StopButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.終了ToolStripMenuItem,
            this.ヘルプToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(878, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ダウンロードToolStripMenuItem,
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem,
            this.出力先ToolStripMenuItem});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            // 
            // ダウンロードToolStripMenuItem
            // 
            this.ダウンロードToolStripMenuItem.Name = "ダウンロードToolStripMenuItem";
            this.ダウンロードToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ダウンロードToolStripMenuItem.Text = "ﾀﾞｳﾝﾛｰﾄﾞ";
            this.ダウンロードToolStripMenuItem.Click += new System.EventHandler(this.ダウンロードToolStripMenuItem_Click);
            // 
            // 出力先ToolStripMenuItem
            // 
            this.出力先ToolStripMenuItem.Name = "出力先ToolStripMenuItem";
            this.出力先ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.出力先ToolStripMenuItem.Text = "出力先";
            this.出力先ToolStripMenuItem.Click += new System.EventHandler(this.出力先ToolStripMenuItem_Click);
            // 
            // 終了ToolStripMenuItem
            // 
            this.終了ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.アプリを終了しますToolStripMenuItem});
            this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
            this.終了ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.終了ToolStripMenuItem.Text = "終了";
            // 
            // アプリを終了しますToolStripMenuItem
            // 
            this.アプリを終了しますToolStripMenuItem.Name = "アプリを終了しますToolStripMenuItem";
            this.アプリを終了しますToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.アプリを終了しますToolStripMenuItem.Text = "アプリを終了します";
            this.アプリを終了しますToolStripMenuItem.Click += new System.EventHandler(this.アプリを終了しますToolStripMenuItem_Click);
            // 
            // ヘルプToolStripMenuItem
            // 
            this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.ヘルプToolStripMenuItem.Text = "ヘルプ";
            this.ヘルプToolStripMenuItem.Click += new System.EventHandler(this.ヘルプToolStripMenuItem_Click);
            // 
            // ResultText
            // 
            this.ResultText.BackColor = System.Drawing.Color.YellowGreen;
            this.ResultText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResultText.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ResultText.Location = new System.Drawing.Point(0, 427);
            this.ResultText.Name = "ResultText";
            this.ResultText.Size = new System.Drawing.Size(878, 254);
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
            this.Download.Location = new System.Drawing.Point(812, 359);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(65, 67);
            this.Download.TabIndex = 5;
            this.Download.UseVisualStyleBackColor = false;
            this.Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.ForestGreen;
            this.label2.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(3, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 19);
            this.label2.TabIndex = 27;
            this.label2.Text = "出力先フォルダ指定";
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.CausesValidation = false;
            this.button1.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(565, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 69);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textOutDir
            // 
            this.textOutDir.BackColor = System.Drawing.Color.YellowGreen;
            this.textOutDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textOutDir.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textOutDir.Location = new System.Drawing.Point(3, 280);
            this.textOutDir.Name = "textOutDir";
            this.textOutDir.Size = new System.Drawing.Size(556, 28);
            this.textOutDir.TabIndex = 2;
            this.textOutDir.Text = "c:\\spotdl\\";
            // 
            // URL
            // 
            this.URL.AutoSize = true;
            this.URL.BackColor = System.Drawing.Color.ForestGreen;
            this.URL.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.URL.Location = new System.Drawing.Point(4, 140);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(144, 19);
            this.URL.TabIndex = 24;
            this.URL.Text = "リンク又はアドレス";
            // 
            // inputTextBox
            // 
            this.inputTextBox.BackColor = System.Drawing.Color.YellowGreen;
            this.inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputTextBox.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.inputTextBox.Location = new System.Drawing.Point(4, 162);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(556, 28);
            this.inputTextBox.TabIndex = 1;
            // 
            // textAlbumName
            // 
            this.textAlbumName.BackColor = System.Drawing.Color.YellowGreen;
            this.textAlbumName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textAlbumName.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textAlbumName.Location = new System.Drawing.Point(0, 73);
            this.textAlbumName.Name = "textAlbumName";
            this.textAlbumName.Size = new System.Drawing.Size(340, 28);
            this.textAlbumName.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.ForestGreen;
            this.label7.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(0, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(157, 19);
            this.label7.TabIndex = 25;
            this.label7.Text = "プレイリストタイトル";
            // 
            // StopButton
            // 
            this.StopButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("StopButton.BackgroundImage")));
            this.StopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.StopButton.Location = new System.Drawing.Point(738, 357);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(68, 69);
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
            // ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem
            // 
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Name = "ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem";
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Text = "ｱｯﾌﾟﾃﾞｰﾄ";
            this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem.Click += new System.EventHandler(this.ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.progressBar1.ForeColor = System.Drawing.Color.Chartreuse;
            this.progressBar1.Location = new System.Drawing.Point(4, 410);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(728, 16);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 30;
            this.progressBar1.Value = 1;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // MusicDL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::MusicDLWin.Properties.Resources.MusicDL1;
            this.ClientSize = new System.Drawing.Size(878, 677);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.ResultText);
            this.Controls.Add(this.Download);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textOutDir);
            this.Controls.Add(this.URL);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.textAlbumName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MusicDL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MusicDL Windows";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MusicDL_FormClosed);
            this.Load += new System.EventHandler(this.SpotDL_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textOutDir;
        private System.Windows.Forms.Label URL;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.TextBox textAlbumName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem ｱｯﾌﾟﾃﾞｰﾄToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

