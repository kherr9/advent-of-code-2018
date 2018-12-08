Example1();
Part1();

void Example1()
{
    var path = FindPath(Example);
    AssertEqual("CABDFE", path);
}

void Part1()
{
    var path = FindPath(Input);
    AssertEqual("GKPTSLUXBIJMNCADFOVHEWYQRZ", path);
}

private string FindPath(string input)
{
    var pairs = ParseInput(input)
        .Select(ParseCharacters);

    var steps = pairs.GroupBy(tup => tup.Item2)
        .ToDictionary(grp => grp.Key, grp => grp.Select(tup => tup.Item1).ToList());

    var letters = pairs.SelectMany(tup => new[] { tup.Item1, tup.Item2 }).Distinct();

    foreach (var letter in letters)
    {
        if (!steps.ContainsKey(letter))
        {
            steps.Add(letter, new List<char>());
        }
    }

    var msg = "";
    while (steps.Any())
    {
        var step = steps.Where(x => !x.Value.Any())
            .OrderBy(x => x.Key)
            .First()
            .Key;

        msg += step;

        foreach (var thing in steps)
        {
            thing.Value.Remove(step);
        }

        steps.Remove(step);
    }

    return msg;
}

private void AssertEqual(char expected, char actual)
{
    if (expected != actual)
        throw new Exception($"Expected {expected}, but got {actual}");

}
private void AssertEqual(string expected, string actual)
{
    if (expected != actual)
        throw new Exception($"Expected {expected}, but got {actual}");
}

private static (char, char) ParseCharacters(string line)
{
    var x = line.Substring(5, 1);
    var y = line.Substring(36, 1);

    return (x.ToCharArray()[0], y.ToCharArray()[0]);
}

private string[] ParseInput(string input)
{
    return input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
}

public static string Example = @"Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.";

public static string Input = @"Step T must be finished before step X can begin.
Step G must be finished before step O can begin.
Step X must be finished before step B can begin.
Step I must be finished before step W can begin.
Step N must be finished before step V can begin.
Step K must be finished before step H can begin.
Step S must be finished before step R can begin.
Step P must be finished before step J can begin.
Step L must be finished before step V can begin.
Step D must be finished before step E can begin.
Step J must be finished before step R can begin.
Step U must be finished before step W can begin.
Step M must be finished before step Q can begin.
Step B must be finished before step F can begin.
Step F must be finished before step E can begin.
Step V must be finished before step Q can begin.
Step C must be finished before step A can begin.
Step H must be finished before step Z can begin.
Step A must be finished before step Y can begin.
Step O must be finished before step Y can begin.
Step W must be finished before step Q can begin.
Step E must be finished before step Y can begin.
Step Y must be finished before step Z can begin.
Step Q must be finished before step R can begin.
Step R must be finished before step Z can begin.
Step S must be finished before step E can begin.
Step O must be finished before step W can begin.
Step G must be finished before step B can begin.
Step I must be finished before step N can begin.
Step G must be finished before step I can begin.
Step H must be finished before step R can begin.
Step N must be finished before step C can begin.
Step M must be finished before step W can begin.
Step Y must be finished before step R can begin.
Step T must be finished before step B can begin.
Step G must be finished before step D can begin.
Step J must be finished before step O can begin.
Step I must be finished before step A can begin.
Step J must be finished before step H can begin.
Step T must be finished before step Y can begin.
Step N must be finished before step H can begin.
Step B must be finished before step V can begin.
Step M must be finished before step R can begin.
Step Y must be finished before step Q can begin.
Step X must be finished before step J can begin.
Step A must be finished before step E can begin.
Step P must be finished before step Z can begin.
Step P must be finished before step C can begin.
Step N must be finished before step Q can begin.
Step A must be finished before step O can begin.
Step G must be finished before step X can begin.
Step P must be finished before step U can begin.
Step T must be finished before step S can begin.
Step I must be finished before step V can begin.
Step V must be finished before step H can begin.
Step U must be finished before step F can begin.
Step D must be finished before step Q can begin.
Step D must be finished before step O can begin.
Step G must be finished before step H can begin.
Step I must be finished before step Z can begin.
Step N must be finished before step D can begin.
Step B must be finished before step Y can begin.
Step J must be finished before step M can begin.
Step V must be finished before step Y can begin.
Step W must be finished before step Y can begin.
Step E must be finished before step Z can begin.
Step T must be finished before step N can begin.
Step L must be finished before step U can begin.
Step S must be finished before step A can begin.
Step Q must be finished before step Z can begin.
Step T must be finished before step F can begin.
Step F must be finished before step Z can begin.
Step J must be finished before step C can begin.
Step X must be finished before step Y can begin.
Step K must be finished before step V can begin.
Step T must be finished before step I can begin.
Step I must be finished before step O can begin.
Step C must be finished before step W can begin.
Step B must be finished before step Q can begin.
Step W must be finished before step Z can begin.
Step D must be finished before step H can begin.
Step K must be finished before step A can begin.
Step M must be finished before step E can begin.
Step T must be finished before step U can begin.
Step I must be finished before step J can begin.
Step O must be finished before step Q can begin.
Step M must be finished before step Z can begin.
Step U must be finished before step C can begin.
Step N must be finished before step F can begin.
Step C must be finished before step H can begin.
Step X must be finished before step E can begin.
Step F must be finished before step O can begin.
Step P must be finished before step O can begin.
Step J must be finished before step A can begin.
Step H must be finished before step Y can begin.
Step A must be finished before step Q can begin.
Step V must be finished before step Z can begin.
Step S must be finished before step L can begin.
Step H must be finished before step E can begin.
Step X must be finished before step I can begin.
Step O must be finished before step R can begin.";