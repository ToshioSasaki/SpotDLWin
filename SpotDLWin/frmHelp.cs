using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MusicDLWin
{
    public partial class frmHelp : Form
    {
        public frmHelp()
        {
            InitializeComponent();
        }

        private void frmHelp_Load(object sender, EventArgs e)
        {
            HelpRichText.Text = GetHelpMessage();
        }

        public string GetHelpMessage()
        {
            byte[] fullData = Convert.FromBase64String(HelpMessage());
            byte[] key = Encoding.UTF8.GetBytes("ThisIsA16ByteKey"); // 暗号化時と同じキー
            byte[] iv = new byte[16];
            Array.Copy(fullData, 0, iv, 0, 16);
            byte[] cipherText = new byte[fullData.Length - 16];
            Array.Copy(fullData, 16, cipherText, 0, cipherText.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (MemoryStream ms = new MemoryStream(cipherText))
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private string HelpMessage()
        {
            string EncryptedHelpMessage = @"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";
            return EncryptedHelpMessage;
        }

        private void 戻るToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void HelpRichText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
