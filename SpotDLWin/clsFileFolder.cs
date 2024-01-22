using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicDLWin
{
    public class clsFileFolder
    {
        public clsFileFolder() {
            
        }

        /// <summary>
        /// フォルダーを指定先に移動します。
        /// </summary>
        /// <param name="sourceDir">コピー元</param>
        /// <param name="destDir">コピー先</param>
        /// <param name="DeleteFlg">コピー元を削除するTrue</param>
        public void MoveFolder(string sourceDir, string destDir,bool DeleteFlg=false)
        {
            // 目的のディレクトリが存在しない場合は作成
            this.CreateDirectory(destDir);

            // ソースディレクトリ内のすべてのファイルを移動
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile);
            }

            // サブディレクトリを再帰的に処理
            foreach (string subdir in Directory.GetDirectories(sourceDir))
            {
                string destSubdir = Path.Combine(destDir, Path.GetFileName(subdir));
                MoveFolder(subdir, destSubdir);
            }

            // 元のディレクトリを削除（空になっているはず）
            if (DeleteFlg)
            {
                this.DeleteFolderContents(sourceDir);
            }
        }

        /// <summary>
        /// 指定したフォルダ内の中身を全て削除します。
        /// </summary>
        /// <param name="folderPath">指定フォルダ</param>
        public void DeleteFolderContents(string folderPath)
        {
            // 指定されたディレクトリ内のすべてのファイルを削除
            foreach (string file in Directory.GetFiles(folderPath))
            {
                File.Delete(file);
            }

            // 指定されたディレクトリ内のすべてのサブディレクトリを削除（中身も含む）
            foreach (string dir in Directory.GetDirectories(folderPath))
            {
                Directory.Delete(dir, true);
            }
        }

        /// <summary>
        /// 指定したフォルダが無ければ作成します。
        /// </summary>
        /// <param name="destDir">指定先フォルダ</param>
        public void CreateDirectory(string destDir)
        {
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
        }

    }
}
