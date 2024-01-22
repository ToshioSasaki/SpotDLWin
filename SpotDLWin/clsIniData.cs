using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDLWin
{
    /// <summary>
    /// INIファイルの取得をします。
    /// </summary>
    public class clsIniData
    {
        public string getOutPath { get; set; } = "";
        public int getKaisuu { get; set; } = 0;
        public int getTimeOut { get; set; } = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public clsIniData() {
            this.getOutPath = string.Empty;
            this.getTimeOut = 0;
            this.getKaisuu = 0;
        }

        /// <summary>
        /// INIファイルの取得
        /// </summary>
        public void GetIniData() {
            this.getOutPath = MusicDLWin.Properties.Settings.Default.OutPath;
            this.getKaisuu = MusicDLWin.Properties.Settings.Default.Kaisuu;
            this.getTimeOut = MusicDLWin.Properties.Settings.Default.TimeOut;
        }

        /// <summary>
        /// 環境設定ファイルの値の設定
        /// </summary>
        /// <param name="path">パス</param>
        public void SetIniData(string path)
        {
            MusicDLWin.Properties.Settings.Default.OutPath = path;
            MusicDLWin.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 環境設定ダウンロード試行回数
        /// </summary>
        /// <param name="kaisuu"></param>
        public void SetKaisuu(int kaisuu)
        {
            MusicDLWin.Properties.Settings.Default.Kaisuu = kaisuu;
            MusicDLWin.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 環境設定TimeOut
        /// </summary>
        /// <param name="timeOut"></param>
        public void SetTimeOut(int timeOut)
        {
            MusicDLWin.Properties.Settings.Default.TimeOut = timeOut;
            MusicDLWin.Properties.Settings.Default.Save();
        }
    }
}
