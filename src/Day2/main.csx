using System;

Example1();
Part1();
Example2();
Part2();

private void Example1()
{
    var input = new[]{
    "abcdef",
    "bababc",
    "abbcde",
    "abcccd",
    "aabcdd",
    "abcdee",
    "ababab"
    };

    var boxIds = input.Select(x => new BoxId(x)).ToArray();

    var checksum = ProduceChecksum(boxIds);

    if (checksum != 12)
        throw new ApplicationException($"Expected checksum of 12, but got {checksum}");
}

private void Part1()
{
    var boxIds = GetBoxIds();

    Console.WriteLine($"Checksum: {ProduceChecksum(boxIds)}");
}

private void Example2()
{
    var input = new[]{
        "abcde",
        "fghij",
        "klmno",
        "pqrst",
        "fguij",
        "axcye",
        "wvxyz",
    };

    var boxIds = input.Select(x => new BoxId(x)).ToArray();

    var commonLetters = FindCommonLettersBetweenTheTwoCorrectBoxIds(boxIds);

    if (commonLetters != "fgij")
        throw new ApplicationException($"Expected common box letters 'fgij', but got '{commonLetters}'");
}

private void Part2()
{
    var boxIds = GetBoxIds();

    Console.WriteLine($"Common letters: '{FindCommonLettersBetweenTheTwoCorrectBoxIds(boxIds)}'");
}

private int ProduceChecksum(BoxId[] boxIds) =>
    boxIds.Count(x => x.HasLetterWhichAppearsTwice()) * boxIds.Count(x => x.HasLetterWhichAppearsThrice());

private string FindCommonLettersBetweenTheTwoCorrectBoxIds(BoxId[] boxIds)
{
    for (var i = 0; i < boxIds.Length; i++)
        for (var j = i + 1; j < boxIds.Length; j++)
        {
            var box1 = boxIds[i];
            var box2 = boxIds[j];

            if (box1.TryGetCommonLetters(box2, out var commonLetters))
            {
                return commonLetters;
            }
        }

    throw new ApplicationException($"Unable to find commone letters");
}

public class BoxId
{
    private readonly string _boxId;

    public BoxId(string boxId)
    {
        _boxId = boxId;
    }

    public bool HasLetterWhichAppearsTwice()
    {
        return _boxId.ToCharArray()
            .GroupBy(x => x)
            .Any(x => x.Count() == 2);
    }

    public bool HasLetterWhichAppearsThrice()
    {
        return _boxId.ToCharArray()
            .GroupBy(x => x)
            .Any(x => x.Count() == 3);
    }

    public bool TryGetCommonLetters(BoxId other, out string commonLetters)
    {
        var temp = _boxId.ToCharArray().Zip(other._boxId.ToCharArray(), (x, y) => (x, y))
            .Where(pair => pair.x == pair.y)
            .Select(pair => pair.x)
            .Aggregate("", (word, letter) => word += letter);

        if (_boxId.Length - 1 == temp.Length)
        {
            commonLetters = temp;
            return true;
        }

        commonLetters = null;
        return false;
    }

}

private BoxId[] GetBoxIds() =>
    Input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
    .Select(x => new BoxId(x))
    .ToArray();

