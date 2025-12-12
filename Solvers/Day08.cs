// csharp
namespace AoC2025.Solvers;

using System;
using System.Collections.Generic;
using System.Linq;

public class Day08 : BaseSolver
{
    public override object Part1()
    {
        var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var pts = new List<(long x, long y, long z)>();
        foreach (var line in lines)
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) continue;
            pts.Add((long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2])));
        }

        int n = pts.Count;
        var pairs = new List<(long dist2, int a, int b)>(n * (n - 1) / 2);
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                var dx = pts[i].x - pts[j].x;
                var dy = pts[i].y - pts[j].y;
                var dz = pts[i].z - pts[j].z;
                long d2 = dx * dx + dy * dy + dz * dz;
                pairs.Add((d2, i, j));
            }
        }

        pairs.Sort((p1, p2) =>
        {
            var c = p1.dist2.CompareTo(p2.dist2);
            if (c != 0) return c;
            c = p1.a.CompareTo(p2.a);
            if (c != 0) return c;
            return p1.b.CompareTo(p2.b);
        });

        int take = Math.Min(1000, pairs.Count);
        var uf = new UnionFind(n);
        for (int i = 0; i < take; i++)
        {
            uf.Union(pairs[i].a, pairs[i].b);
        }

        var sizes = new List<int>(n);
        var counts = new Dictionary<int, int>();
        for (int i = 0; i < n; i++)
        {
            int r = uf.Find(i);
            if (!counts.ContainsKey(r)) counts[r] = 0;
            counts[r]++;
        }
        sizes.AddRange(counts.Values);
        sizes.Sort((a, b) => b.CompareTo(a)); // descending

        long product = 1;
        for (int i = 0; i < Math.Min(3, sizes.Count); i++)
            product *= sizes[i];

        return product;
    }

    public override object Part2()
    {
        var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var pts = new List<(long x, long y, long z)>();
        foreach (var line in lines)
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) continue;
            pts.Add((long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2])));
        }

        int n = pts.Count;
        if (n < 2) return 0L;

        var pairs = new List<(long dist2, int a, int b)>(n * (n - 1) / 2);
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                var dx = pts[i].x - pts[j].x;
                var dy = pts[i].y - pts[j].y;
                var dz = pts[i].z - pts[j].z;
                long d2 = dx * dx + dy * dy + dz * dz;
                pairs.Add((d2, i, j));
            }
        }

        pairs.Sort((p1, p2) =>
        {
            var c = p1.dist2.CompareTo(p2.dist2);
            if (c != 0) return c;
            c = p1.a.CompareTo(p2.a);
            if (c != 0) return c;
            return p1.b.CompareTo(p2.b);
        });

        var uf = new UnionFind(n);
        int components = n;
        long product = 0L;

        foreach (var p in pairs)
        {
            if (uf.Union(p.a, p.b))
            {
                components--;
                if (components == 1)
                {
                    product = pts[p.a].x * pts[p.b].x;
                    break;
                }
            }
        }

        return product;
    }
}
