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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupURL = new System.Windows.Forms.Panel();
            this.URL = new System.Windows.Forms.Label();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.groupOutput = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textOutDir = new System.Windows.Forms.TextBox();
            this.groupResult = new System.Windows.Forms.Panel();
            this.groupDownload = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.Download = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupCommand = new System.Windows.Forms.Panel();
            this.Result = new System.Windows.Forms.Label();
            this.ResultText = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupURL.SuspendLayout();
            this.groupOutput.SuspendLayout();
            this.groupDownload.SuspendLayout();
            this.groupCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.LightBlue;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.終了ToolStripMenuItem,
            this.ヘルプToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1703, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ダウンロードToolStripMenuItem,
            this.出力先ToolStripMenuItem});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            // 
            // ダウンロードToolStripMenuItem
            // 
            this.ダウンロードToolStripMenuItem.Name = "ダウンロードToolStripMenuItem";
            this.ダウンロードToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.ダウンロードToolStripMenuItem.Text = "ダウンロード";
            this.ダウンロードToolStripMenuItem.Click += new System.EventHandler(this.ダウンロードToolStripMenuItem_Click);
            // 
            // 出力先ToolStripMenuItem
            // 
            this.出力先ToolStripMenuItem.Name = "出力先ToolStripMenuItem";
            this.出力先ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.groupURL);
            this.panel1.Controls.Add(this.groupOutput);
            this.panel1.Controls.Add(this.groupResult);
            this.panel1.Controls.Add(this.groupDownload);
            this.panel1.Controls.Add(this.groupCommand);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1703, 634);
            this.panel1.TabIndex = 13;
            // 
            // groupURL
            // 
            this.groupURL.AllowDrop = true;
            this.groupURL.Controls.Add(this.URL);
            this.groupURL.Controls.Add(this.inputTextBox);
            this.groupURL.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupURL.Location = new System.Drawing.Point(0, 0);
            this.groupURL.Name = "groupURL";
            this.groupURL.Size = new System.Drawing.Size(1703, 67);
            this.groupURL.TabIndex = 11;
            // 
            // URL
            // 
            this.URL.AutoSize = true;
            this.URL.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.URL.Location = new System.Drawing.Point(0, 12);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(144, 19);
            this.URL.TabIndex = 7;
            this.URL.Text = "リンク又はアドレス";
            // 
            // inputTextBox
            // 
            this.inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputTextBox.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.inputTextBox.Location = new System.Drawing.Point(0, 39);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(1703, 28);
            this.inputTextBox.TabIndex = 0;
            // 
            // groupOutput
            // 
            this.groupOutput.Controls.Add(this.label2);
            this.groupOutput.Controls.Add(this.label1);
            this.groupOutput.Controls.Add(this.button1);
            this.groupOutput.Controls.Add(this.textOutDir);
            this.groupOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupOutput.Location = new System.Drawing.Point(0, 70);
            this.groupOutput.Name = "groupOutput";
            this.groupOutput.Size = new System.Drawing.Size(1703, 66);
            this.groupOutput.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(1520, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 19);
            this.label2.TabIndex = 17;
            this.label2.Text = "出力先フォルダ指定";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "出力先";
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.CausesValidation = false;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1664, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 38);
            this.button1.TabIndex = 15;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textOutDir
            // 
            this.textOutDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textOutDir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textOutDir.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textOutDir.Location = new System.Drawing.Point(0, 38);
            this.textOutDir.Name = "textOutDir";
            this.textOutDir.Size = new System.Drawing.Size(1703, 28);
            this.textOutDir.TabIndex = 1;
            this.textOutDir.Text = "c:\\spotdl\\";
            // 
            // groupResult
            // 
            this.groupResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupResult.Location = new System.Drawing.Point(0, 136);
            this.groupResult.Name = "groupResult";
            this.groupResult.Size = new System.Drawing.Size(1703, 10);
            this.groupResult.TabIndex = 14;
            // 
            // groupDownload
            // 
            this.groupDownload.Controls.Add(this.label4);
            this.groupDownload.Controls.Add(this.Download);
            this.groupDownload.Controls.Add(this.label3);
            this.groupDownload.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupDownload.Location = new System.Drawing.Point(0, 146);
            this.groupDownload.Name = "groupDownload";
            this.groupDownload.Size = new System.Drawing.Size(1703, 36);
            this.groupDownload.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(1521, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 19);
            this.label4.TabIndex = 16;
            this.label4.Text = "ダウンロードボタン";
            // 
            // Download
            // 
            this.Download.BackColor = System.Drawing.SystemColors.Control;
            this.Download.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Download.BackgroundImage")));
            this.Download.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Download.Dock = System.Windows.Forms.DockStyle.Right;
            this.Download.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Download.ForeColor = System.Drawing.Color.Transparent;
            this.Download.Location = new System.Drawing.Point(1664, 0);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(39, 36);
            this.Download.TabIndex = 15;
            this.Download.UseVisualStyleBackColor = false;
            this.Download.Click += new System.EventHandler(this.Download_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 19);
            this.label3.TabIndex = 14;
            this.label3.Text = "結果表示";
            // 
            // groupCommand
            // 
            this.groupCommand.Controls.Add(this.Result);
            this.groupCommand.Controls.Add(this.ResultText);
            this.groupCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupCommand.Location = new System.Drawing.Point(0, 182);
            this.groupCommand.Name = "groupCommand";
            this.groupCommand.Size = new System.Drawing.Size(1703, 452);
            this.groupCommand.TabIndex = 12;
            // 
            // Result
            // 
            this.Result.AutoSize = true;
            this.Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Result.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Result.Location = new System.Drawing.Point(0, 449);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(69, 19);
            this.Result.TabIndex = 9;
            this.Result.Text = "結果表示";
            // 
            // ResultText
            // 
            this.ResultText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResultText.Dock = System.Windows.Forms.DockStyle.Top;
            this.ResultText.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ResultText.Location = new System.Drawing.Point(0, 0);
            this.ResultText.Name = "ResultText";
            this.ResultText.Size = new System.Drawing.Size(1703, 449);
            this.ResultText.TabIndex = 6;
            this.ResultText.Text = "";
            // 
            // MusicDL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1703, 658);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MusicDL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MusicDL Windows";
            this.Load += new System.EventHandler(this.SpotDL_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupURL.ResumeLayout(false);
            this.groupURL.PerformLayout();
            this.groupOutput.ResumeLayout(false);
            this.groupOutput.PerformLayout();
            this.groupDownload.ResumeLayout(false);
            this.groupDownload.PerformLayout();
            this.groupCommand.ResumeLayout(false);
            this.groupCommand.PerformLayout();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel groupURL;
        private System.Windows.Forms.Label URL;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Panel groupOutput;
        private System.Windows.Forms.TextBox textOutDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel groupResult;
        private System.Windows.Forms.Panel groupDownload;
        private System.Windows.Forms.Panel groupCommand;
        private System.Windows.Forms.Label Result;
        private System.Windows.Forms.RichTextBox ResultText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Download;
        private System.Windows.Forms.Label label3;
    }
}

