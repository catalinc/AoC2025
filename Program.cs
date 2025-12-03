using System.Reflection;
using AoC2025.Solvers;

var solvers = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsSubclassOf(typeof(BaseSolver)) && !t.IsAbstract)
    .Select(t => (BaseSolver)Activator.CreateInstance(t)!)
    .OrderBy(s => s.Day)
    .ToDictionary(s => s.Day);

if (solvers.Count == 0)
{
    Console.WriteLine("No solvers found. Create a class like 'Day01' inheriting BaseSolver.");
    return;
}

// ARGS PARSING
// No args -> Run latest day
// "5" -> Run Day 5 (both parts)
// "5 1" -> Run Day 5, Part 1 only
int dayToRun = solvers.Keys.Last();
int? partToRun = null;

if (args.Length >= 1 && int.TryParse(args[0], out int d))
{
    dayToRun = d;
}

if (args.Length >= 2 && int.TryParse(args[1], out int p))
{
    partToRun = p;
}

if (solvers.TryGetValue(dayToRun, out var solver))
{
    solver.Solve(partToRun);
}
else
{
    Console.WriteLine($"Solver for Day {dayToRun} not found.");
}