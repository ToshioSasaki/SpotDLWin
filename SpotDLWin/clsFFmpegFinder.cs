using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicDLWin
{
    public class FFmpegFinder
    {
        /// <summary>
        /// PythonFinderクラスのコンストラクタです。
        /// </summary>
        /// <param name="rTextBox">RichTextBox</param>
        public FFmpegFinder(RichTextBox rTextBox)
        {
            // コンストラクタ
            Message = new MessageDisplayer(rTextBox);
        }

        /// <summary>
        /// メッセージを表示するためのインスタンスです。
        /// </summary>
        public MessageDisplayer Message;

        /// <summary>
        /// パイソンのインストールディレクトリを取得します。
        /// </summary>
        /// <returns>パイソンのインストールディレクトリのリスト</returns>
        public List<string> GetFFmpegFinder()
        {
            var FFmpegDirs = new List<string>();
            try
            {
                var process = new Process();
                process.StartInfo.FileName = "where";
                process.StartInfo.Arguments = "ffmpeg.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var dir = Path.GetDirectoryName(line.Trim());
                    if (!string.IsNullOrWhiteSpace(dir) && !FFmpegDirs.Contains(dir))
                    {
                        FFmpegDirs.Add(dir);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.UpdateRichTextBox($"FFmpegのディレクトリ取得エラー: {ex.Message}");
            }
            return FFmpegDirs;
        }
    }
}
