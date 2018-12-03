using System;

Example1();
Part1();
Example2();
Part2();

public void Example1()
{
    Console.WriteLine("Example 1:");

    var claims = CreateClaimFromInput(Inputs.Example);

    AssertEqual(8, claims.Width);
    AssertEqual(8, claims.Height);
    AssertEqual(4, claims.GetOverlapSquareInches());
}

public void Part1()
{
    Console.WriteLine("Part 1:");

    var claims = CreateClaimFromInput(Inputs.Input);

    AssertEqual(101469, claims.GetOverlapSquareInches());
}

public void Example2()
{
    Console.WriteLine("Example 2:");

    var claims = CreateClaimFromInput(Inputs.Example);

    var claimWithNoOverlaps = claims.GetClaimWithNoOverlaps();

    AssertEqual(3, claimWithNoOverlaps.Id);
}

public void Part2()
{
    Console.WriteLine("Part 2:");

    var claims = CreateClaimFromInput(Inputs.Input);

    var claimWithNoOverlaps = claims.GetClaimWithNoOverlaps();

    AssertEqual(1067, claimWithNoOverlaps.Id);
}

public Claims CreateClaimFromInput(string input)
{
    var inputs = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

    var claims = inputs.Select(Claim.Parse).ToArray();

    return new Claims(claims);
}

public void AssertEqual(int expected, int actual)
{
    if (expected != actual)
    {
        throw new Exception($"Expected {expected}, but got {actual}");
    }
}

public class Claims
{
    private readonly Claim[] _claims;

    public Claims(Claim[] claims)
    {
        _claims = claims;
    }

    public int Width => _claims.Max(c => c.Right) + 1;

    public int Height => _claims.Max(c => c.Bottom) + 1;

    public int GetOverlapSquareInches()
    {
        var width = Width;
        var height = Height;

        var result = 0;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var point = new Point(x, y);

                var claimsRequired = _claims.Where(c => c.Contains(point)).Take(2);

                if (claimsRequired.Count() >= 2)
                {
                    result++;
                }
            }
        }

        return result;
    }

    public Claim GetClaimWithNoOverlaps()
    {
        foreach (var claim in _claims)
        {
            var others = _claims.Except(new[] { claim });

            if (others.All(c => !claim.Overlap(c)))
            {
                return claim;
            }
        }

        throw new Exception("Did not find fabric");
    }
}

public class Claim
{
    public int Id { get; set; }

    public Rectangle Rectangle { get; set; }

    public int Right => Rectangle.Right;

    public int Bottom => Rectangle.Bottom;

    public bool Contains(Point point) => Rectangle.Contains(point);

    public bool Overlap(Claim other) => Rectangle.IntersectsWith(other.Rectangle);

    public override string ToString() => $"#{Id} @ {Rectangle.Location}: {Rectangle.Size}";

    public static Claim Parse(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var idPart = parts[0];
        var locationPart = parts[2];
        var sizePart = parts[3];

        var id = int.Parse(idPart.Replace("#", ""));
        var location = ParseLocation(locationPart);
        var size = ParseSize(sizePart);

        var result = new Claim
        {
            Id = id,
            Rectangle = new Rectangle(location, size)
        };

        return result;

        Point ParseLocation(string value)
        {
            var xs = value
                .Replace(":", "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            return new Point(
                int.Parse(xs[0]),
                int.Parse(xs[1]));
        }

        Size ParseSize(string value)
        {
            var xs = value.Split('x', StringSplitOptions.RemoveEmptyEntries);

            return new Size(
                int.Parse(xs[0]),
                int.Parse(xs[1])
            );
        }
    }
}

public struct Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }

    public int Y { get; }

    public override string ToString() => $"{X},{Y}";
}

public struct Size
{
    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }

    public override string ToString() => $"{Width}x{Height}";
}

public struct Rectangle
{
    public Rectangle(Point upperLeft, Size size)
    {
        Location = upperLeft;
        Size = size;
    }

    public Point Location { get; }

    public Point BottomRight => new Point(Location.X + Size.Width - 1, Location.Y + Size.Height - 1);

    public Size Size { get; }

    public int Right => Location.X + Size.Width;

    public int Bottom => Location.Y + Size.Height;

    public bool Contains(Point point)
    {
        return IsBetween(point.X, Location.X, BottomRight.X)
            && IsBetween(point.Y, Location.Y, BottomRight.Y);

        bool IsBetween(int value, int start, int end)
        {
            return value >= start && value <= end;
        }
    }

    public bool IntersectsWith(Rectangle other)
    {
        if (Location.X > other.BottomRight.X || other.Location.X > BottomRight.X)
        {
            return false;
        }

        if (Location.Y > other.BottomRight.Y || other.Location.Y > BottomRight.Y)
        {
            return false;
        }

        return true;
    }
}

struct Inputs
{
    public const string Example = @"#1 @ 1,3: 4x4
#2 @ 3,1: 4x4
#3 @ 5,5: 2x2";

