namespace AoC2025.Solvers;

public class Day06 : BaseSolver
{
    public override object Part1()
    {
        // Call the static parser from the framework
        var columns = ColumnParser.Parse(Input);
        var total = 0L;
        foreach (var col in columns)
        {
            switch (col.Operation)
            {
                case "+":
                    total += col.Values.Sum();
                    break;
                case "*":
                {
                    var product = col.Values.Aggregate(1L, (current, value) => current * value);
                    total += product;
                    break;
                }
            }
        }
        return total;
    }

    public override object Part2()
    {
        var columns = ColumnParserTopDown.Parse(Input);
        var total = 0L;
        foreach (var col in columns)
        {
            switch (col.Operator)
            {
                case '+':
                    total += col.Numbers.Sum();
                    break;
                case '*':
                {
                    var product = col.Numbers.Aggregate(1L, (current, value) => current * value);
                    total += product;
                    break;
                }
            }
        }
        return total;
    }
}