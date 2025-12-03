namespace AoC2025.Solvers;

public class Day02 : BaseSolver
{
    public override object Part1()
    {
        var input = InputLines[0];
        var result = 0L;

        foreach (var range in input.Split(','))
        {
            var splits = range.Split('-');
            var start = long.Parse(splits[0]);
            var end = long.Parse(splits[1]);

            for (var i = start; i <= end; i++)
            {
                var s = i.ToString();
                if (s.Length % 2 == 0)
                {
                    var mid = s.Length / 2;
                    var h = s.Substring(0, mid);
                    var t = s.Substring(mid);
                    if (h == t)
                    {
                        result += i;
                    }
                }
            }
        }
        return result;
    }
    
    public override object Part2()
    {
        var input = InputLines[0];
        var result = 0L;
        
        foreach(var range in input.Split(',')) 
        {
            var splits = range.Split('-');
            var start = long.Parse(splits[0]);
            var end = long.Parse(splits[1]);
            for (var i = start; i <= end; i++) 
            {
                var s = i.ToString();
                if (IsInvalidId(s)) 
                {
                    result += i;
                }
            }
        }
        return result;
    }
    
    private static bool IsInvalidId(string id)
    {
        var mid = id.Length/2;
        for (var i = 1; i <= mid; i++)
        {
            var pattern = id.Substring(0, i);
            var splits = id.Split(pattern, StringSplitOptions.RemoveEmptyEntries);
            if (splits.Length == 0) return true;
        }
        return false;
    }
}