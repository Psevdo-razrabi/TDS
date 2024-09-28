using System;
using System.Diagnostics;

public class SimpleSheduler
{
    public readonly int millisecondsPerFrame;
    private Stopwatch sw = new Stopwatch();

    public SimpleSheduler(int millisecondsPerFrame)
    {
        this.millisecondsPerFrame = millisecondsPerFrame;
    }

    public void Start()
    {
        sw.Restart();
    }

    public bool IsEnd()
    {
        return sw.ElapsedMilliseconds >= millisecondsPerFrame;
    }
}
