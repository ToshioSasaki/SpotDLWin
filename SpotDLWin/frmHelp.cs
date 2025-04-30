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
