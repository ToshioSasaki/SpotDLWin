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
