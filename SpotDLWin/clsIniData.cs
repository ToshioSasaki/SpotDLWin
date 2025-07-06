using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDLWin
{
    /// <summary>
    /// Configファイルの取得をします。
    /// </summary>
    public class clsIniData
    {
        public string getOutPath { get; set; } = "";
        public string getPythonPath { get; private set; } = "";
        public string getFFmpegPath { get; private set; } = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public clsIniData() {
            this.getOutPath = string.Empty;
            this.getPythonPath = string.Empty;
            this.getFFmpegPath = string.Empty;
        }

        /// <summary>
        /// Configファイルの一括取得
        /// </summary>
        public void GetIniData() {
            this.getOutPath = MusicDLWin.Properties.Settings.Default.OutPath;
            this.getPythonPath = MusicDLWin.Properties.Settings.Default.PythonPath;
            this.getFFmpegPath = MusicDLWin.Properties.Settings.Default.FFmpegPath;
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
        /// Pythonのパスを設定します。
        /// </summary>
        /// <param name="path"></param>
        public void SetPythonPath(string path)
        {
            MusicDLWin.Properties.Settings.Default.PythonPath = path;
            MusicDLWin.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// FFmpegのパスを設定します。
        /// </summary>
        /// <param name="path"></param>
        public void SetFFmpegPath(string path)
        {
            MusicDLWin.Properties.Settings.Default.FFmpegPath = path;
            MusicDLWin.Properties.Settings.Default.Save();
        }
    }
}
