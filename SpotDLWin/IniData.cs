using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDLWin
{
    public class IniData
    {
        public string OutPath { get; set; } = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IniData() {
            this.OutPath = string.Empty;
        }

        /// <summary>
        /// INIファイルの取得
        /// </summary>
        public void GetIniData() {
            this.OutPath = MusicDLWin.Properties.Settings.Default.OutPath;

        }

        /// <summary>
        /// 設定ファイルの値の設定
        /// </summary>
        /// <param name="path">パス</param>
        public void SetIniData(string path)
        {
            MusicDLWin.Properties.Settings.Default.OutPath = path;
            MusicDLWin.Properties.Settings.Default.Save();
        }

        
    }
}
