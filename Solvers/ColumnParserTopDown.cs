using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Represents a fully parsed column, containing the list of numbers constructed from it
/// and its corresponding operator.
/// </summary>
public record ParsedColumn(List<long> Numbers, char Operator);

public static class ColumnParserTopDown
{
    /// <summary>
    /// Parses a multi-line string format where columns are defined by character position.
    /// </summary>
    /// <remarks>
    /// This parser uses the last line of the input to identify columns. The position of
    /// non-space characters in the operator line defines the start of each column.
    /// It then applies a "digit pivot" logic to the fields in that column to construct numbers.
    /// For example, given fields [" 51", "387", "215"], it will produce [32, 581, 175].
    /// </remarks>
    /// <param name="input">The string input containing the number and operator lines.</param>
    /// <returns>A list of ParsedColumn objects, one for each column found in the input.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is null, empty, or has fewer than two lines.</exception>
    /// <exception cref="FormatException">Thrown if no operators are found or if a number cannot be parsed.</exception>
    public static List<ParsedColumn> Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new List<ParsedColumn>();
        }

        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            throw new ArgumentException("Input must contain at least one number line and one operator line.");
        }

        var numberLines = lines.Take(lines.Length - 1).ToArray();
        var operatorLine = lines.Last();

        // Find the start index of each column from the operator line.
        var columnStarts = new List<int>();
        for (int i = 0; i < operatorLine.Length; i++)
        {
            if (!char.IsWhiteSpace(operatorLine[i]))
            {
                columnStarts.Add(i);
            }
        }

        if (columnStarts.Count == 0)
        {
            throw new FormatException("No operators found in the last line.");
        }

        // Pad all lines to the same length to prevent Substring errors.
        int maxLength = lines.Select(l => l.Length).Max();
        var paddedNumberLines = numberLines.Select(l => l.PadRight(maxLength)).ToList();

        var result = new List<ParsedColumn>();
        var numberBuilder = new StringBuilder();

        // Process each identified column.
        for (int i = 0; i < columnStarts.Count; i++)
        {
            int colStart = columnStarts[i];
            int colEnd = (i + 1 < columnStarts.Count) ? columnStarts[i + 1] : maxLength;
            int colWidth = colEnd - colStart;

            // Extract all string fields for the current column.
            var fieldsInColumn = paddedNumberLines.Select(line => line.Substring(colStart, colWidth)).ToList();
            char operation = operatorLine[colStart];
            var numbersInColumn = new List<long>();

            // Iterate through each digit position (j) within the fields.
            for (int j = 0; j < colWidth; j++)
            {
                numberBuilder.Clear();
                // Build a new number string from the j-th digit of each field.
                foreach (var field in fieldsInColumn)
                {
                    if (j < field.Length && char.IsDigit(field[j]))
                    {
                        numberBuilder.Append(field[j]);
                    }
                }

                if (numberBuilder.Length > 0)
                {
                    if (!long.TryParse(numberBuilder.ToString(), out long number))
                    {
                        throw new FormatException($"Failed to parse number from constructed string '{numberBuilder}' in column {i + 1}.");
                    }
                    numbersInColumn.Add(number);
                }
            }

            result.Add(new ParsedColumn(numbersInColumn, operation));
        }

        return result;
    }
}