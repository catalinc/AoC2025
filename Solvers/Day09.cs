namespace AoC2025.Solvers;

using System;
using System.Collections.Generic;

public class Day09 : BaseSolver
{
    public override object Part1()
    {
        var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var pts = new List<(long x, long y)>();
        foreach (var line in lines)
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) continue;
            pts.Add((long.Parse(parts[0]), long.Parse(parts[1])));
        }

        int n = pts.Count;
        if (n < 2) return 0L;

        long best = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                long dx = Math.Abs(pts[i].x - pts[j].x) + 1;
                long dy = Math.Abs(pts[i].y - pts[j].y) + 1;
                long area = dx * dy;
                if (area > best) best = area;
            }
        }

        return best;
    }

    public override object Part2()
    {
        // Parse input
        var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var pts = new List<(int x, int y)>();
        foreach (var line in lines)
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) continue;
            pts.Add((int.Parse(parts[0]), int.Parse(parts[1])));
        }

        int n = pts.Count;
        if (n < 2) return 0L;

        // Bounds
        int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;
        for (int i = 0; i < n; i++)
        {
            var p = pts[i];
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }

        int H = maxY - minY + 1;

        // Build vertical edges (x, y1, y2) with y1 < y2
        var vEdges = new List<(int x, int y1, int y2)>(n);
        for (int i = 0; i < n; i++)
        {
            var a = pts[i];
            var b = pts[(i + 1) % n];
            if (a.x == b.x)
            {
                int y1 = Math.Min(a.y, b.y);
                int y2 = Math.Max(a.y, b.y);
                vEdges.Add((a.x, y1, y2));
            }
        }

        // For each scanline rowY (minY..maxY) compute intervals of x that are inside polygon.
        // Use horizontal scanline at y + 0.5 and standard ray-even-odd using vertical edges.
        var rowIntervals = new List<(int l, int r)>[H];
        for (int yi = 0; yi < H; yi++)
        {
            int rowY = minY + yi;
            double scanY = rowY + 0.5;
            var xs = new List<int>(vEdges.Count);

            // collect intersections (x positions) where vertical edge crosses the scanline
            for (int e = 0; e < vEdges.Count; e++)
            {
                var edge = vEdges[e];
                // include edge if scanY is in [y1, y2) to avoid double counting
                if (scanY >= edge.y1 && scanY < edge.y2)
                {
                    xs.Add(edge.x);
                }
            }

            xs.Sort();
            var intervals = new List<(int l, int r)>();
            for (int k = 0; k + 1 < xs.Count; k += 2)
            {
                int left = xs[k];
                int right = xs[k + 1];
                if (right <= left) continue;
                // Tiles with integer x in [left, right-1] have centers between left+0.5 .. right-0.5
                intervals.Add((left, right - 1)); // inclusive interval of allowed x
            }

            rowIntervals[yi] = intervals;
        }

        // Helper: check whether row yi (0-based) has an interval covering [lx, rx] inclusive.
        bool RowCovers(int yi, int lx, int rx)
        {
            var intervals = rowIntervals[yi];
            int lo = 0, hi = intervals.Count - 1;
            while (lo <= hi)
            {
                int mid = (lo + hi) >> 1;
                var it = intervals[mid];
                if (it.l > lx) hi = mid - 1;
                else if (it.r < lx) lo = mid + 1;
                else
                {
                    // found interval with it.l <= lx <= it.r, check if it.r >= rx
                    return it.r >= rx;
                }
            }
            return false;
        }

        // Map red points to zero-based inner coords relative to minX/minY
        var redMapped = new List<(int x, int y)>(n);
        for (int i = 0; i < n; i++)
        {
            redMapped.Add((pts[i].x, pts[i].y));
        }

        long best = 0;
        for (int i = 0; i < redMapped.Count; i++)
        {
            for (int j = i + 1; j < redMapped.Count; j++)
            {
                int x1 = redMapped[i].x;
                int y1 = redMapped[i].y;
                int x2 = redMapped[j].x;
                int y2 = redMapped[j].y;

                int left = Math.Min(x1, x2);
                int right = Math.Max(x1, x2);
                int top = Math.Min(y1, y2);
                int bottom = Math.Max(y1, y2);

                long area = (long)(right - left + 1) * (bottom - top + 1);
                if (area <= best) continue;

                int lx = left;
                int rx = right;
                bool ok = true;
                for (int row = top; row <= bottom; row++)
                {
                    int yi = row - minY;
                    if (yi < 0 || yi >= H) { ok = false; break; }
                    if (!RowCovers(yi, lx, rx)) { ok = false; break; }
                }

                if (ok && area > best) best = area;
            }
        }

        return best;
    }
}
