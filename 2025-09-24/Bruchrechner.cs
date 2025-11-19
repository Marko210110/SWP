using System;
using System.Text.RegularExpressions;
using System.Threading; 

internal readonly struct Fraction
{
    public long N { get; }  
    public long D { get; }  

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
        if (num < 0) throw new ArgumentException("Zähler im gemischten Bruch darf nicht negativ sein.");
        long sign = whole < 0 ? -1 : 1;
        long absWhole = Math.Abs(whole);
        long improperNum = checked(absWhole * den + num);
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
        // 0 als Sonderfall
        if (N == 0) return "0";

        long absN = Math.Abs(N);
        long whole = absN / D;
        long rem = absN % D;

        // Stelle sicher, dass der Bruchteil gekürzt ist (falls erforderlich)
        if (rem != 0)
        {
            long g = Gcd(rem, D);
            rem /= g;
            long denReduced = D / g;

            if (whole == 0)
                return (N < 0 ? "-" : "") + $"{rem}/{denReduced}";

            return (N < 0 ? "-" : "") + $"{whole} {rem}/{denReduced}";
        }

        // kein Rest -> ganze Zahl
        return (N < 0 ? "-" : "") + $"{whole}";
    }

    private static long Gcd(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
        {
            long t = a % b;
            a = b;
            b = t;
        }
        return a == 0 ? 1 : a;
    }
}

internal static partial class Program
{
    private static void Main()
    {
        RunTests();

        Console.WriteLine("Bruchrechner – zwei Brüche addieren");
        Console.WriteLine("Erlaubte Formate: Ganze Zahl (3), Bruch (5/7), Gemischter Bruch (2 3/8)");
        Console.WriteLine();

        Fraction f1 = ReadFraction("Bitte ersten Bruch eingeben: ");
        Fraction f2 = ReadFraction("Bitte zweiten Bruch eingeben: ");

        try
        {
            Fraction sum = f1 + f2;
            Console.WriteLine();
            Console.WriteLine($"{f1} + {f2} = {sum}");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Fehler: Die Berechnung führte zu einem Überlauf. Verwende kleinere Zahlen.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unerwarteter Fehler bei der Addition: " + ex.Message);
        }
    }

    private static void RunTests()
    {
        Console.WriteLine("Starte Tests...\n");

        Console.WriteLine("Test: Addition");
        try
        {
            var a = Fraction.Parse("1/2");
            var b = Fraction.Parse("1/3");
            var c = a + b;
            if (c.N != 5 || c.D != 6) throw new Exception("Addition fehlgeschlagen");
            Console.WriteLine("Addition-Test OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Addition-Test FEHLER: " + ex.Message);
        }
        Thread.Sleep(500);

        Console.WriteLine("\nTest: Parse gemischter Bruch");
        try
        {
            var f = Fraction.Parse("2 3/8");
            if (f.N != 19 || f.D != 8) throw new Exception("Parse gemischter Bruch fehlgeschlagen");
            Console.WriteLine("Parse-Test OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Parse-Test FEHLER: " + ex.Message);
        }
        Thread.Sleep(500);

        Console.WriteLine("\nTest: Parse ungültig");
        try
        {
            Fraction.Parse("abc");
            Console.WriteLine("Parse ungültig-Test FEHLER: Keine Exception");
        }
        catch (FormatException)
        {
            Console.WriteLine("Parse ungültig-Test OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Parse ungültig-Test FEHLER: " + ex.Message);
        }
        Thread.Sleep(500);

        Console.WriteLine("\nTest: Nenner = 0");
        try
        {
            var f = new Fraction(1, 0);
            Console.WriteLine("Nenner-0-Test FEHLER: Keine Exception");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Nenner-0-Test OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Nenner-0-Test FEHLER: " + ex.Message);
        }
        Thread.Sleep(500);

        Console.WriteLine("\nTest: ToString");
        try
        {
            var f = Fraction.Parse("-2 3/8");
            if (f.ToString() != "-2 3/8") throw new Exception("ToString fehlgeschlagen");
            Console.WriteLine("ToString-Test OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine("ToString-Test FEHLER: " + ex.Message);
        }
        Thread.Sleep(500);

        Console.WriteLine("\nTest: Zufällige Brüche und Addition");
        var rnd = new Random();
        for (int i = 1; i <= 5; i++)
        {
            try
            {
                long n1 = rnd.Next(-10, 11);
                long d1 = rnd.Next(1, 11);
                long n2 = rnd.Next(-10, 11);
                long d2 = rnd.Next(1, 11);

                var f1 = new Fraction(n1, d1);
                var f2 = new Fraction(n2, d2);
                var sum = f1 + f2;

                Console.WriteLine($"Testfall {i}: {f1} + {f2} = {sum}");
            }
            catch (OverflowException)
            {
                Console.WriteLine($"Testfall {i}: FEHLER - Überlauf bei Berechnung.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Testfall {i}: FEHLER - {ex.Message}");
            }
            Thread.Sleep(500);
        }

        Console.WriteLine("\nTests abgeschlossen.\n");
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
            catch (FormatException)
            {
                Console.WriteLine("Ungültiges Format. Erlaubte Formate: Ganze Zahl (3), Bruch (5/7), Gemischter Bruch (2 3/8)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Eingabefehler: " + ex.Message);
            }
            catch (OverflowException)
            {
                Console.WriteLine("Zahl zu groß. Bitte kleinere Werte eingeben.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unerwarteter Fehler: " + ex.Message);
            }
        }
    }
}
