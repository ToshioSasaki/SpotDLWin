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

            string EncryptedHelpMessage = "q00GkIcQ1DUbgcDCustw7JVRkj4JQvVAFS4T/feHMRPY/4oSrJ"
                                        + "Cw+dUMcq1fNOdJqJslqbzLIfcmzHg4V2fMYPTzlLTQldor87AT"
                                        + "cQUP/bAUc/RhlchrSjzH/F5zKLtYxhPCoGfreNTz/fXsN7uYI5"
                                        + "ZdmT8i2oJD6oElactDrbMGEstWSGX3PyQ3MeMHwx6HfkSyoJ0Z"
                                        + "51I7VJ6P7MGXWksCeNZdSrRJdbwJu7A3z/UXxWrfysAUAVAxlg"
                                        + "kykgfa/zdaGmdDJIyuuGAqmjiaxVQ7XrprJlDFwwLJYvwmf80L"
                                        + "VzhlIdhfNTh8ASraH9CBq+ydrhW9F75hwqlPGF9s8CbyIjP9Kq"
                                        + "GQFlayNYixAqnH3YQFcqllGrCasL9sFBLPFshJP4TbYrC6oz3v"
                                        + "ZpOvuMiJC9tOzl+uOoSXhEzfWOMVK2QNbL7rfZ/DXKVjiRVzE1"
                                        + "eNKjJrzyor9jSDlbBN1GNXMwZe49+B5CylTOD/tr+3mx7FhKPR"
                                        + "sdB6kW7pNnA/MsdFc6RoQ+3ngkgeO6SBt+CJ3Wj2SXFgsdNb4T"
                                        + "6osBTV7HgODh/V/aGpvGhgZjHLDS2xSxC5jEza9lMaFelnKxPR"
                                        + "vGKCUzvQUuSGbVEJSAjYHNafFRh+uqOJmWssRLw4IMcDRyZrrT"
                                        + "5b+wBoaDhfqQpHHxIfTm9ZF9yQ6cRq0Izlq82Pv76qxo7uus/H"
                                        + "o5XQAHo+5T9XmsmW7LJY47hLJ6C7zLvVObM90zGMxPTBMMbpBT"
                                        + "7mGVchJsA0eflAx9pru/dfn92JNtuHf6F8y6cXDiZSK+FhczMT"
                                        + "XNeuobeGTPxoijY0vCS5StT8d3ftK4n0tCm0N7tVz8PjHx7Qpe"
                                        + "dk8z8B+/zTJ9IREM8NcJdT/12hFebx4VZElri2+wPEdEf305d/"
                                        + "98hl8dxuMGnaBKy/bbM0p3v7UAZU8PbbGdy7gM7V9rRXBtACye"
                                        + "2pHUOV1Vjk6WfQLwYyt7XtOVUUoLGAXB38gpqpQvVzsFMj2WXj"
                                        + "Txy55HOt1sHqGXfPpJM2ZcGdIQeUTeNFsvixQj0/gbUyT1TBXa"
                                        + "mifYcxjBWAb3rlaN9Yg7zunAcxO6X8lFRw3u2Sf2RqGAir6sAr"
                                        + "t4YKLz+HXWV/YG6Ow1AD9D7lBILRyRx3QJIldXLPmt2MJjQNKj"
                                        + "0avCivgrzHNiSW2/Gnt8nver5tMCeGJ93+MbOtReSzUeH3SzM6"
                                        + "euzNGNsnDH+rqPjxY3Cl8omI2MuSMm+WhL9KflN0csHqhyJp7z"
                                        + "vX02+LHMumZa6chMS/b8lA+rR3hNR5SSIe1wrwaVK5DG6xNriU"
                                        + "teRJ1Qx4E6cFm6ZIU9SbUnzWtkGMKmfswq/xX6kKKZIR67aqVM"
                                        + "QBrsTRWNwr5HVqKHq9iMxSHgjOb+jux0JcNTYEKEUNDO1QOG/Z"
                                        + "dcvyZHxK7TcUoB/R9ZxflYTLbB09Obq/eNXOjj3MB9NIqSDGNf"
                                        + "ybhJWUZOeG1XzSnJINQBci8QN3cxXihWrQWygoD4H6zPqQf4zs"
                                        + "IyoXg1UKyRYnqOaPAsw2g9uVaACAMpz8z39Jx0ly61rnNe3yAj"
                                        + "ZkZdKJUbVnfM7UIN/dffndTrVjl+LaN6+FXAsWQVGJeeP8oOYj"
                                        + "bV2nk6xS1h8xq/6s/Lvlm1BpOgkPN9yyAZpxPDtucT8MlpxdMs"
                                        + "cByN/HMK8L2qVIOVNV7HcI05BJIJUSX+TvfIwQqQS9Nq15EhMI"
                                        + "F1z+qd45C2yiZcn+8xDqcUUTMHVdqgCrKQon9PgHQRltFTdnW/"
                                        + "Kk6WQ7XGFDAXjq04sDM6O3QylT6f+yw5VbaNPuYoQkyh+/PNTr"
                                        + "qodVlqXlQI97Se3+QmgkQy3b//ujjfRpt1W+H5wp6xRUMWz2sO"
                                        + "WLnELy7Ec6l/zPJfOdugKPoGY9iJ3NjopB0LkJbBk/Ro1BeFVa"
                                        + "tBZRX5AZjAIlTZ046Xqhr4D6DDu7n4IJpuHVhBdTA3pytbskFR"
                                        + "2nvEj0AWKKnG8mAyF3cFoD9IDswRg3v406OPdsv8xSBr8AJfqS"
                                        + "vNaMXXOMXWD211HIdBIFAmmvo8H9CSYuZlBB3zEapmUcPtnLJZ"
                                        + "gLX1ARiWrts9h6lZ40qETfNQxCcRmYVu9m2k/s8mVU6HNYG6aC"
                                        + "ejYE/hrq4HwcgqoU2frUS5L+QVy2p4rsqgj2fyelN1RMdLWsH7"
                                        + "dBAMMTluUw5wsvzjeAhIDd8xRfqz+NLbjzEk3bKvs7qGa2pHgj"
                                        + "z8c3NPFx4h8pak2M6jWr8UDV130m2EdwpugkjO5HAN4w5GujMj"
                                        + "7JciS0aM1Os51O7cFoZ//6G0ygMR+GfIY=";

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
