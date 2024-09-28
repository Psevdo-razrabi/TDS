using System;
using UnityEngine;

public readonly struct CIndex
{
    public static readonly CIndex NULL = new CIndex(int.MaxValue, int.MinValue);

    public readonly int x;
    public readonly int z;


    public CIndex(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is CIndex index &&
               x == index.x &&
               z == index.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override string ToString()
    {
        return x + ", " + z;
    }

    public Vector2 GetSize()
    {
        return new Vector2(Chunk.SXZ, Chunk.SXZ);
    }

    public Vector2 GetPosition()
    {
        return new Vector2(x, z);
    }

    public Vector3Int GetPosition(int sxz, float y)
    {
        return new Vector3Int(x * sxz, (int)y, z * sxz);
    }

    public static bool operator ==(CIndex a, CIndex b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(CIndex a, CIndex b)
    {
        return a.x != b.x || a.z != b.z;
    }
}