const string Input = @"auxwcbzrmdvpsjfgkrthnkioqm
auxwcbzrmdvpsjfgbltonyijqe
auxwcbzrmdfpsefgklthnoioqe
auxwcbzrmdvpsjfgkluhnjisqe
auxwcbzrmdvesjfgdzthnyioqe
auxwcbzrmdvhsjfgklthnmijqe
auxwcbzridvpsjfgkltxeyioqe
ayxwcbzrgdvpsjfgklthiyioqe
ajxwcbzrmdvpsjfgklkhnyiode
auxwcbcrmdvpsjfqelthnyioqe
auxwcbzrmsvpsjsgklthnyiope
auxwcbzrmqvpsjzgklghnyioqe
auxwcbzrmdvpsjtqklthxyioqe
auxwcbzrmdopsjfdklthncioqe
auxwcbzrmdvpsjfgkltmhyfoqe
aixwcbzrmdvpsjfgllthdyeoqe
vuxicbzrmdepsjfgklthnyioqe
auxwcbbxmdkpsjfgklthnyioqe
auxwcbzrgdvpsofaklthnyioqe
auxycbzrmdvpsjfgklthnyuose
aujwcbzrmdvprjfgkltcnyioqe
auxwgbzrmdvpsjfgyzthnyioqe
auxwcbzrmavpsjfgkltsnyiome
auxwcbgrmdvpsjfgkdthnrioqe
kuxwcbzrmdvpsyfgklthnyioue
auxwcbzomdvpjdfgklthnyioqe
auxwcbzrmdppsjfgklthvyifqe
aunwubzrmdvpsjrgklthnyioqe
auxwcbzrmoipsjfgklbhnyioqe
auxwdbzrmdvpsjfgmlthnyioce
auxwcbzjmsvpsjfiklthnyioqe
auxwcbzrmwcpsjfcklthnyioqe
auxwcbzfmdvprjfhklthnyioqe
auxdcbzrgdvpsjfgklthnyxoqe
wuxwbbzrmdvpsjfgklthnyiote
auowcbjrmdvpsjfgklthnyfoqe
auxwsbzrmdvpsjfglltcnyioqe
quxwcbzrmdvpkjfgklthnyioqt
vuxwcbzrudvpsjfgklthnyioqi
puxwcbzrmdvgsjfgklthncioqe
luxdcbzrmdvpsjfgkothnyioqe
auxwcbzrmdvpsjfyklthfhioqe
auxwcbqrmdvpsjfgkldhnyiote
quxwcbzrmlvpsjfgklthnyioqi
auxwcbzgmdvpsjfoklthnyiuqe
auxwcbzrmdvpsbfgkltdjyioqe
auxwcbzsmdrpsjfgklthpyioqe
auxwcbzrmfvpsjfwklthnyiote
auxbkpzrmdvpsjfgklthnyioqe
auxwcbzrddvpsjfsklthnyroqe
abxwcbzrmdvpsjfgkltdnyivqe
awxwcbzrmvvpsjfgklthngioqe
auxwcbzrmkvgsjfgkltcnyioqe
auxwcbammdvpsjfgklthpyioqe
auxwcbhrmdvpsjfgtlthnuioqe
auxwcpzrmdvpbjogklthnyioqe
auxwcbzrmdvpslfgklbhkyioqe
auxwcbsrmdvpjjfgkldhnyioqe
auxwcbzrmdqpsjfgauthnyioqe
ydxwcbxrmdvpsjfgklthnyioqe
auxwcbzrmdvpejfgklthnyyofe
auxwchzrmxvpsjfgklthnyioqh
auxwcbzrtdvpsjfgklxhnzioqe
auxwcbyrmdvpsnfgklnhnyioqe
auxwcbzrcdvpsjugklihnyioqe
auxwcbzrddvpsjfgklhhnyiaqe
aumwtbzrmdvpsjfgklthnyitqe
auxucbzrmdvpsjfgklthwfioqe
auxwcbzrmdvpzmfgkllhnyioqe
auxwcbzrmdvpsjhgklthntiome
buxwzbzrmdvpszfgklthnyioqe
ouxwcbzsgdvpsjfgklthnyioqe
auxwcbzrmdvpsjfskltgnyioqz
auxwcbbrmdvpsjftklthnyioqu
quxocbzrmdvpsjfgklthfyioqe
acxwcbzrmdvpsjfgklfhnrioqe
auxwcbzrmdnpsjfrkjthnyioqe
wuxwybzrmdwpsjfgklthnyioqe
auxwgbxrmdvpsjfghlthnyioqe
atxwcbzrmdvnsjfgklthnyjoqe
acxwcbzmmdvpsjfbklthnyioqe
auxhcbzrmdvbsjbgklthnyioqe
auxwlbzrfdvpsjfgxlthnyioqe
auxwmbzrmdfpsjqgklthnyioqe
auxwcbzrmdvpsgfgklahnyigqe
auxwgbzrmdvpsjfgzldhnyioqe
auxwcbzrmdvpydfgklthnyiohe
auxwxbzrmdvpsjfsklchnyioqe
auxqcbzrmdvpsjfgqlthnyiwqe
auxwcozrmdvssbfgklthnyioqe
auxvcczrmdvpsufgklthnyioqe
auxwcbzrudvpsjfgklyhnyioxe
aulwcbzrmdvpsjqgknthnyioqe
auukcbzrmdvpsjfgklthtyioqe
auxwcszimdvpsjfgklthnyigqe
juxwcbzrbdvpsjfgklthnyboqe
auxwcbzrmdvpjofgklthnyioqj
auxwcbzrmdvpsjfgplfhnyione
auxwcbzrmdhpsjfgkltknyeoqe
luxwcqzrmdvpsjfgklthnbioqe
uuxwcbzrmdvpsjfgkithnyiiqe
auxwcbzrmdvpdjfgkrthnyeoqe
auuwcbnrmdvpsjfgklthnjioqe
auxwcnzrmdvpsjvgklthnyooqe
auxwcbzcmdvpsjfcklthnyiose
auxwcbzrldfpsjfgklthjyioqe
auxwcizrmdvpsjfjklthnymoqe
auxwcbtrmdvpsjfgtlphnyioqe
amxwcbzrmdvksjfgklthnyiove
auxwcbzrmdvpszfgkpthnyiuqe
auxwcbzrmdvxdjfgkltqnyioqe
auxwcbzrudvpsjfgklthnymiqe
auxwcbirmdvfsjfgklmhnyioqe
auwwcbzrndvprjfgklthnyioqe
auxwcbormdgpsjfgklbhnyioqe
auxwabzrmdupsjfgklthnyioqt
auxvcbzrmdvpsjfgkltrmyioqe
auxwcbzrmddpsjfsklthnyizqe
auxwcczrmuvpyjfgklthnyioqe
auxwcczrmdvpsnfgkpthnyioqe
auxkcbzrmdvpsjfhklihnyioqe
auxwcbzrmdvpsjfgklthnkijje
auxwcbzcmdvpsjpgkldhnyioqe
auxwcnzrudvpstfgklthnyioqe
xuxwcbzrgdvusjfgklthnyioqe
aaxwcbzrmdvpsjvgklthnyidqe
auxwcbztmdvpsjfgklthnyhqqe
auxwcbzrmfvpsjfgklthnyilfe
auxwcbzrmdvksjfgklthjyioqq
auxwcbzrmdzksjfgktthnyioqe
auxwcbzrmfvpszfgklohnyioqe
auxwckzamdvpsjfgklthnyioqs
auxwcmzrhdvpsjfaklthnyioqe
fuxwcbzrmdapsjfgklrhnyioqe
avxwxbzrmdvpsjfgklthniioqe
auxwubzrmevpsjfgkltpnyioqe
fuxwcbzrgdvpsjfgklhhnyioqe
duxwwbdrmdvpsjfgklthnyioqe
audwcbzrmdvpnjcgklthnyioqe
auxtcbzrmdvpsjmgklthnyyoqe
aucwcbwrmdepsjfgklthnyioqe
auxwcbzrudvpsjfpklthnyiose
auxwcbzridvpsjfsklthxyioqe
auxtcbzrmdvpscfgklyhnyioqe
auxwcbzrmdvppjfgklthnyivee
auxwdbzrmuvpskfgklthnyioqe
auxwubzrmdvosjfgklthnyiope
auxwcbzrmhnpsjfgklthnyimqe
auxwcbzrmdqpwjfgkltpnyioqe
auxwcbormdvpsjljklthnyioqe
auxwcbzrmdjpsjfgkltjpyioqe
auxwcbzrmdvpszfgklthkyizqe
auxwcbzighvpsjfgklthnyioqe
auxwcbzrmdlpsjfgcythnyioqe
auxwcbzumdvpsjflklthnyimqe
pdxwcbzrmdvpsjfgklthnyihqe
auxwcbzrsdvpsjfgklhhvyioqe
auxwcfzamdvpsjfgkmthnyioqe
aexwcdzrmdvpsjogklthnyioqe
auxxcbkrmavpsjfgklthnyioqe
auxwcbzredvssjfgklthryioqe
aupwqbzrmdvpsjfgklthnyioqc
auxwcbzrmdvpkcagklthnyioqe
auxwcbzrmdvwsbfgklthnlioqe
aunwcbzxmdvhsjfgklthnyioqe
auxwcbzrhddpsjfgklthnnioqe
ouxwcbzrmdvtsifgklthnyioqe
auxwcbzrmdqpsjfgklthnyfoqp
auxwrbzrhdvpsjfgolthnyioqe
auxwcbcqmdvpsjugklthnyioqe
auxwcbzrqdvpsjhgklthnjioqe
auxmcbzrmdvpsjfgmlthnyjoqe
auxwcbzrmdvpsjfgzlthnycoqv
auswcbzrmdvpsffgslthnyioqe
auxwcbzrfdvpsjfrmlthnyioqe
auxwcbzrmdvpsjngzlthnxioqe
auxwcbzrmdvpsjfuqlthnyiyqe
auxwzbzrrdvosjfgklthnyioqe
auxwcbzdmdvpsjfikxthnyioqe
guxwcbzrmdvpsjfgmlthnytoqe
auxwcbzrmdvpspfgkytenyioqe
auxvcbzrldvpsjfgklthnyhoqe
auxwcbzrmavpckfgklthnyioqe
autwcbzrmdvpsafgklthnyirqe
auxwcbzrxuvpsjfgklthmyioqe
auxwcbarmdppsjfgklthnywoqe
anxvcbzrmdvpsjfgklthnyijqe
auxwcbwrmdapsjngklthnyioqe
abxwcbzrmdvpsjugkltgnyioqe
auxwcbtrmdvpsjfgkltunyioue
aujwcbzrmovpsjfgklthryioqe
auxwcbzrydvpsjfgklthndikqe
auxwcbzrmdvpsjfgklmrnyioqo
auxwcbzrddvpsjfggithnyioqe
auxwcbzrmdvpfjfaklthlyioqe
fuxtcbzrmdvpsjfgklwhnyioqe
tuxwcbzrjdvpsjfgjlthnyioqe
auxwcbzrmdppsofgklthnyfoqe
auxvclzamdvpsjfgklthnyioqe
auxwcbzrmdvpsjfdklhhnzioqe
auxwcbzrmsvpsvdgklthnyioqe
arxfcbzrmdvpsvfgklthnyioqe
auxzcbzrmdvpsjfgklthnhioqj
auxwcbzrrdvpsjfgpltunyioqe
auxuibzrmdvpwjfgklthnyioqe
auxwcbzrwdqpsjfgklthnyooqe
aujwcbzrmdvpsjvgklthxyioqe
abxwcbzrmfvpsjfgklthnyxoqe
aurwcbzrmdvpshfgklthnyhoqe
auxwcbzjmdvpsjfgknthnycoqe
auxwcbzrmdvpsjfgklmhxwioqe
auxwcbzrmfvpsjfgklthnyiorq
auxwcbormdvpsjfgklwhnlioqe
auxwctzrmdvpsjfgklcknyioqe
awxwcbzrmdvpsjfgvlthnyiome
auxwcbzrmdvpsjfjklthnyixje
auxwcsxrmdvpsjfgkltsnyioqe
auxbmbzrmdvpsjfgklthnyioce
auxwcbzrmdvpsjfukzthnytoqe
aixwcbzrmdvpsjfgllthdyioqe
auxwcbzrmdypsjfgklthnlioqy
auxccbzrmdvpsjfgkltrnnioqe
auxwcznrmdvpsjfgklthnykoqe
auxwmqzrmdvpsjfgilthnyioqe
auxwcbzrmdvpdyfgolthnyioqe
auxwcbzrmdvpsjfgkmohnqioqe
auxwcfzrmzvpsjfoklthnyioqe
auxwjyzrmdvpsjfgulthnyioqe
auxwcgzredvpsjfgkxthnyioqe
wuxwcbtrmdvpsjfgklthnyiofe
auxwcbzrmdopsgfgklthncioqe
auxmcbzjmdvpsjfgklbhnyioqe
auxwlbzrmdvpsjffklthgyioqe
auxwcbzrmrvpsjfgqlthtyioqe
kuxwhbzrmdvpsjfgklthgyioqe
auxwcozrmdgpsjfgklthnydoqe
auxwdbzrmdvpdjfgklthgyioqe
auxwqbzrmdapsvfgklthnyioqe
auqwcbzridvjsjfgklthnyioqe
auxwckzrmdvpsjfoklthnyuoqe
auxwcbzvmdvpsjfgklghnyiome
auxtcbzrmdvpsjqgktthnyioqe
auxwcbzrmdvesjfgkljhnnioqe
auxwcbzrmpvpsqfgklthnqioqe
auxwcbzrmdcpsqfgklthnzioqe
yuxwcbzrmdvpsjggklthnlioqe
auxwcbzradvpsjftklthoyioqe
auxwcbzrmdvjujfgklmhnyioqe
auxwcbzrmdvpsrfgklpinyioqe
auxwobzrvqvpsjfgklthnyioqe";