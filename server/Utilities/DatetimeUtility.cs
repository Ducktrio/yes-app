using System;

namespace Yes.Utilities;

public class DatetimeUtility
{
    public static string ToUnixTimestampString(DateTime dateTime)
    {
        DateTimeOffset dto = new(dateTime.ToUniversalTime());
        return dto.ToUnixTimeSeconds().ToString();
    }

    public static DateTime FromUnixTimestampString(string unixTimestamp)
    {
        if (long.TryParse(unixTimestamp, out long unixTime))
        {
            DateTimeOffset dto = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            return dto.UtcDateTime;
        }
        throw new ArgumentException("Invalid Unix timestamp format.");
    }
}
