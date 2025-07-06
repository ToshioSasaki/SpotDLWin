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
            string EncryptedHelpMessage = @"8un21nc3rdW+gMUn9j6t432uvdBexMDr1gyg7vq6ZvLOfcNhLnA/zV8zE9B1iIglTvVbrSudcDIm14/XPvzQFmnkVpVH9kwOBcydhNA2U+g60cf4S3UahivtMXm2ESjmyfO+uOnaf3yOBpaW/7FWdUh6ezO2q5RubL1Jw0iVDZAZqwXhUFXs42n8n2yL1yPuM13Hslcf0r9ctyxwfrX+3GFzRPv9mgNbQcPtsBeYZTHpCNJ"
                                        + "cYwf/A5yX5gJUmk8C8kIpGx6PanuMxtsUdyiBkQxetRUeAckwR9lBLpQixvdR1hVneOUiZLfuP9pIY6qdMzNBmYp9Th0o5taxkyqIgGsaQNFlxzsEkk+e79WKXoUpFgqN6I1h1uRu0oL6UhOYjJ7kHWFi16D5BPmzRRWdcIJSPL0lou/E5sK52TJrV2FBjXRZqnOfaaSWshDUCVDsjhrRZyhfkNSukb2Nmo5QryVInvFJq0U"
                                        + "2PFkHBYRUk+lOtpoBWKQqb/U3MXk/v6obIkpnEX7KrYeGn9Xd6LfD4cxPGxkP5hkP7sauL3VyLY6YejvMSPdp1T92kPd6d8Zz9tr87mgpV6Oi95KI64qf/nZFM4x1ofvmuXB5va/1bKeEU18ExM0IH0W052+k6cj1CaGWtywtrGdhumuMOtRlXwNFTuxVK0CnRGrwKQJth+8n/BI7WXObPPpELdkxtz8MZguwaZPQbjJ//il"
                                        + "TIzjAXonuzm/GsP9A5xgYjNOheVf0ZDTaXYrGhUGzNfv1Kg8g/k65n5Q6EP598IV8/J4wmw0HMyBrH8MdQNww9/r13tvYDbsNq9gG7oWN3VOerqk3/5ScFq2sl3S1EceOt+S6GFNefO1twP0grEGBv+iGOdW66f9QUFkVOS8OBHEh8bpdEa8xq548WyoK2JHujUrxul/+SmoGOS/owHPZVqSxK2ice5Bk0vtpZhXHF9dNOFq"
                                        + "3svtGNQJESLSaZo29l+qxrL4PeYZb7850Ceu6qdaKAqHQVqHCPZc1OpU9KxaR3fQSLiEHSKmtkylLX9QO78HybRRvhObCNxWwQlXLY2xwaN053RYslghCbHCoYi4qdFf1KyqyzTSqoA5zbwbp0YgiYSecQJZQFv+EUTql+Jst506LmmlQfWv8QeVvjWYNuRihH4AZjqSRDBK3rqrXc/u6j1Mw32ccU30VGuwuBJCB4m8XxHu"
                                        + "5Crc3DK7WjLdJLVdV5jDRrUeGK2cTEguNwptCz8HvU0RHu8Opokxy2AjGKKqP6egta2DXIQB4BT9e4B4RCL/sjGu8r77efr6QioJcz68sFhQ6SzFQpCTnOuN94NHxVdLRP3/xWx310MID06OZFtcgt7n8cgzBMVQr991OG8pKmhtKAgJ8MGLy4sy3mUDSP0pdePvzEdFP4ReyFgfxb4pT21a3bl1Mtp8OJQoGVvLXPgHp5hJ"
                                        + "ey0YiOhnmuiRThOCV3zRb97NbGc4vnnlmSMp01QEy5Vfps8urbJPD3sKUQOd1uG8s64yMB8M5tsCUqYQcj+nCJ/InMxpej2qx/1I8cEIWm5SbatWUMT7fhQWLPoMLhJaQNVOMaJ4yWdsLRaJooL5NWzFs470fH4uoePEzrL6aeiFQcZSdam6mdg9DobtUyS8GLO47uUSMltK5LCoo9/RzQpdOlCBpEoH6aH0hKqr456YBkrW"
                                        + "KIbl2VLOseDU97KiEnqw6K49OpGpYJp5ktoiPbFjcYDn8d75M+jN+xyuzTvTabWBHT/4AaQs+5Ug1Br1dOIOQH7XvYiSaY/4EA8bJ/3JXsKj1dBbbtAu0WuG04T4y1W26uCyN3f2Bwsz43jTT3IJNy09cX4TKPXhtO83CY9Lb7XWkkS4W39nWV9ppVo9OfqWRKFQbInsO31hHSYqHkt/N7bizFQLBDZbORprt9cDjuur2s4m"
                                        + "f4FKCTHqHdxXgdPNnT2JOOC2jIt66ZMKaFxQH5mk0jAKJB744NkmEUkSi4IQf/pnpahKKIXSSi9pfZLgqXL9zO+E+XRnnImzBnZbiDO1jxrVytEmQmHV7OxpFcLwToJDzV8y/B1EGXjd8Ws3ol6GbjmWtP0dJgGXsSHLiGyWQv+KCS/deD5cnblDd3tnewKuYBbsfSVb+tN77xRUpAbxRGrcOKGpBnOYclMPVjkQLGL79psZ"
                                        + "xyno9guwr591DuuXjqTU39dLE70iCTdDET6I0vHJRF7jpeP1atdFbfFbe9YEkRfeXJZNXswxSiNRWjFvyU5Z50TfA6a46LlGHRdxbdG6S4xsoeGsible6Ml97omm1lJx5l82GRPdA2BIYfpDI/s6uP7amX5gUwc0xAPZ+7TVzQ+/oSjkEjeESFwItQ2zEsD2/5GKZrWCPBdwFckYz3RIAd7yISN/ibu3hol/Cc9QoWXuMtFd"
                                        + "wiYUIlahL9eFbP/lau2ug4/v/sxZRae3MtrvZESTYBMaH+s7ige886RtFmOyVqasiT4cVfsOOetf+v8DnqWNpiyc3o8qFZ51IqCE4SOK6a6qqMJFcCOM+VWqzVNcNBUxZXW81oHlNJYp2b0JpvJX059hJcXfj3nPKl3h3r6ompoqgudhYECez/N+/F5xz6KKAW6SK3r8riWWWsIkoZIY+lQ06xsFnCVsyNFNsKbDfstL7Dj6"
                                        + "TS18xG1cF6zGXWuup6CMgHgY8GaeMcTXOMRW2swrTrlfzIqNj2EGzp/v4OHsVol4NCF7upKtZeamMdVeTK9gBWAr/uxCHWuzmhE+qp7o2seeZl110O3ndH3yX6H0Fg+jhUervQ4UWCgqMubc31PFI0BNL2W2QBauc3aHXL39H5O4lS4b9khMA0NjnFLaA5hKVVfCSL8P9kLoIdyv1GJSWn6F7WXwj69EMokEHzKziA2EQS5V"
                                        + "lIrwC++PH0FS+RbWZyqIgTW8frxc046aAqZt9T6N/yvYfmK2RrGO/fq0+WGwBxPfV9peKMGLAEZWzj9gwqjRo1DveN3fTMCddTiozI2oCO1DLSKJHcnOiciY4pG3tIFjqANgswDZNrr2avCE5umSHblPRPLl7vTdSVR3eO91IiFA=";
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
