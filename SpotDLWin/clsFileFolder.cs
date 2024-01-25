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

        /// <summary>
        /// 指定したパス先にフォルダがあるか確認します。
        /// </summary>
        /// <param name="parentDirectoryPath">指定するパス先</param>
        /// <param name="searchFolderName">検索するフォルダ名</param>
        /// <returns></returns>
        public string GetFindDirectory(string parentDirectoryPath, string searchFolderName)
        {
            // 指定した親ディレクトリのサブディレクトリを取得
            DirectoryInfo dir = new DirectoryInfo(parentDirectoryPath);
            DirectoryInfo[] subDirs = dir.GetDirectories(searchFolderName + "*", SearchOption.TopDirectoryOnly);

            // ディレクトリ名に検索テキストが含まれているかチェック
            foreach (DirectoryInfo subDir in subDirs)
            {
                //見つかった場合は最初のものをreturn
                return subDir.FullName.ToString();
            }
            //見つからない場合
            return null;
        }

        /// <summary>
        /// ディレクトリ名を変更します。
        /// </summary>
        /// <param name="originalDirectory">変更前ディレクトリ</param>
        /// <param name="newDirectory">変更後ディレクトリ</param>
        public void RenewDirectory(string originalDirectory,string newDirectory)
        {
            try
            {
                // ディレクトリ名の変更
                Directory.Move(originalDirectory, newDirectory);
            }
            catch (Exception ex)
            {
                // エラーハンドリング
                Console.WriteLine("エラーが発生しました: " + ex.Message);
            }
        }

        /// <summary>
        /// 最後尾のバックスラッシュ(\)を外します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <return>最後尾のバックスラッシュ(\)を外したパス</return>
        public string RemoveBackSlash(string path)
        {
            string returnPath = path;
            if (path.LastIndexOf("\\")>0)
            {
                int lastIndex = path.LastIndexOf("\\");
                returnPath = path.Substring(0,lastIndex);
            }
            return returnPath;
        }

        /// <summary>
        /// 環境パスの設定･即に同じ環境パスが設定してあれば設定しません。
        /// </summary>
        /// <param name="currentPath">環境パス</param>
        /// <param name="currentTitle">環境パス名</param>
        public void SetEnvironment(string currentPath, string currentTitle="Path")
        {
            
            // 環境変数を更新(システム環境変数)
            string path = Environment.GetEnvironmentVariable(currentTitle, EnvironmentVariableTarget.Machine);
            if (path.Contains(currentPath) == false)
            {
                Environment.SetEnvironmentVariable("Path", path + ";" + currentPath, EnvironmentVariableTarget.Machine);
            }
            // 環境変数を更新(ユーザー環境変数)
            path = Environment.GetEnvironmentVariable(currentTitle, EnvironmentVariableTarget.User);
            if (path.Contains(currentPath) == false)
            {
                Environment.SetEnvironmentVariable("Path", path + ";" + currentPath, EnvironmentVariableTarget.User);
            }
        }

    }
}
