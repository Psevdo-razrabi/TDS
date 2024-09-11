using System.Diagnostics;

public class Time
{
    private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public static long GetMilliseconds()
    {
        return _stopwatch.ElapsedMilliseconds;
    }

    public static double GetNanoseconds()
    {
        return (double)_stopwatch.ElapsedTicks / Stopwatch.Frequency * 1_000_000_000D;
    }
}
