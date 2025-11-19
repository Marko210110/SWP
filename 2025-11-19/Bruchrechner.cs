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
        if (N == 0) return "0";

        long absN = Math.Abs(N);
        long whole = absN / D;
        long rem = absN % D;

        if (rem != 0)
        {
            long g = Gcd(rem, D);
            rem /= g;
            long denReduced = D / g;

            if (whole == 0)
                return (N < 0 ? "-" : "") + $"{rem}/{denReduced}";

            return (N < 0 ? "-" : "") + $"{whole} {rem}/{denReduced}";
        }

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
    // Main verarbeitet jetzt Argumente
    private static void Main(string[] args)
    {
        // Fall 1: Keine Argumente -> Interaktiver Modus
        if (args.Length == 0)
        {
            RunInteractiveMode();
        }
        // Fall 2: "test" als Argument -> Tests ausführen
        else if (args.Length == 1 && args[0].ToLower() == "test")
        {
            RunTests();
        }
        // Fall 3: Zwei Argumente -> Direkt berechnen
        else if (args.Length == 2)
        {
            try
            {
                Fraction f1 = Fraction.Parse(args[0]);
                Fraction f2 = Fraction.Parse(args[1]);
                Console.WriteLine($"{f1} + {f2} = {f1 + f2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Berechnung: {ex.Message}");
                ShowHelp();
            }
        }
        // Fall 4: Falsche Anzahl an Argumenten
        else
        {
            ShowHelp();
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("Verwendung:");
        Console.WriteLine("  dotnet run                   -> Interaktiver Modus");
        Console.WriteLine("  dotnet run test              -> Testmodus");
        Console.WriteLine("  dotnet run \"1/2\" \"1/4\"       -> Berechnet Summe (Ausgabe: 3/4)");
        Console.WriteLine("  dotnet run \"1 1/2\" \"2 1/2\"   -> Berechnet Summe (Ausgabe: 4)");
    }

    private static void RunInteractiveMode()
    {
        Console.WriteLine("Bruchrechner – Interaktiver Modus");
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
        catch (Exception ex)
        {
            Console.WriteLine("Fehler: " + ex.Message);
        }
    }

    private static void RunTests()
    {
        Console.WriteLine("Starte Tests...\n");

        // Test 1: Spezifische Anforderung aus der HÜ
        // "1 1/2" + "2 1/2" soll "4" ergeben
        TestCalculation("1 1/2", "2 1/2", "4");

        // Test 2: Einfache Brüche
        // "1/2" + "1/4" soll "3/4" ergeben
        TestCalculation("1/2", "1/4", "3/4");

        // Test 3: Gemischt + Bruch
        // "1 1/2" + "1/2" soll "2" ergeben
        TestCalculation("1 1/2", "1/2", "2");

        // Test 4: Negative Zahlen
        // "-1/2" + "1/2" soll "0" ergeben
        TestCalculation("-1/2", "1/2", "0");

        // Test 5: Constructor Exception (Ungültiger Wert)
        Console.Write("Test: Exception bei Nenner 0... ");
        try
        {
            var f = new Fraction(1, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FEHLER (Keine Exception geworfen)");
        }
        catch (ArgumentException)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("OK (ArgumentException gefangen)");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FEHLER (Falsche Exception: {ex.GetType().Name})");
        }
        Console.ResetColor();

        // Test 6: Parse Exception
        Console.Write("Test: Exception bei ungültigem String... ");
        try
        {
            Fraction.Parse("Hallo Welt");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FEHLER (Keine Exception geworfen)");
        }
        catch (FormatException)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("OK (FormatException gefangen)");
        }
        Console.ResetColor();

        Console.WriteLine("\nTests abgeschlossen.");
    }

    private static void TestCalculation(string s1, string s2, string expected)
    {
        Console.Write($"Test: {s1} + {s2} == {expected} ... ");
        try
        {
            var f1 = Fraction.Parse(s1);
            var f2 = Fraction.Parse(s2);
            var sum = f1 + f2;
            if (sum.ToString() == expected)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"FEHLER (Erwartet: {expected}, Ist: {sum})");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"EXCEPTION: {ex.Message}");
        }
        Console.ResetColor();
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
                Console.WriteLine($"Eingabefehler: {ex.Message}. Bitte erneut versuchen.");
            }
        }
    }
}
