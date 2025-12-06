namespace AoC2025.Solvers;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a single vertical column from the parsed text grid.
/// </summary>
/// <param name="Values">The collection of numeric values found in the column.</param>
/// <param name="Operation">The operator string found at the bottom of the column.</param>
public record ColumnData(IReadOnlyList<int> Values, string Operation);

/// <summary>
/// Provides functionality to parse formatted text grids into structured column data.
/// </summary>
public static class ColumnParser
{
    /// <summary>
    /// Parses a whitespace-delimited text block where columns represent data sets and the last row represents an operation.
    /// </summary>
    /// <param name="input">The raw string input containing rows and columns.</param>
    /// <returns>A list of ColumnData objects representing the vertical columns.</returns>
    /// <exception cref="ArgumentException">Thrown if input is null or empty.</exception>
    /// <exception cref="FormatException">Thrown if rows have uneven lengths or non-numeric values are found in data rows.</exception>
    public static List<ColumnData> Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));

        // 1. Normalize Input: Split into lines
        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // 2. Tokenize: Create a grid (List of string arrays)
        var grid = lines
            .Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            .ToList();

        // 3. Validate Dimensions
        int rowCount = grid.Count;
        if (rowCount == 0) return new List<ColumnData>();

        int colCount = grid[0].Length;
        if (grid.Any(row => row.Length != colCount))
        {
            throw new FormatException("Input text is jagged; all rows must have the same number of columns.");
        }

        // 4. Transpose & Build
        var results = new List<ColumnData>(colCount);

        for (int c = 0; c < colCount; c++)
        {
            var numbers = new List<int>();
            string op = string.Empty;

            for (int r = 0; r < rowCount; r++)
            {
                string token = grid[r][c];

                // Check if we are at the last row (the operation row)
                if (r == rowCount - 1)
                {
                    op = token;
                }
                else
                {
                    if (int.TryParse(token, out int val))
                    {
                        numbers.Add(val);
                    }
                    else
                    {
                        throw new FormatException(
                            $"Parsing failed. '{token}' is not a valid integer at Row {r + 1}, Column {c + 1}.");
                    }
                }
            }

            // Use AsReadOnly() to ensure immutability in the record
            results.Add(new ColumnData(numbers.AsReadOnly(), op));
        }

        return results;
    }
}