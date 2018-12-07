Example1();

void Example1()
{
    var (x, y) = ParseInput(Example).Select(ParseCharacters).First();

    AssertEqual('C', x);
    AssertEqual('A', y);
}

private void AssertEqual(char expected, char actual)
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