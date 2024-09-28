using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public partial class PointMap
{
    private abstract class GenerateBase
    {
        protected Chunk chunk;
        protected IGenerateCallback callback;

        protected GenerateBase(Chunk chunk, IGenerateCallback callback)
        {
            this.chunk = chunk;
            this.callback = callback;
        }

        /**
         *  return true if Task is not void
        */
        public abstract bool Execute();
    }

    private class GeneratePoints : GenerateBase
    {
        public GeneratePoints(Chunk chunk, IGenerateCallback callback) : base(chunk, callback)
        {
        }

        public override bool Execute()
        {
            return chunk.GeneratePoints(callback);
        }
    }

    private class GenerateNVPCIndex : GenerateBase
    {
        private PointMap map;
        private CIndex chunkB;

        public GenerateNVPCIndex(PointMap map, Chunk chunkA, CIndex chunkB, IGenerateCallback callback) : base(chunkA, callback)
        {
            this.map = map;
            this.chunkB = chunkB;
        }

        public override bool Execute()
        {
            if (map.map.ContainsKey(chunkB))
            {
                return chunk.CalculateNotVisiblePoints(callback, map.map[chunkB]);
            }
            return false;
        }
    }

    public readonly Dictionary<CIndex, Chunk> map = new Dictionary<CIndex, Chunk>();
    private readonly LinkedList<GenerateBase> tasks = new LinkedList<GenerateBase>();
    private readonly SimpleSheduler sheduler = new SimpleSheduler(6);
    public readonly IGenerateCallback GenerateCallback;
    public readonly int round = 2;

    public PointMap(IGenerateCallback generateCallback)
    {
        GenerateCallback = generateCallback;
    }

    public CIndex GetCIndexAtPosition(Vector3 position)
    {
        int x = (int)(position.x / Chunk.SXZ);
        int z = (int)(position.z / Chunk.SXZ);
        if (position.x < 0) { x--; }
        if (position.z < 0) { z--; }
        return new CIndex(x, z);
    }

    public void CreateGenerateChunkTack(CIndex cIndex)
    {
        var chunk = GetOrCreateChunk(cIndex);

        if (!chunk.PointsIsCreated())
        {
            tasks.AddFirst(new GeneratePoints(chunk, GenerateCallback));
        }

        for (int z = chunk.cIndex.z - round; z <= cIndex.z + round; z++)
        {
            for (int x = chunk.cIndex.x - round; x <= cIndex.x + round; x++)
            {
                CIndex cindex = new CIndex(x, z);
                if (!chunk.NVPIsGenerated(cindex))
                {
                    tasks.AddLast(new GenerateNVPCIndex(this, chunk, cindex, GenerateCallback));
                }
            }
        }
    }

    public void Update()
    {
        sheduler.Start();
        while (!sheduler.IsEnd() && tasks.Count > 0)
        {
            GenerateChunk();
        }
    }

    private bool GenerateChunk()
    {
        if (tasks.Count == 0)
        {
            return false;
        }
        GenerateBase gb = tasks.First.Value;
        tasks.RemoveFirst();
        return gb.Execute();
    }

    private Chunk GetOrCreateChunk(CIndex c)
    {
        if (map.ContainsKey(c))
        {
            return map[c];
        }
        Chunk chunk = new Chunk(c, GetChunkHeight(c));
        map[c] = chunk;
        return chunk;
    }

    private int GetChunkHeight(CIndex c)
    {
        return GenerateCallback.GetChunkHeight(c.GetPosition(), c.GetSize());
    }

    public GridPoint GetPointAtPosition(Vector3 position)
    {
        /*if (!VectorInMap(position))
        {
            return null;
        }
        var cindex = GetCIndexAtPosition(position);
        Vector3Int posInChunk = new Vector3Int((int)position.x, (int)position.y, (int)position.z) - cindex.GetPosition(Chunk.SXZ, 0);
        if (position.x < 0) posInChunk.x--;
        if (position.z < 0) posInChunk.z--;
        if (!map.ContainsKey(cindex)) return null;
        return map[cindex].points.Get(posInChunk);*/

        var d = GetDataAtPosition(position);
        if (d.cindex == CIndex.NULL)
        {
            return null;
        }
        if (!map.ContainsKey(d.cindex)) return null;
        return map[d.cindex].points.Get(d.posInChunk);
    }

    public (CIndex cindex, Vector3Int posInChunk) GetDataAtPosition(Vector3 position)
    {
        if (!VectorInMap(position))
        {
            return (CIndex.NULL, Vector3Int.zero);
        }
        var cindex = GetCIndexAtPosition(position);
        Vector3Int posInChunk = new Vector3Int((int)position.x, (int)position.y, (int)position.z) - cindex.GetPosition(Chunk.SXZ, 0);
        if (position.x < 0) posInChunk.x--;
        if (position.z < 0) posInChunk.z--;
        return (cindex, posInChunk);
    }

    public bool VectorInMap(Vector3 position)
    {
        return position.y >= 0 && position.y < Chunk.GetChunkMaxHeight();
    }

    public int GetCountGenerateTask()
    {
        return tasks.Count;
    }


    public GridPoint FindFreePointAtPointMinDist(GridPoint point)
    {
        if (point == null)
        {
            return null;
        }
        GridPoint res = null;

        /*
         * try find round
        */

        int fidDist = 1;
        CIndex cindex = point.chunk.cIndex;
        float dist = float.MaxValue;

        for (int z = cindex.z - fidDist; z <= cindex.z + 1; z++)
        {
            for (int x = cindex.x - fidDist; x <= cindex.x + 1; x++)
            {
                CIndex idx = new CIndex(x, z);
                if (!map.ContainsKey(idx)) continue;
                Chunk chunk = map[idx];
                foreach (short pointIndex in chunk.indices)
                {
                    GridPoint gp = chunk.points.Get(pointIndex);
                    if (!gp.isFree) continue;
                    float distance = (point.position - gp.position).sqrMagnitude;
                    if (distance < dist)
                    {
                        dist = distance;
                        res = gp;
                    }
                }
            }
        }
        return res;
    }

    public GridPoint FindFreePointAtPointRandomDist(GridPoint point, float minDist, float maxDist)
    {
        if (point == null)
        {
            return null;
        }
        List<GridPoint> candidanes = new List<GridPoint>();
        minDist = minDist * minDist;
        maxDist = maxDist * maxDist;
        int fidDist = 1;//fix
        CIndex cindex = point.chunk.cIndex;
        for (int z = cindex.z - fidDist; z <= cindex.z + 1; z++)
        {
            for (int x = cindex.x - fidDist; x <= cindex.x + 1; x++)
            {
                CIndex idx = new CIndex(x, z);
                if (!map.ContainsKey(idx)) continue;
                Chunk chunk = map[idx];
                foreach (short pointIndex in chunk.indices)
                {
                    GridPoint gp = chunk.points.Get(pointIndex);
                    if (!gp.isFree) continue;
                    float distance = (point.position - gp.position).sqrMagnitude;
                    if (distance >= minDist && distance <= maxDist)
                    {
                        candidanes.Add(gp);
                    }
                }
            }
        }
        return candidanes.Count > 0 ? candidanes[UnityEngine.Random.Range(0, candidanes.Count)] : null;

    }

   /* public GridPoint FindFreeNearestPoint(Vector3 position)
    {
        var d = GetDataAtPosition(position);
        if (d.cindex != CIndex.NULL)
        {
            if (map.ContainsKey(d.cindex))
            {
                var chunk = map[d.cindex];
                if (GridPointIsFree(chunk, d.posInChunk)) return chunk.points.Get(d.posInChunk);
                if (GridPointIsFree(chunk, d.posInChunk)) return chunk.points.Get(d.posInChunk);
            }
        }
    }*/

    private bool GridPointIsFree(Chunk chunk, Vector3Int posInChunk)
    {
        var p = chunk.points.Get(posInChunk);
        return p != null && p.isFree;
    }
}
