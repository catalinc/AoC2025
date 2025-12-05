using System.Text.RegularExpressions;

namespace AoC2025.Solvers;

public partial class Day05 : BaseSolver
{
    public override object Part1()
    {
        var ranges = new List<IdRange>();
        var productIds = new List<long>();

        foreach (var line in InputLines)
        {
            var match = MyRegex().Match(line);
            if (match.Success)
            {
                var start = long.Parse(match.Groups[1].Value);
                var end = long.Parse(match.Groups[2].Value);
                ranges.Add(new IdRange(start, end));
            }

            var match2 = MyRegex1().Match(line);
            if (match2.Success)
            {
                var id = long.Parse(match2.Groups[1].Value);
                productIds.Add(id);
            }
        }

        return productIds.Count(id => ranges.Any(r => r.Contains(id)));
    }

    public readonly record struct IdRange(long Start, long End)
    {
        public bool Contains(long value)
        {
            return (Start <= value) && (value <= End);
        }
    }
    
    public static List<IdRange> ConsolidateOverlaps(List<IdRange> ranges)
    {
        if (ranges == null || ranges.Count <= 1)
        {
            return new List<IdRange>(ranges ?? new List<IdRange>());
        }

        var workingList = new List<IdRange>(ranges);
        bool wasMergedInPass;

        do
        {
            wasMergedInPass = false;
            for (int i = 0; i < workingList.Count; i++)
            {
                for (int j = i + 1; j < workingList.Count; j++)
                {
                    IdRange first = workingList[i];
                    IdRange second = workingList[j];

                    if (first.Start <= second.End && second.Start <= first.End)
                    {
                        long unionStart = Math.Min(first.Start, second.Start);
                        long unionEnd = Math.Max(first.End, second.End);

                        workingList[i] = new IdRange(unionStart, unionEnd);
                        workingList.RemoveAt(j);

                        wasMergedInPass = true;

                        break;
                    }
                }

                if (wasMergedInPass)
                {
                    break;
                }
            }
        } while (wasMergedInPass);

        return workingList;
    }

    public override object Part2()
    {
        var ranges = new List<IdRange>();
        foreach (var line in InputLines)
        {
            var match = MyRegex().Match(line);
            if (!match.Success) continue;
            var start = long.Parse(match.Groups[1].Value);
            var end = long.Parse(match.Groups[2].Value);
            ranges.Add(new IdRange(start, end));
        }

        var consolidatedRanges = ConsolidateOverlaps(ranges);
        return consolidatedRanges.Sum(r => r.End - r.Start + 1);
    }

    [GeneratedRegex(@"^(\d+)-(\d+)$")]
    private static partial Regex MyRegex();

    [GeneratedRegex(@"^(\d+)$")]
    private static partial Regex MyRegex1();
}