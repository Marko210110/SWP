using System;
using System.Collections.Generic;


public interface IInventarGegenstand
{
    string Name { get; }
    string BeschreibeDich();
}


public class Waffe : IInventarGegenstand
{
    public string Name { get; }
    public int Schaden { get; }

    public Waffe(string name, int schaden)
    {
        Name = name;
        Schaden = schaden;
    }

    public string BeschreibeDich()
    {
        return $"Ich bin {Name} und mache {Schaden} Schaden.";
    }
}


public class Heiltrank : IInventarGegenstand
{
    public string Name { get; }
    public int Heilwert { get; }

    public Heiltrank(string name, int heilwert)
    {
        Name = name;
        Heilwert = heilwert;
    }

    public string BeschreibeDich()
    {
        return $"Ich bin {Name} und heile {Heilwert} Lebenspunkte.";
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        List<IInventarGegenstand> inventar = new List<IInventarGegenstand>();

        inventar.Add(new Waffe("das Schwert", 15));
        inventar.Add(new Heiltrank("der Heiltrank", 25));

        foreach (IInventarGegenstand gegenstand in inventar)
        {
            Console.WriteLine(gegenstand.BeschreibeDich());
        }
    }
}