using System;
using System.Text.RegularExpressions;

internal readonly struct Fraction
{
    public long N { get; }  // Zähler
    public long D { get; }  // Nenner

    public Fraction(long numerator, long denominator)
    {
        if (denominator == 0) throw new ArgumentException("Nenner darf nicht 0 sein.");
        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }
        long g = Gcd(Math.Abs(numerator), denominator);
        N = numerator / g;
        D = denominator / g;
    }

    public static Fraction FromMixed(long whole, long num, long den)
    {
        if (den <= 0) throw new ArgumentException("Nenner muss > 0 sein.");
        long sign = whole < 0 ? -1 : 1;
        long absWhole = Math.Abs(whole);
        long improperNum = absWhole * den + num;
        return new Fraction(sign * improperNum, den);
    }

    public static Fraction Parse(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new FormatException("Leerer Ausdruck.");

        s = s.Trim();

        var mixed = Regex.Match(s, @"^([+-]?\d+)\s+(\d+)\s*/\s*(\d+)$");
        if (mixed.Success)
            return FromMixed(long.Parse(mixed.Groups[1].Value),
                             long.Parse(mixed.Groups[2].Value),
                             long.Parse(mixed.Groups[3].Value));

        var frac = Regex.Match(s, @"^([+-]?\d+)\s*/\s*(\d+)$");
        if (frac.Success)
            return new Fraction(long.Parse(frac.Groups[1].Value),
                                long.Parse(frac.Groups[2].Value));

        var whole = Regex.Match(s, @"^([+-]?\d+)$");
        if (whole.Success)
            return new Fraction(long.Parse(whole.Groups[1].Value), 1);

        throw new FormatException($"Ungültiges Format: \"{s}\"");
    }

    public static Fraction operator +(Fraction a, Fraction b)
    {
        long g1 = Gcd(Math.Abs(a.N), b.D);
        long g2 = Gcd(Math.Abs(b.N), a.D);

        long n1 = a.N / g1;
        long d1 = a.D / g2;
        long n2 = b.N / g2;
        long d2 = b.D / g1;

        long numerator = checked(n1 * d2 + n2 * d1);
        long denominator = checked(d1 * d2);

        return new Fraction(numerator, denominator);
    }

    public override string ToString()
    {
        if (N == 0) return "0";
        long absN = Math.Abs(N);
        long whole = absN / D;
        long rem = absN % D;

        if (rem == 0)
            return (N < 0 ? "-" : "") + whole;

        if (whole == 0)
            return (N < 0 ? "-" : "") + $"{rem}/{D}";

        return (N < 0 ? "-" : "") + $"{whole} {rem}/{D}";
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long t = a % b;
            a = b;
            b = t;
        }
        return a == 0 ? 1 : Math.Abs(a);
    }
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("Bruchrechner – zwei Brüche addieren");
        Console.WriteLine("Erlaubte Formate: Ganze Zahl (3), Bruch (5/7), Gemischter Bruch (2 3/8)");
        Console.WriteLine();

        Fraction f1 = ReadFraction("Bitte ersten Bruch eingeben: ");
        Fraction f2 = ReadFraction("Bitte zweiten Bruch eingeben: ");

        Fraction sum = f1 + f2;
        Console.WriteLine();
        Console.WriteLine($"{f1} + {f2} = {sum}");
    }

    private static Fraction ReadFraction(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            try
            {
                return Fraction.Parse(input ?? "");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }
    }
}
