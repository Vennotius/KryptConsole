public static class Extensions
{
    public static string PrettyHours(this TimeSpan duration)
    {
        return $"{Math.Floor(duration.TotalHours):00}:{duration.Minutes:00}:{duration.Seconds:00}";
    }
}
