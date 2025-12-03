namespace AoC2025.Solvers;

public class Day01 : BaseSolver
{
    public override object Part1()
    {
        var dial = 50;
        var password = 0;
        foreach (var inputLine in InputLines)
        {
            (dial, _) = RotateDial(dial, inputLine);
            if (dial == 0) password++;
        }
        return password;
    }

    private static (int dial, int zeros) RotateDial(int dial, string rotationSpec, bool countZeros = false)
    {
        var zeros = 0;
        var rotations = int.Parse(rotationSpec[1..]);
        var direction = rotationSpec[0];
        for (var i = 1; i <= rotations; i++)
        {
            switch (direction)
            {
                case 'R':
                    dial += 1;
                    if (dial >= 100)
                    {
                        dial -= 100;
                    }
                    break;
                case 'L':
                    dial -= 1;
                    if (dial < 0)
                    {
                        dial += 100;
                    }
                    break;
                default:
                    Console.WriteLine($"Invalid direction {direction} ({rotationSpec})");
                    break;
            }
            if (countZeros && dial == 0) zeros++;
        }
        return (dial, zeros);
    }

    public override object Part2()
    {
        var dial = 50;
        var password = 0;
        foreach (var inputLine in InputLines)
        {
            var zeros = 0;
            (dial, zeros) = RotateDial(dial, inputLine, true);
            password += zeros;
        }
        return password;
    }
}