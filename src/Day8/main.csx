
Example1();

void Example1()
{
    var sum = Parse(GetInput(Example));

    AssertEqual(138, sum);
}

int Parse(int[] values) => Parse(values, 0).Item1;

(int, int) Parse(int[] values, int head)
{
    var childCount = values[head++];
    var metadataCount = values[head++];

    int sum = 0;
    for (var i = 0; i < childCount; i++)
    {
        var (childSum, newHead) = Parse(values, head);
        sum += childSum;
        head = newHead;
    }

    for (var i = 0; i < metadataCount; i++)
    {
        sum += values[head++];
    }

    return (sum, head);
}

int[] GetInput(string input) => input.Split(' ').Select(int.Parse).ToArray();

void AssertEqual(int expect, int actual)
{
    if (expect != actual)
        throw new Exception($"expected {expect}, actual {actual}");
}

const string Example = @"2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";