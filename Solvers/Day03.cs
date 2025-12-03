using System.Text;

namespace AoC2025.Solvers;

public class Day03 : BaseSolver
{
    public override object Part1()
    {
        return InputLines.Sum(GetMaxVoltageTwoBatteries);
    }

    private static int GetMaxVoltageTwoBatteries(string batteryBank)
    {
        var firstVoltage = GetDigit(batteryBank[0]);
        var firstVoltageIndex = 0;
        for (var i = 0; i < batteryBank.Length - 1; i++)
        {
            var voltage = GetDigit(batteryBank[i]);
            if (voltage <= firstVoltage) continue;
            firstVoltage = voltage;
            firstVoltageIndex = i;
        }

        var secondVoltage = GetDigit(batteryBank[firstVoltageIndex + 1]);
        for (var i = firstVoltageIndex + 2; i < batteryBank.Length; i++)
        {
            var voltage = GetDigit(batteryBank[i]);
            if (voltage > secondVoltage)
            {
                secondVoltage = voltage;
            }
        }

        return secondVoltage + 10 * firstVoltage;
    }

    private static int GetDigit(char c)
    {
        return c - '0';
    }

    public override object Part2()
    {
        return InputLines.Sum(GetMaxVoltageTwelveBatteries);
    }

    private static long GetMaxVoltageTwelveBatteries(string batteryBank)
    {
        var voltages = batteryBank.Select(GetDigit).ToArray();
        return GetMax12DigitValue(voltages);
    }

    private static long GetMax12DigitValue(int[] digits)
    {
        const int requiredLength = 12;

        if (digits == null || digits.Length < requiredLength)
        {
            throw new ArgumentException("Input array must contain at least 12 digits.");
        }

        var resultBuilder = new StringBuilder();
        var currentSearchIndex = 0;

        for (var i = 0; i < requiredLength; i++)
        {
            var digitsNeededAfter = requiredLength - 1 - i;
            var maxSearchIndex = digits.Length - 1 - digitsNeededAfter;

            var maxDigit = -1;
            var maxDigitIndex = -1;

            for (var j = currentSearchIndex; j <= maxSearchIndex; j++)
            {
                if (digits[j] == 9)
                {
                    maxDigit = 9;
                    maxDigitIndex = j;
                    break;
                }

                if (digits[j] <= maxDigit) continue;
                maxDigit = digits[j];
                maxDigitIndex = j;
            }

            resultBuilder.Append(maxDigit);

            currentSearchIndex = maxDigitIndex + 1;
        }

        return long.Parse(resultBuilder.ToString());
    }
}