    public const string Input = @"#1 @ 429,177: 12x27
#2 @ 862,948: 20x11
#3 @ 783,463: 22x20
#4 @ 603,555: 29x23
#5 @ 553,529: 25x11
#6 @ 219,969: 14x20
#7 @ 873,917: 29x16
#8 @ 752,63: 27x29
#9 @ 112,119: 18x28
#10 @ 257,504: 10x15
#11 @ 807,884: 26x13
#12 @ 222,600: 14x12
#13 @ 80,113: 21x10
#14 @ 88,626: 10x26
#15 @ 500,196: 17x29
#16 @ 242,436: 16x22
#17 @ 334,714: 22x22
#18 @ 371,130: 23x10
#19 @ 46,847: 29x25
#20 @ 215,950: 29x20
#21 @ 216,427: 12x13
#22 @ 184,230: 13x25
#23 @ 828,217: 11x23
#24 @ 390,276: 27x15
#25 @ 15,381: 10x12
#26 @ 303,953: 23x20
#27 @ 928,149: 14x25
#28 @ 210,792: 11x11
#29 @ 885,694: 16x28
#30 @ 283,819: 13x15
#31 @ 214,249: 18x28
#32 @ 983,900: 12x27
#33 @ 144,886: 15x23
#34 @ 891,876: 18x19
#35 @ 739,113: 26x18
#36 @ 353,814: 15x29
#37 @ 286,548: 15x24
#38 @ 921,827: 15x29
#39 @ 452,895: 18x19
#40 @ 352,692: 19x27
#41 @ 692,931: 12x15
#42 @ 230,559: 24x25
#43 @ 386,488: 21x20
#44 @ 578,725: 22x19
#45 @ 952,16: 28x18
#46 @ 152,862: 19x16
#47 @ 825,680: 3x9
#48 @ 21,381: 13x17
#49 @ 499,635: 21x28
#50 @ 211,781: 12x27
#51 @ 480,311: 21x11
#52 @ 300,693: 28x14
#53 @ 332,358: 11x16
#54 @ 839,548: 24x13
#55 @ 269,469: 25x25
#56 @ 397,728: 18x17
#57 @ 921,636: 14x17
#58 @ 774,427: 17x16
#59 @ 484,129: 24x18
#60 @ 566,45: 28x24
#61 @ 589,573: 27x23
#62 @ 709,766: 23x24
#63 @ 126,62: 4x12
#64 @ 117,430: 11x12
#65 @ 733,745: 20x17
#66 @ 118,279: 26x10
#67 @ 693,86: 12x19
#68 @ 159,29: 24x24
#69 @ 910,467: 19x15
#70 @ 858,513: 10x29
#71 @ 433,532: 23x17
#72 @ 618,146: 28x27
#73 @ 240,594: 13x15
#74 @ 299,528: 11x22
#75 @ 416,233: 13x28
#76 @ 287,442: 17x28
#77 @ 32,671: 24x14
#78 @ 342,948: 29x22
#79 @ 934,110: 20x10
#80 @ 750,36: 15x10
#81 @ 552,927: 11x17
#82 @ 356,776: 10x16
#83 @ 463,42: 10x14
#84 @ 780,102: 16x18
#85 @ 650,384: 18x10
#86 @ 3,8: 29x12
#87 @ 367,429: 27x11
#88 @ 470,525: 10x16
#89 @ 115,777: 12x28
#90 @ 460,534: 15x15
#91 @ 199,673: 27x21
#92 @ 954,717: 13x19
#93 @ 5,682: 16x19
#94 @ 582,4: 17x13
#95 @ 405,823: 14x18
#96 @ 506,499: 11x21
#97 @ 155,815: 28x20
#98 @ 337,392: 10x21
#99 @ 877,585: 27x27
#100 @ 878,891: 11x14
#101 @ 583,405: 20x24
#102 @ 127,117: 11x16
#103 @ 246,38: 18x23
#104 @ 175,427: 10x26
#105 @ 668,229: 26x12
#106 @ 11,434: 29x22
#107 @ 553,460: 14x23
#108 @ 228,235: 22x22
#109 @ 165,299: 16x11
#110 @ 265,424: 22x10
#111 @ 461,884: 11x15
#112 @ 10,38: 17x28
#113 @ 633,562: 24x29
#114 @ 794,401: 11x17
#115 @ 435,872: 17x18
#116 @ 807,239: 11x12
#117 @ 922,65: 23x13
#118 @ 886,607: 29x17
#119 @ 1,625: 15x12
#120 @ 498,569: 24x21
#121 @ 204,831: 23x18
#122 @ 564,551: 25x12
#123 @ 131,541: 19x20
#124 @ 772,356: 20x12
#125 @ 353,902: 19x10
#126 @ 198,663: 17x21
#127 @ 449,207: 16x12
#128 @ 12,182: 16x15
#129 @ 908,20: 29x27
#130 @ 32,572: 14x20
#131 @ 383,977: 13x20
#132 @ 81,502: 28x19
#133 @ 565,767: 27x22
#134 @ 214,942: 4x8
#135 @ 464,190: 20x21
#136 @ 741,68: 19x19
#137 @ 413,67: 21x23
#138 @ 507,820: 21x12
#139 @ 360,938: 16x11
#140 @ 484,831: 15x21
#141 @ 733,931: 29x11
#142 @ 219,692: 27x25
#143 @ 636,584: 11x26
#144 @ 48,659: 21x18
#145 @ 975,850: 14x29
#146 @ 359,831: 19x23
#147 @ 800,901: 26x16
#148 @ 388,195: 25x25
#149 @ 155,320: 20x20
#150 @ 827,207: 13x25
#151 @ 11,489: 15x17
#152 @ 26,587: 11x20
#153 @ 448,477: 24x26
#154 @ 750,250: 19x23
#155 @ 912,628: 23x11
#156 @ 589,328: 18x18
#157 @ 292,335: 13x16
#158 @ 751,357: 12x28
#159 @ 233,775: 16x16
#160 @ 855,923: 22x16
#161 @ 439,517: 27x21
#162 @ 465,829: 25x21
#163 @ 435,635: 11x20
#164 @ 354,394: 10x11
#165 @ 507,928: 13x14
#166 @ 541,899: 27x22
#167 @ 702,939: 28x28
#168 @ 849,818: 14x26
#169 @ 776,252: 3x7
#170 @ 229,32: 14x28
#171 @ 397,411: 17x18
#172 @ 35,292: 26x14
#173 @ 333,174: 13x15
#174 @ 768,134: 29x19
#175 @ 46,690: 29x18
#176 @ 554,773: 28x24
#177 @ 727,704: 12x16
#178 @ 295,274: 16x13
#179 @ 160,707: 25x22
#180 @ 265,208: 18x28
#181 @ 815,26: 24x13
#182 @ 541,154: 29x29
#183 @ 130,898: 27x19
#184 @ 35,926: 24x21
#185 @ 65,84: 25x26
#186 @ 890,919: 20x17
#187 @ 762,444: 29x13
#188 @ 154,440: 17x18
#189 @ 527,514: 16x22
#190 @ 93,857: 21x14
#191 @ 352,947: 15x21
#192 @ 642,434: 17x22
#193 @ 303,547: 10x24
#194 @ 611,911: 10x17
#195 @ 8,715: 26x28
#196 @ 909,178: 29x11
#197 @ 225,621: 23x14
#198 @ 835,411: 26x18
#199 @ 315,466: 29x11
#200 @ 234,440: 12x25
#201 @ 39,773: 23x15
#202 @ 147,155: 10x14
#203 @ 186,790: 29x20
#204 @ 651,389: 12x24
#205 @ 612,930: 17x11
#206 @ 73,238: 12x21
#207 @ 458,808: 14x17
#208 @ 742,784: 8x6
#209 @ 754,878: 20x16
#210 @ 176,698: 20x15
#211 @ 831,442: 19x16
#212 @ 411,262: 24x15
#213 @ 422,948: 11x18
#214 @ 67,108: 29x27
#215 @ 713,538: 10x23
#216 @ 528,153: 26x25
#217 @ 878,238: 29x24
#218 @ 494,54: 15x17
#219 @ 878,892: 11x14
#220 @ 478,269: 23x13
#221 @ 595,908: 20x23
#222 @ 73,120: 15x27
#223 @ 766,792: 15x20
#224 @ 562,263: 21x25
#225 @ 164,790: 23x11
#226 @ 49,290: 23x13
#227 @ 413,625: 28x24
#228 @ 168,197: 24x22
#229 @ 458,886: 13x14
#230 @ 285,457: 29x14
#231 @ 972,133: 14x28
#232 @ 757,267: 16x14
#233 @ 680,958: 25x10
#234 @ 889,35: 24x11
#235 @ 835,535: 19x28
#236 @ 590,81: 5x6
#237 @ 863,589: 19x23
#238 @ 761,726: 23x16
#239 @ 837,819: 21x23
#240 @ 12,632: 18x22
#241 @ 805,922: 29x15
#242 @ 836,520: 19x28
#243 @ 89,622: 15x24
#244 @ 397,818: 10x13
#245 @ 163,337: 18x25
#246 @ 47,967: 23x29
#247 @ 595,195: 11x12
#248 @ 620,601: 11x28
#249 @ 425,264: 6x4
#250 @ 765,741: 26x28
#251 @ 475,571: 28x26
#252 @ 79,536: 12x24
#253 @ 30,420: 24x15
#254 @ 450,214: 25x26
#255 @ 932,685: 26x26
#256 @ 277,390: 27x18
#257 @ 238,264: 24x14
#258 @ 766,254: 21x16
#259 @ 852,820: 3x19
#260 @ 521,21: 23x11
#261 @ 743,821: 27x11
#262 @ 415,84: 18x28
#263 @ 506,286: 19x22
#264 @ 128,913: 14x20
#265 @ 521,152: 12x26
#266 @ 790,445: 15x16
#267 @ 113,297: 25x22
#268 @ 418,62: 28x11
#269 @ 609,594: 28x19
#270 @ 500,538: 21x19
#271 @ 153,290: 19x24
#272 @ 369,114: 18x22
#273 @ 26,710: 15x25
#274 @ 907,958: 17x21
#275 @ 119,52: 18x29
#276 @ 154,484: 12x18
#277 @ 8,364: 21x24
#278 @ 665,320: 4x7
#279 @ 212,136: 17x13
#280 @ 141,418: 22x14
#281 @ 17,479: 19x21
#282 @ 911,819: 17x19
#283 @ 505,705: 21x11
#284 @ 357,964: 16x26
#285 @ 540,502: 21x13
#286 @ 103,868: 15x24
#287 @ 945,365: 19x25
#288 @ 970,806: 20x14
#289 @ 871,887: 13x28
#290 @ 461,909: 26x21
#291 @ 360,252: 27x23
#292 @ 670,351: 29x18
#293 @ 551,707: 12x28
#294 @ 578,839: 22x26
#295 @ 876,955: 29x29
#296 @ 571,140: 15x21
#297 @ 634,801: 23x17
#298 @ 489,415: 23x20
#299 @ 396,962: 21x15
#300 @ 722,128: 29x25
#301 @ 169,707: 20x13
#302 @ 584,176: 19x13
#303 @ 738,602: 19x13
#304 @ 48,756: 15x20
#305 @ 334,360: 16x18
#306 @ 421,613: 18x27
#307 @ 775,414: 21x21
#308 @ 680,166: 18x23
#309 @ 984,884: 7x5
#310 @ 241,967: 11x18
#311 @ 308,398: 28x17
#312 @ 236,984: 11x10
#313 @ 108,24: 26x14
#314 @ 894,1: 12x16
#315 @ 544,773: 18x18
#316 @ 103,754: 15x22
#317 @ 563,139: 14x13
#318 @ 394,878: 15x12
#319 @ 744,370: 15x28
#320 @ 463,874: 10x20
#321 @ 359,308: 15x17
#322 @ 877,771: 19x20
#323 @ 902,875: 13x21
#324 @ 272,267: 19x12
#325 @ 543,527: 17x21
#326 @ 5,650: 25x18
#327 @ 584,592: 20x15
#328 @ 730,355: 21x21
#329 @ 720,889: 20x11
#330 @ 412,334: 28x23
#331 @ 311,217: 20x24
#332 @ 281,932: 12x19
#333 @ 58,53: 16x20
#334 @ 545,557: 27x23
#335 @ 868,585: 20x10
#336 @ 748,782: 16x24
#337 @ 411,841: 23x21
#338 @ 240,12: 20x27
#339 @ 793,7: 14x22
#340 @ 215,115: 14x17
#341 @ 318,178: 15x12
#342 @ 695,31: 16x26
#343 @ 735,770: 19x28
#344 @ 777,786: 22x16
#345 @ 531,501: 20x17
#346 @ 569,1: 20x27
#347 @ 424,146: 20x18
#348 @ 530,652: 28x17
#349 @ 470,857: 29x16
#350 @ 590,43: 20x26
#351 @ 909,890: 15x18
#352 @ 406,23: 29x20
#353 @ 254,506: 27x18
#354 @ 231,172: 19x24
#355 @ 651,567: 3x10
#356 @ 258,313: 19x15
#357 @ 920,169: 12x23
#358 @ 707,474: 24x11
#359 @ 810,854: 20x15
#360 @ 428,661: 29x11
#361 @ 115,433: 13x24
#362 @ 977,181: 15x25
#363 @ 747,125: 26x23
#364 @ 635,286: 22x29
#365 @ 788,246: 10x17
#366 @ 275,258: 13x12
#367 @ 635,362: 16x21
#368 @ 447,903: 26x20
#369 @ 939,559: 27x20
#370 @ 929,550: 23x28
#371 @ 764,722: 28x14
#372 @ 884,764: 24x21
#373 @ 290,153: 28x20
#374 @ 868,560: 28x11
#375 @ 759,712: 18x12
#376 @ 654,468: 26x26
#377 @ 390,425: 17x28
#378 @ 678,224: 11x10
#379 @ 937,226: 13x22
#380 @ 533,313: 14x21
#381 @ 239,695: 26x26
#382 @ 632,160: 21x13
#383 @ 727,271: 15x19
#384 @ 8,532: 18x20
#385 @ 232,978: 29x15
#386 @ 431,371: 21x11
#387 @ 875,299: 22x19
#388 @ 479,678: 17x26
#389 @ 108,951: 19x23
#390 @ 735,112: 20x18
#391 @ 318,893: 17x20
#392 @ 194,215: 17x19
#393 @ 214,528: 28x15
#394 @ 46,142: 26x12
#395 @ 528,379: 26x25
#396 @ 479,646: 13x14
#397 @ 865,562: 17x13
#398 @ 441,142: 29x15
#399 @ 174,199: 24x17
#400 @ 561,47: 21x24
#401 @ 629,284: 17x22
#402 @ 19,654: 20x16
#403 @ 101,568: 11x23
#404 @ 641,262: 13x26
#405 @ 709,960: 22x17
#406 @ 939,522: 13x5
#407 @ 317,400: 8x10
#408 @ 829,239: 23x21
#409 @ 288,438: 28x20
#410 @ 452,405: 16x20
#411 @ 718,197: 24x13
#412 @ 607,956: 11x18
#413 @ 777,65: 28x19
#414 @ 548,933: 17x28
#415 @ 255,696: 29x23
#416 @ 245,954: 24x23
#417 @ 942,688: 27x14
#418 @ 671,254: 19x29
#419 @ 152,554: 22x25
#420 @ 312,457: 14x14
#421 @ 202,959: 27x22
#422 @ 622,27: 26x19
#423 @ 957,700: 13x18
#424 @ 555,477: 26x13
#425 @ 652,652: 15x10
#426 @ 89,165: 26x26
#427 @ 23,906: 17x25
#428 @ 648,157: 13x19
#429 @ 789,16: 22x14
#430 @ 56,288: 27x15
#431 @ 855,805: 24x22
#432 @ 139,316: 25x18
#433 @ 715,539: 21x14
#434 @ 203,689: 21x22
#435 @ 244,334: 16x16
#436 @ 436,626: 21x28
#437 @ 809,228: 17x14
#438 @ 900,244: 28x15
#439 @ 945,13: 12x22
#440 @ 817,479: 13x5
#441 @ 869,440: 8x4
#442 @ 528,730: 19x20
#443 @ 9,707: 24x17
#444 @ 44,70: 27x29
#445 @ 577,461: 10x12
#446 @ 31,742: 11x16
#447 @ 717,227: 21x18
#448 @ 129,130: 10x18
#449 @ 87,929: 17x11
#450 @ 807,849: 20x26
#451 @ 583,586: 11x23
#452 @ 483,150: 22x21
#453 @ 223,731: 13x14
#454 @ 402,834: 13x25
#455 @ 131,782: 18x27
#456 @ 21,693: 10x10
#457 @ 672,226: 20x17
#458 @ 490,953: 21x17
#459 @ 374,924: 17x4
#460 @ 660,488: 20x11
#461 @ 155,16: 22x13
#462 @ 135,829: 22x16
#463 @ 593,620: 20x13
#464 @ 142,9: 29x14
#465 @ 493,708: 16x14
#466 @ 541,636: 25x19
#467 @ 342,179: 25x18
#468 @ 499,563: 15x22
#469 @ 643,368: 20x17
#470 @ 7,679: 28x14
#471 @ 77,865: 17x28
#472 @ 875,800: 26x28
#473 @ 422,862: 26x18
#474 @ 972,880: 25x14
#475 @ 892,963: 24x13
#476 @ 448,536: 14x10
#477 @ 210,940: 13x13
#478 @ 361,197: 27x16
#479 @ 787,417: 17x28
#480 @ 976,467: 17x15
#481 @ 592,316: 18x21
#482 @ 589,931: 24x12
#483 @ 361,207: 20x20
#484 @ 831,802: 19x29
#485 @ 466,399: 10x28
#486 @ 381,971: 15x11
#487 @ 552,629: 12x10
#488 @ 172,713: 10x28
#489 @ 849,106: 16x20
#490 @ 35,660: 26x27
#491 @ 37,762: 18x25
#492 @ 174,926: 29x28
#493 @ 113,323: 26x14
#494 @ 586,78: 19x16
#495 @ 539,313: 13x12
#496 @ 685,97: 22x13
#497 @ 10,515: 29x20
#498 @ 90,852: 16x18
#499 @ 496,868: 12x26
#500 @ 141,8: 25x13
#501 @ 705,219: 13x14
#502 @ 168,181: 25x17
#503 @ 162,16: 15x14
#504 @ 291,943: 28x23
#505 @ 203,394: 28x16
#506 @ 243,967: 12x22
#507 @ 130,595: 15x20
#508 @ 489,803: 27x27
#509 @ 694,701: 14x16
#510 @ 461,147: 29x13
#511 @ 914,952: 26x28
#512 @ 701,192: 13x26
#513 @ 289,130: 12x29
#514 @ 590,380: 19x26
#515 @ 488,939: 24x11
#516 @ 8,469: 20x23
#517 @ 776,424: 20x10
#518 @ 850,807: 25x19
#519 @ 814,557: 24x13
#520 @ 754,695: 19x16
#521 @ 973,173: 14x11
#522 @ 548,559: 14x16
#523 @ 232,255: 19x10
#524 @ 276,904: 16x27
#525 @ 487,197: 17x22
#526 @ 80,524: 18x24
#527 @ 169,14: 27x18
#528 @ 938,190: 12x23
#529 @ 776,963: 18x21
#530 @ 698,938: 12x28
#531 @ 528,27: 28x16
#532 @ 42,186: 27x16
#533 @ 532,312: 17x16
#534 @ 113,842: 14x24
#535 @ 245,149: 15x29
#536 @ 741,690: 21x27
#537 @ 220,102: 11x16
#538 @ 753,770: 22x12
#539 @ 117,918: 22x20
#540 @ 722,533: 17x12
#541 @ 578,255: 22x15
#542 @ 505,117: 16x25
#543 @ 152,138: 21x29
#544 @ 595,704: 22x24
#545 @ 720,98: 27x10
#546 @ 754,571: 21x26
#547 @ 10,353: 24x16
#548 @ 163,11: 12x26
#549 @ 683,859: 18x27
#550 @ 771,579: 26x27
#551 @ 22,772: 21x14
#552 @ 265,388: 19x22
#553 @ 589,431: 18x11
#554 @ 348,382: 11x28
#555 @ 33,387: 20x27
#556 @ 74,417: 23x17
#557 @ 297,954: 17x19
#558 @ 426,666: 12x23
#559 @ 364,247: 16x15
#560 @ 854,944: 14x15
#561 @ 369,916: 27x16
#562 @ 738,65: 26x16
#563 @ 804,204: 19x18
#564 @ 722,483: 27x19
#565 @ 492,109: 18x12
#566 @ 564,722: 25x13
#567 @ 31,366: 14x19
#568 @ 635,271: 23x22
#569 @ 823,678: 24x12
#570 @ 320,103: 11x22
#571 @ 450,879: 18x21
#572 @ 119,705: 20x14
#573 @ 634,149: 17x22
#574 @ 770,664: 15x13
#575 @ 256,303: 16x22
#576 @ 554,180: 29x13
#577 @ 661,273: 16x27
#578 @ 361,688: 10x18
#579 @ 578,573: 10x28
#580 @ 536,710: 19x15
#581 @ 479,175: 10x24
#582 @ 313,963: 29x21
#583 @ 870,878: 13x27
#584 @ 148,99: 12x12
#585 @ 2,30: 13x17
#586 @ 490,680: 29x23
#587 @ 563,458: 14x23
#588 @ 517,933: 19x22
#589 @ 466,885: 10x16
#590 @ 955,575: 18x17
#591 @ 400,301: 5x15
#592 @ 768,208: 18x25
#593 @ 687,913: 13x27
#594 @ 282,893: 27x28
#595 @ 410,339: 12x17
#596 @ 552,764: 29x24
#597 @ 25,165: 25x29
#598 @ 126,796: 11x24
#599 @ 689,704: 17x14
#600 @ 509,956: 10x22
#601 @ 484,604: 11x27
#602 @ 551,538: 21x29
#603 @ 433,845: 29x27
#604 @ 702,699: 29x17
#605 @ 5,528: 12x20
#606 @ 849,804: 13x16
#607 @ 769,707: 17x21
#608 @ 514,557: 17x16
#609 @ 275,733: 17x11
#610 @ 608,973: 17x14
#611 @ 967,855: 14x25
#612 @ 48,653: 13x19
#613 @ 508,119: 14x23
#614 @ 957,866: 14x17
#615 @ 345,785: 28x29
#616 @ 175,141: 20x20
#617 @ 111,426: 28x27
#618 @ 854,362: 19x17
#619 @ 935,172: 10x13
#620 @ 282,498: 12x29
#621 @ 248,546: 19x23
#622 @ 75,824: 23x11
#623 @ 695,800: 13x20
#624 @ 62,147: 14x17
#625 @ 150,962: 28x27
#626 @ 574,576: 16x21
#627 @ 271,510: 19x16
#628 @ 194,615: 25x18
#629 @ 977,150: 14x18
#630 @ 300,269: 17x28
#631 @ 290,528: 11x21
#632 @ 546,387: 18x26
#633 @ 171,30: 24x10
#634 @ 476,628: 12x21
#635 @ 676,907: 19x13
#636 @ 230,605: 27x10
#637 @ 514,736: 17x29
#638 @ 5,490: 14x27
#639 @ 364,680: 10x17
#640 @ 398,251: 20x16
#641 @ 150,101: 6x5
#642 @ 109,420: 19x26
#643 @ 118,715: 27x10
#644 @ 979,723: 10x18
#645 @ 530,729: 16x20
#646 @ 724,284: 14x21
#647 @ 164,89: 26x15
#648 @ 218,833: 4x13
#649 @ 697,207: 13x13
#650 @ 482,5: 19x13
#651 @ 383,142: 15x29
#652 @ 734,287: 14x24
#653 @ 727,956: 28x22
#654 @ 588,424: 17x20
#655 @ 602,582: 22x23
#656 @ 936,520: 21x13
#657 @ 110,304: 20x19
#658 @ 368,689: 25x11
#659 @ 139,966: 25x28
#660 @ 369,289: 11x20
#661 @ 821,483: 24x21
#662 @ 690,171: 10x28
#663 @ 409,696: 20x16
#664 @ 836,557: 17x24
#665 @ 466,407: 28x27
#666 @ 244,573: 29x21
#667 @ 439,475: 14x26
#668 @ 570,337: 14x17
#669 @ 276,244: 12x15
#670 @ 157,271: 20x19
#671 @ 984,460: 13x29
#672 @ 38,109: 14x11
#673 @ 318,369: 26x27
#674 @ 706,262: 23x22
#675 @ 292,547: 11x22
#676 @ 774,750: 28x11
#677 @ 869,507: 13x24
#678 @ 214,746: 13x14
#679 @ 711,753: 22x12
#680 @ 482,861: 18x10
#681 @ 926,53: 12x25
#682 @ 282,901: 29x24
#683 @ 578,825: 29x23
#684 @ 262,555: 17x25
#685 @ 476,930: 23x10
#686 @ 928,239: 13x11
#687 @ 691,43: 28x17
#688 @ 711,194: 15x23
#689 @ 83,157: 12x16
#690 @ 59,853: 14x24
#691 @ 9,384: 10x13
#692 @ 655,814: 12x24
#693 @ 928,33: 20x25
#694 @ 271,716: 15x25
#695 @ 190,710: 19x19
#696 @ 436,851: 12x24
#697 @ 445,294: 14x26
#698 @ 544,889: 11x21
#699 @ 361,304: 24x13
#700 @ 787,333: 18x28
#701 @ 442,851: 16x14
#702 @ 542,962: 20x12
#703 @ 876,920: 20x10
#704 @ 920,65: 15x26
#705 @ 393,176: 28x23
#706 @ 528,428: 6x3
#707 @ 700,72: 11x13
#708 @ 133,350: 23x26
#709 @ 485,630: 27x11
#710 @ 174,150: 10x11
#711 @ 675,192: 29x11
#712 @ 182,190: 10x12
#713 @ 105,21: 26x25
#714 @ 957,479: 28x18
#715 @ 577,942: 13x17
#716 @ 313,824: 19x27
#717 @ 845,175: 23x16
#718 @ 198,397: 19x15
#719 @ 544,203: 24x25
#720 @ 236,626: 11x24
#721 @ 210,373: 25x26
#722 @ 441,483: 25x14
#723 @ 154,850: 20x12
#724 @ 50,165: 17x28
#725 @ 284,267: 10x21
#726 @ 803,395: 10x24
#727 @ 742,793: 14x27
#728 @ 951,576: 11x23
#729 @ 269,434: 24x11
#730 @ 160,658: 15x18
#731 @ 239,866: 26x27
#732 @ 325,187: 25x16
#733 @ 649,547: 12x20
#734 @ 646,438: 11x21
#735 @ 199,127: 24x17
#736 @ 178,322: 13x29
#737 @ 777,18: 16x23
#738 @ 72,121: 14x21
#739 @ 204,718: 29x13
#740 @ 750,887: 12x25
#741 @ 884,804: 10x18
#742 @ 642,618: 10x20
#743 @ 734,667: 14x14
#744 @ 262,670: 26x23
#745 @ 218,599: 21x21
#746 @ 726,266: 13x25
#747 @ 742,956: 25x20
#748 @ 147,110: 15x14
#749 @ 62,381: 12x23
#750 @ 247,628: 10x29
#751 @ 948,190: 11x26
#752 @ 284,545: 10x15
#753 @ 851,927: 13x13
#754 @ 331,568: 21x13
#755 @ 180,200: 12x13
#756 @ 979,926: 20x23
#757 @ 957,940: 21x26
#758 @ 297,833: 22x25
#759 @ 601,53: 22x15
#760 @ 223,264: 22x19
#761 @ 384,881: 21x11
#762 @ 245,966: 14x22
#763 @ 397,740: 15x16
#764 @ 579,320: 17x29
#765 @ 132,947: 19x19
#766 @ 482,927: 17x21
#767 @ 773,249: 11x15
#768 @ 366,407: 24x25
#769 @ 802,56: 13x27
#770 @ 581,370: 23x11
#771 @ 431,429: 15x28
#772 @ 130,0: 22x13
#773 @ 815,116: 19x10
#774 @ 922,617: 16x10
#775 @ 193,979: 29x13
#776 @ 817,819: 18x13
#777 @ 753,270: 28x12
#778 @ 759,709: 14x18
#779 @ 582,750: 13x14
#780 @ 387,490: 21x13
#781 @ 114,766: 17x11
#782 @ 777,260: 21x10
#783 @ 529,151: 15x19
#784 @ 686,547: 27x22
#785 @ 259,271: 27x11
#786 @ 192,271: 5x9
#787 @ 238,507: 20x13
#788 @ 129,577: 25x22
#789 @ 835,411: 28x13
#790 @ 547,549: 25x18
#791 @ 933,40: 16x15
#792 @ 815,553: 11x18
#793 @ 503,10: 18x24
#794 @ 951,25: 18x23
#795 @ 71,387: 25x27
#796 @ 405,252: 12x21
#797 @ 918,790: 27x24
#798 @ 832,851: 26x27
#799 @ 909,769: 18x15
#800 @ 864,515: 17x15
#801 @ 342,823: 17x19
#802 @ 591,976: 18x22
#803 @ 310,573: 28x18
#804 @ 101,569: 13x11
#805 @ 369,376: 24x11
#806 @ 723,731: 20x15
#807 @ 394,268: 12x21
#808 @ 154,899: 29x13
#809 @ 188,599: 27x18
#810 @ 433,171: 25x13
#811 @ 925,905: 20x24
#812 @ 633,430: 17x18
#813 @ 187,268: 16x19
#814 @ 467,886: 13x16
#815 @ 897,746: 26x13
#816 @ 308,689: 23x21
#817 @ 966,385: 15x28
#818 @ 368,293: 13x14
#819 @ 821,804: 17x29
#820 @ 890,715: 13x20
#821 @ 755,145: 23x12
#822 @ 860,545: 10x26
#823 @ 797,518: 26x25
#824 @ 625,3: 27x27
#825 @ 359,201: 17x17
#826 @ 720,614: 23x28
#827 @ 104,795: 18x24
#828 @ 885,965: 10x17
#829 @ 530,731: 25x20
#830 @ 919,264: 11x22
#831 @ 150,950: 14x24
#832 @ 785,213: 14x12
#833 @ 413,627: 15x23
#834 @ 723,689: 10x10
#835 @ 828,202: 19x23
#836 @ 241,262: 13x11
#837 @ 788,26: 18x22
#838 @ 203,172: 20x19
#839 @ 181,207: 14x15
#840 @ 915,680: 14x10
#841 @ 742,399: 22x28
#842 @ 760,95: 21x28
#843 @ 152,75: 18x23
#844 @ 47,286: 13x27
#845 @ 923,20: 17x20
#846 @ 305,940: 13x19
#847 @ 691,504: 15x11
#848 @ 73,228: 26x13
#849 @ 329,512: 18x15
#850 @ 154,85: 10x29
#851 @ 200,157: 12x27
#852 @ 209,692: 11x10
#853 @ 242,331: 14x26
#854 @ 895,118: 19x16
#855 @ 580,973: 19x23
#856 @ 663,317: 11x18
#857 @ 922,548: 26x16
#858 @ 930,784: 14x11
#859 @ 802,455: 29x27
#860 @ 888,979: 15x10
#861 @ 688,91: 14x10
#862 @ 752,823: 27x13
#863 @ 61,831: 27x11
#864 @ 345,944: 11x10
#865 @ 8,713: 13x27
#866 @ 298,199: 20x19
#867 @ 423,260: 14x15
#868 @ 41,376: 13x17
#869 @ 204,673: 8x8
#870 @ 105,288: 15x17
#871 @ 108,482: 17x28
#872 @ 603,578: 10x20
#873 @ 738,674: 12x25
#874 @ 456,563: 24x13
#875 @ 340,694: 14x11
#876 @ 163,560: 15x22
#877 @ 15,705: 23x11
#878 @ 849,445: 28x23
#879 @ 53,664: 21x29
#880 @ 256,253: 16x21
#881 @ 609,80: 18x28
#882 @ 585,93: 25x11
#883 @ 681,98: 20x19
#884 @ 318,184: 25x14
#885 @ 33,517: 16x26
#886 @ 490,281: 21x12
#887 @ 613,179: 28x16
#888 @ 25,968: 11x20
#889 @ 276,432: 17x10
#890 @ 482,907: 28x19
#891 @ 332,737: 29x14
#892 @ 58,964: 12x21
#893 @ 728,684: 19x11
#894 @ 776,660: 10x21
#895 @ 441,629: 26x15
#896 @ 467,291: 26x25
#897 @ 83,183: 14x17
#898 @ 391,832: 19x26
#899 @ 364,728: 28x28
#900 @ 23,498: 26x25
#901 @ 780,896: 21x13
#902 @ 330,176: 23x18
#903 @ 337,690: 25x16
#904 @ 334,801: 21x24
#905 @ 176,795: 16x28
#906 @ 374,929: 23x23
#907 @ 394,166: 25x21
#908 @ 633,146: 11x13
#909 @ 579,129: 12x12
#910 @ 225,283: 18x27
#911 @ 277,500: 17x21
#912 @ 396,299: 13x21
#913 @ 669,212: 17x22
#914 @ 372,731: 28x15
#915 @ 905,10: 25x24
#916 @ 697,65: 15x20
#917 @ 190,587: 26x12
#918 @ 930,54: 15x19
#919 @ 360,359: 21x18
#920 @ 15,507: 16x22
#921 @ 722,512: 12x29
#922 @ 400,868: 13x21
#923 @ 707,759: 15x22
#924 @ 57,70: 26x18
#925 @ 321,882: 26x11
#926 @ 408,743: 25x21
#927 @ 435,371: 15x15
#928 @ 181,434: 11x21
#929 @ 571,36: 25x15
#930 @ 777,545: 22x25
#931 @ 823,678: 11x16
#932 @ 7,663: 28x16
#933 @ 862,259: 29x23
#934 @ 915,964: 25x26
#935 @ 843,659: 16x21
#936 @ 887,236: 23x26
#937 @ 116,855: 25x21
#938 @ 210,609: 11x28
#939 @ 434,51: 17x25
#940 @ 912,761: 23x11
#941 @ 948,470: 26x21
#942 @ 158,887: 13x27
#943 @ 513,33: 26x23
#944 @ 832,889: 16x10
#945 @ 414,691: 24x12
#946 @ 218,773: 18x16
#947 @ 869,570: 17x18
#948 @ 598,585: 27x10
#949 @ 973,525: 10x28
#950 @ 117,475: 22x29
#951 @ 726,426: 24x13
#952 @ 854,275: 15x11
#953 @ 312,723: 24x23
#954 @ 814,477: 20x11
#955 @ 677,842: 24x18
#956 @ 880,495: 25x25
#957 @ 183,33: 20x15
#958 @ 817,108: 13x10
#959 @ 922,5: 28x13
#960 @ 322,464: 18x15
#961 @ 903,555: 29x26
#962 @ 826,210: 14x21
#963 @ 678,183: 15x23
#964 @ 339,885: 22x12
#965 @ 328,969: 29x13
#966 @ 367,721: 10x28
#967 @ 15,405: 13x22
#968 @ 961,565: 13x17
#969 @ 249,131: 22x21
#970 @ 282,272: 27x22
#971 @ 965,377: 23x11
#972 @ 373,886: 23x24
#973 @ 818,873: 17x15
#974 @ 976,358: 11x16
#975 @ 640,561: 23x20
#976 @ 210,661: 14x22
#977 @ 722,249: 22x26
#978 @ 841,461: 15x26
#979 @ 113,546: 14x23
#980 @ 474,654: 10x19
#981 @ 740,123: 14x12
#982 @ 303,662: 27x13
#983 @ 514,300: 29x29
#984 @ 692,784: 28x10
#985 @ 34,117: 21x22
#986 @ 867,432: 15x21
#987 @ 896,967: 21x16
#988 @ 554,390: 20x12
#989 @ 698,215: 28x11
#990 @ 90,625: 14x28
#991 @ 781,252: 14x11
#992 @ 106,25: 23x20
#993 @ 634,455: 12x22
#994 @ 212,316: 22x28
#995 @ 576,53: 15x18
#996 @ 721,304: 10x14
#997 @ 465,686: 28x20
#998 @ 611,178: 28x25
#999 @ 524,426: 18x10
#1000 @ 681,91: 19x18
#1001 @ 552,392: 10x16
#1002 @ 467,435: 14x17
#1003 @ 839,640: 14x24
#1004 @ 556,594: 28x19
#1005 @ 551,952: 20x26
#1006 @ 919,261: 10x21
#1007 @ 153,420: 13x10
#1008 @ 391,750: 18x19
#1009 @ 690,565: 22x17
#1010 @ 545,954: 20x12
#1011 @ 498,855: 16x28
#1012 @ 770,142: 12x22
#1013 @ 646,596: 28x28
#1014 @ 284,3: 6x10
#1015 @ 30,743: 19x19
#1016 @ 280,1: 20x23
#1017 @ 476,683: 14x19
#1018 @ 384,947: 29x11
#1019 @ 65,109: 16x12
#1020 @ 466,877: 16x28
#1021 @ 141,717: 21x22
#1022 @ 975,359: 23x10
#1023 @ 328,894: 27x10
#1024 @ 946,543: 12x14
#1025 @ 420,17: 27x25
#1026 @ 198,424: 19x18
#1027 @ 674,201: 23x19
#1028 @ 884,932: 10x24
#1029 @ 497,491: 10x25
#1030 @ 435,853: 25x13
#1031 @ 890,125: 20x12
#1032 @ 451,811: 16x21
#1033 @ 846,219: 27x25
#1034 @ 941,30: 14x21
#1035 @ 891,504: 13x29
#1036 @ 797,525: 11x25
#1037 @ 237,8: 20x19
#1038 @ 630,607: 25x19
#1039 @ 196,671: 20x14
#1040 @ 970,830: 26x12
#1041 @ 158,656: 23x26
#1042 @ 555,266: 15x24
#1043 @ 19,782: 21x29
#1044 @ 787,730: 13x10
#1045 @ 482,106: 29x24
#1046 @ 216,729: 24x19
#1047 @ 738,797: 23x11
#1048 @ 588,198: 21x26
#1049 @ 164,185: 17x26
#1050 @ 128,708: 20x17
#1051 @ 132,145: 19x28
#1052 @ 789,474: 12x11
#1053 @ 715,671: 10x12
#1054 @ 778,970: 3x9
#1055 @ 472,530: 5x5
#1056 @ 11,462: 24x25
#1057 @ 974,353: 17x13
#1058 @ 623,841: 22x25
#1059 @ 228,632: 21x16
#1060 @ 154,578: 15x14
#1061 @ 217,568: 16x16
#1062 @ 223,522: 14x14
#1063 @ 705,903: 22x20
#1064 @ 509,542: 19x13
#1065 @ 24,966: 24x21
#1066 @ 58,676: 16x27
#1067 @ 668,880: 12x24
#1068 @ 789,481: 24x24
#1069 @ 71,937: 17x16
#1070 @ 350,185: 13x29
#1071 @ 839,179: 12x11
#1072 @ 335,361: 4x9
#1073 @ 781,511: 27x12
#1074 @ 623,839: 27x24
#1075 @ 587,361: 11x18
#1076 @ 167,710: 5x14
#1077 @ 92,620: 20x27
#1078 @ 223,327: 14x26
#1079 @ 452,271: 27x17
#1080 @ 16,734: 14x23
#1081 @ 586,165: 28x19
#1082 @ 190,260: 27x24
#1083 @ 321,709: 28x16
#1084 @ 966,727: 14x28
#1085 @ 410,739: 29x12
#1086 @ 552,552: 17x10
#1087 @ 128,327: 3x4
#1088 @ 253,628: 15x29
#1089 @ 406,833: 19x19
#1090 @ 230,19: 14x19
#1091 @ 740,94: 27x29
#1092 @ 854,346: 10x27
#1093 @ 431,280: 28x22
#1094 @ 276,242: 10x29
#1095 @ 177,232: 14x19
#1096 @ 789,243: 15x12
#1097 @ 636,554: 26x11
#1098 @ 308,642: 12x23
#1099 @ 146,497: 23x16
#1100 @ 38,725: 5x4
#1101 @ 533,715: 14x27
#1102 @ 290,660: 27x16
#1103 @ 516,636: 18x10
#1104 @ 249,653: 12x17
#1105 @ 661,203: 23x18
#1106 @ 582,610: 16x18
#1107 @ 4,687: 22x11
#1108 @ 419,855: 28x23
#1109 @ 970,537: 21x13
#1110 @ 262,498: 12x29
#1111 @ 271,815: 19x21
#1112 @ 928,86: 13x28
#1113 @ 193,956: 21x11
#1114 @ 754,772: 11x25
#1115 @ 161,257: 28x17
#1116 @ 719,258: 19x11
#1117 @ 932,623: 27x26
#1118 @ 682,942: 28x26
#1119 @ 179,788: 26x10
#1120 @ 206,669: 15x19
#1121 @ 665,353: 12x11
#1122 @ 419,961: 18x18
#1123 @ 975,350: 19x24
#1124 @ 876,232: 10x19
#1125 @ 560,728: 12x26
#1126 @ 460,434: 14x22
#1127 @ 221,752: 16x28
#1128 @ 918,753: 27x22
#1129 @ 822,661: 20x11
#1130 @ 764,259: 16x18
#1131 @ 931,97: 14x17
#1132 @ 298,120: 23x14
#1133 @ 285,469: 14x20
#1134 @ 503,554: 28x15
#1135 @ 330,802: 17x14
#1136 @ 731,77: 20x10
#1137 @ 366,128: 16x10
#1138 @ 343,524: 16x26
#1139 @ 820,212: 11x19
#1140 @ 283,917: 16x27
#1141 @ 610,971: 17x11
#1142 @ 568,467: 14x16
#1143 @ 195,964: 10x13
#1144 @ 966,531: 12x25
#1145 @ 453,623: 13x23
#1146 @ 488,941: 17x15
#1147 @ 376,949: 22x29
#1148 @ 333,870: 22x18
#1149 @ 400,808: 11x17
#1150 @ 130,435: 28x16
#1151 @ 348,971: 12x20
#1152 @ 234,268: 26x29
#1153 @ 684,213: 13x24
#1154 @ 215,670: 21x25
#1155 @ 285,335: 24x17
#1156 @ 940,918: 21x19
#1157 @ 923,687: 11x10
#1158 @ 970,857: 13x21
#1159 @ 478,929: 18x27
#1160 @ 493,634: 20x29
#1161 @ 97,503: 16x11
#1162 @ 18,475: 27x16
#1163 @ 324,871: 12x28
#1164 @ 721,127: 18x15
#1165 @ 600,164: 21x26
#1166 @ 954,579: 21x10
#1167 @ 170,694: 12x29
#1168 @ 298,475: 29x25
#1169 @ 555,264: 25x15
#1170 @ 8,14: 26x14
#1171 @ 155,870: 10x14
#1172 @ 582,759: 18x10
#1173 @ 377,869: 13x25
#1174 @ 821,202: 10x14
#1175 @ 617,550: 16x14
#1176 @ 808,13: 17x17
#1177 @ 530,717: 27x25
#1178 @ 88,407: 24x15
#1179 @ 914,465: 16x16
#1180 @ 445,880: 26x29
#1181 @ 271,654: 19x17
#1182 @ 154,581: 10x11
#1183 @ 545,282: 24x16
#1184 @ 606,940: 11x17
#1185 @ 22,583: 11x20
#1186 @ 156,916: 19x25
#1187 @ 343,382: 11x18
#1188 @ 566,484: 18x12
#1189 @ 160,747: 16x17
#1190 @ 308,263: 28x21
#1191 @ 310,854: 23x10
#1192 @ 681,497: 13x16
#1193 @ 716,480: 20x16
#1194 @ 932,367: 23x21
#1195 @ 171,739: 11x14
#1196 @ 817,459: 11x10
#1197 @ 110,289: 10x20
#1198 @ 751,701: 19x29
#1199 @ 112,552: 10x20
#1200 @ 377,819: 12x21
#1201 @ 461,34: 29x23
#1202 @ 80,412: 14x19
#1203 @ 572,123: 18x26
#1204 @ 756,243: 21x21
#1205 @ 262,659: 15x12
#1206 @ 472,0: 18x17
#1207 @ 726,373: 22x12
#1208 @ 581,264: 10x24
#1209 @ 253,652: 14x19
#1210 @ 958,812: 17x11
#1211 @ 899,388: 18x23
#1212 @ 622,141: 14x14
#1213 @ 195,632: 27x25
#1214 @ 543,956: 27x23
#1215 @ 579,607: 28x17
#1216 @ 850,237: 29x18
#1217 @ 552,665: 14x26
#1218 @ 234,40: 15x22
#1219 @ 716,678: 24x25
#1220 @ 928,112: 23x11
#1221 @ 21,406: 27x14
#1222 @ 986,814: 11x25
#1223 @ 8,667: 28x17
#1224 @ 382,948: 27x19
#1225 @ 692,99: 27x22
#1226 @ 344,871: 29x25
#1227 @ 215,253: 22x22
#1228 @ 33,780: 11x20
#1229 @ 248,884: 11x25
#1230 @ 504,106: 26x26
#1231 @ 240,264: 23x27
#1232 @ 656,570: 11x10
#1233 @ 195,410: 10x13
#1234 @ 802,933: 10x15
#1235 @ 835,224: 23x17
#1236 @ 887,490: 28x23
#1237 @ 960,947: 8x7
#1238 @ 649,648: 22x18
#1239 @ 173,584: 23x13
#1240 @ 326,851: 25x10
#1241 @ 878,304: 9x10
#1242 @ 87,945: 22x14
#1243 @ 718,871: 14x24
#1244 @ 137,846: 23x15
#1245 @ 588,177: 16x23
#1246 @ 825,795: 20x27
#1247 @ 249,209: 25x10
#1248 @ 179,646: 22x13
#1249 @ 956,535: 18x24
#1250 @ 78,148: 14x28
#1251 @ 519,202: 28x16
#1252 @ 540,192: 28x17
#1253 @ 240,131: 22x25
#1254 @ 786,204: 19x16
#1255 @ 748,705: 12x22
#1256 @ 435,626: 19x29
#1257 @ 386,848: 23x29
#1258 @ 243,596: 10x27
#1259 @ 721,489: 13x17
#1260 @ 761,784: 10x29
#1261 @ 736,10: 20x27
#1262 @ 128,37: 11x25
#1263 @ 92,102: 26x19
#1264 @ 905,386: 12x26
#1265 @ 357,120: 18x19
#1266 @ 14,589: 22x19
#1267 @ 704,920: 18x10
#1268 @ 49,642: 10x26
#1269 @ 306,634: 28x28
#1270 @ 226,513: 22x16
#1271 @ 451,467: 12x21
#1272 @ 588,922: 24x29
#1273 @ 130,445: 13x29
#1274 @ 917,539: 14x23
#1275 @ 839,125: 13x11
#1276 @ 434,866: 23x21
#1277 @ 215,290: 29x21
#1278 @ 127,371: 16x27
#1279 @ 718,933: 20x18
#1280 @ 701,208: 26x13
#1281 @ 432,443: 26x28
#1282 @ 71,129: 10x15
#1283 @ 665,661: 12x14
#1284 @ 548,489: 19x23
#1285 @ 36,718: 10x18
#1286 @ 410,888: 10x22
#1287 @ 146,440: 11x19
#1288 @ 122,529: 14x23
#1289 @ 753,130: 22x10
#1290 @ 685,201: 11x28
#1291 @ 103,403: 20x24
#1292 @ 661,670: 25x10
#1293 @ 675,789: 21x21
#1294 @ 44,44: 17x10
#1295 @ 497,58: 13x10
#1296 @ 892,892: 21x10
#1297 @ 788,461: 17x25
#1298 @ 134,271: 22x19
#1299 @ 228,583: 19x29
#1300 @ 526,954: 29x27
#1301 @ 176,276: 23x28";
}
