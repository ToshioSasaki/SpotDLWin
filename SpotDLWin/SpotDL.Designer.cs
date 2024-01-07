using System;

namespace SpotDLWin
{
    partial class SpotDL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotDL));
            this.groupResult = new System.Windows.Forms.Panel();
            this.groupDownload = new System.Windows.Forms.Panel();
            this.Download = new System.Windows.Forms.Button();
            this.groupCommand = new System.Windows.Forms.Panel();
            this.groupURL = new System.Windows.Forms.Panel();
            this.URL = new System.Windows.Forms.Label();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.groupOutput = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.textOutDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ResultText = new System.Windows.Forms.RichTextBox();
            this.Result = new System.Windows.Forms.Label();
            this.groupDownload.SuspendLayout();
            this.groupCommand.SuspendLayout();
            this.groupURL.SuspendLayout();
            this.groupOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupResult
            // 
            this.groupResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupResult.Location = new System.Drawing.Point(0, 154);
            this.groupResult.Name = "groupResult";
            this.groupResult.Size = new System.Drawing.Size(1075, 10);
            this.groupResult.TabIndex = 9;
            // 
            // groupDownload
            // 
            this.groupDownload.Controls.Add(this.Download);
            this.groupDownload.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupDownload.Location = new System.Drawing.Point(0, 52);
            this.groupDownload.Name = "groupDownload";
            this.groupDownload.Size = new System.Drawing.Size(1075, 36);
            this.groupDownload.TabIndex = 8;
            // 
            // Download
            // 
            this.Download.BackColor = System.Drawing.SystemColors.Control;
            this.Download.BackgroundImage = global::SpotDLWin.Properties.Resources.download_transparent;
            this.Download.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Download.Dock = System.Windows.Forms.DockStyle.Right;
            this.Download.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Download.ForeColor = System.Drawing.Color.Transparent;
            this.Download.Location = new System.Drawing.Point(1036, 0);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(39, 36);
            this.Download.TabIndex = 11;
            this.Download.UseVisualStyleBackColor = false;
            this.Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // groupCommand
            // 
            this.groupCommand.Controls.Add(this.Result);
            this.groupCommand.Controls.Add(this.ResultText);
            this.groupCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupCommand.Location = new System.Drawing.Point(0, 164);
            this.groupCommand.Name = "groupCommand";
            this.groupCommand.Size = new System.Drawing.Size(1075, 452);
            this.groupCommand.TabIndex = 7;
            this.groupCommand.Paint += new System.Windows.Forms.PaintEventHandler(this.groupCommand_Paint);
            // 
            // groupURL
            // 
            this.groupURL.Controls.Add(this.URL);
            this.groupURL.Controls.Add(this.inputTextBox);
            this.groupURL.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupURL.Location = new System.Drawing.Point(0, 0);
            this.groupURL.Name = "groupURL";
            this.groupURL.Size = new System.Drawing.Size(1075, 52);
            this.groupURL.TabIndex = 6;
            // 
            // URL
            // 
            this.URL.AutoSize = true;
            this.URL.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.URL.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.URL.Location = new System.Drawing.Point(0, 4);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(39, 20);
            this.URL.TabIndex = 7;
            this.URL.Text = "URL";
            // 
            // inputTextBox
            // 
            this.inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputTextBox.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.inputTextBox.Location = new System.Drawing.Point(0, 24);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(1075, 28);
            this.inputTextBox.TabIndex = 0;
            // 
            // groupOutput
            // 
            this.groupOutput.Controls.Add(this.button1);
            this.groupOutput.Controls.Add(this.textOutDir);
            this.groupOutput.Controls.Add(this.label1);
            this.groupOutput.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupOutput.Location = new System.Drawing.Point(0, 88);
            this.groupOutput.Name = "groupOutput";
            this.groupOutput.Size = new System.Drawing.Size(1075, 66);
            this.groupOutput.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.BackgroundImage = global::SpotDLWin.Properties.Resources.folder_transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1036, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 38);
            this.button1.TabIndex = 12;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textOutDir
            // 
            this.textOutDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textOutDir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textOutDir.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textOutDir.Location = new System.Drawing.Point(0, 38);
            this.textOutDir.Name = "textOutDir";
            this.textOutDir.Size = new System.Drawing.Size(1075, 28);
            this.textOutDir.TabIndex = 1;
            this.textOutDir.Text = "c:\\spotdl\\";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "OUTPUT_DIR";
            // 
            // ResultText
            // 
            this.ResultText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResultText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ResultText.Font = new System.Drawing.Font("游ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ResultText.Location = new System.Drawing.Point(0, 23);
            this.ResultText.Name = "ResultText";
            this.ResultText.Size = new System.Drawing.Size(1075, 429);
            this.ResultText.TabIndex = 6;
            this.ResultText.Text = "";
            this.ResultText.TextChanged += new System.EventHandler(this.ResultText_TextChanged_1);
            // 
            // Result
            // 
            this.Result.AutoSize = true;
            this.Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Result.Font = new System.Drawing.Font("游ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Result.Location = new System.Drawing.Point(0, 0);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(53, 20);
            this.Result.TabIndex = 9;
            this.Result.Text = "Result";
            // 
            // SpotDL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1075, 616);
            this.Controls.Add(this.groupOutput);
            this.Controls.Add(this.groupResult);
            this.Controls.Add(this.groupDownload);
            this.Controls.Add(this.groupCommand);
            this.Controls.Add(this.groupURL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpotDL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "spotDL:GUI";
            this.Load += new System.EventHandler(this.SpotDL_Load);
            this.groupDownload.ResumeLayout(false);
            this.groupCommand.ResumeLayout(false);
            this.groupCommand.PerformLayout();
            this.groupURL.ResumeLayout(false);
            this.groupURL.PerformLayout();
            this.groupOutput.ResumeLayout(false);
            this.groupOutput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label URL;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Panel groupURL;
        private System.Windows.Forms.Panel groupCommand;
        private System.Windows.Forms.Panel groupDownload;
        private System.Windows.Forms.Panel groupResult;
        private System.Windows.Forms.Panel groupOutput;
        private System.Windows.Forms.TextBox textOutDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button Download;
        private System.Windows.Forms.Label Result;
        private System.Windows.Forms.RichTextBox ResultText;
    }
}

