using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public class Counter
{

    private static Dictionary<string, Counter> _Counters = new Dictionary<string, Counter>();
    private static Dictionary<string, Stopwatch> _StopWatch = new();
    public static Counter GetCounter(string name)
    {
        if (!_Counters.ContainsKey(name))
        {
            _Counters.Add(name, new Counter());
        }
        return _Counters[name];
    }
    public static string AllCounterToStringTimeNS()
    {
        var sb = new StringBuilder();
        foreach (var name in _Counters.Keys)
        {
            sb.Append(name + "(" + _Counters[name].ToStringTimeNS() + ")\n");
        }
        return sb.ToString();
    }
    public static void AddCounter(string name, double val)
    {
        GetCounter(name).Add(val);
    }
    public static void StartTimeCounter(string name)
    {
        if (_StopWatch.ContainsKey(name))
        {
            var sw = _StopWatch[name];
            sw.Restart();
        }
        else
        {
            var sw = Stopwatch.StartNew();
            _StopWatch.Add(name, sw);
        }
    }
    public static void EndTimeCounter(string name)
    {
        AddCounter(name, _StopWatch[name].ElapsedMilliseconds * 1000000.0);
    }



    private double min, max;
    private long count;

    public void Add(double val)
    {
        count++;
        if (val < min) min = val;
        if (val > max) max = val;
    }

    public double GetMin()
    {
        return min;
    }

    public double GetMax()
    {
        return max;
    }

    public double GetMiddle()
    {
        return (max - min) / count;
    }

    public string ToStringTimeNS()
    {
        return "Min:" + TimeToHuman(GetMin()) + ", Middle:" + TimeToHuman(GetMiddle()) + ",  Max:" + TimeToHuman(GetMax());
    }

    public static string TimeToHuman(double ns)
    {
        if (ns < 1000)
        {
            return string.Format("{0:f3}ns", ns);
        }
        if (ns < 1000000)
        {
            return string.Format("{0:f3}mk", ns / 1000);
        }
        if (ns < 1000000000)
        {
            return string.Format("{0:f3}ms", ns / 1000000);
        }
        return string.Format("{0:f3}sec", ns / 1000000000);
    }
}
