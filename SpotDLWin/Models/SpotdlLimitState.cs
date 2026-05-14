using System;
using System.Globalization;

namespace MusicDLWin.Models
{
    /// <summary>
    /// SpotDLの再試行可能時刻を表すデータです。
    /// </summary>
    public class SpotdlLimitState
    {
        public DateTimeOffset? LimitTime { get; set; }

        public bool HasLimit => LimitTime.HasValue;

        public bool IsActive => LimitTime.HasValue && LimitTime.Value > DateTimeOffset.Now;

        public static SpotdlLimitState Empty()
        {
            return new SpotdlLimitState();
        }

        public static SpotdlLimitState FromRetryAfterSeconds(int seconds)
        {
            return new SpotdlLimitState
            {
                LimitTime = DateTimeOffset.Now.AddSeconds(seconds)
            };
        }

        public static SpotdlLimitState FromStorage(string storedValue)
        {
            if (string.IsNullOrWhiteSpace(storedValue))
            {
                return Empty();
            }

            if (DateTimeOffset.TryParse(
                storedValue,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out DateTimeOffset parsedValue))
            {
                return new SpotdlLimitState
                {
                    LimitTime = parsedValue
                };
            }

            return Empty();
        }

        public string ToStorageValue()
        {
            if (!LimitTime.HasValue)
            {
                return string.Empty;
            }

            return LimitTime.Value.ToString("o");
        }

        public string ToDisplayText()
        {
            if (!LimitTime.HasValue)
            {
                return string.Empty;
            }

            return LimitTime.Value.LocalDateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
