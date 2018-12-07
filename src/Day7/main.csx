Example1();

void Example1()
{
    var path = FindPath(Example);
    AssertEqual("CABDFE", path);
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