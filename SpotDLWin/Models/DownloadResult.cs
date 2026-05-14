namespace MusicDLWin.Models
{
    /// <summary>
    /// ダウンロード結果を表すデータです。
    /// </summary>
    public class DownloadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string OutputPath { get; set; } = string.Empty;
    }
}
