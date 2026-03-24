using System;
using System.Collections.Generic;

class Program
{

    static void PreisHinzufuegen(ref double summe, double preis)
    {
        summe += preis;
    }

 
    static bool ArtikelSuchen(List<string> liste, string name, out int index)
    {
        for (int i = 0; i < liste.Count; i++)
        {
            if (liste[i].ToLower() == name.ToLower())
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    static void Main(string[] args)
    {
        List<string> einkaufsliste = new List<string>();
        einkaufsliste.Add("Milch");
        einkaufsliste.Add("Brot");
        einkaufsliste.Add("Äpfel");
        einkaufsliste.Add("Käse");


        Console.WriteLine("=== Einkaufsliste ===");
        foreach (string artikel in einkaufsliste)
        {
            if (artikel.Length < 4)
            {
                continue; /
            }
            Console.WriteLine("- " + artikel.ToUpper()); 
        }

        
        double summe = 0.0;
        PreisHinzufuegen(ref summe, 1.2); 
        PreisHinzufuegen(ref summe, 2.4); 
        PreisHinzufuegen(ref summe, 3.9); 
        Console.WriteLine("\nGesamtpreis: " + summe + " €");


        Console.WriteLine("\n[ Suche nach 'Brot' ]");
        if (ArtikelSuchen(einkaufsliste, "Brot", out int gefundenerIndex))
        {
            Console.WriteLine("Gefunden an Position: " + gefundenerIndex);
        }
        else
        {
            Console.WriteLine("Nicht gefunden.");
        }


        Console.WriteLine("\n[ Erster Artikel mit mehr als 4 Buchstaben ]");
        foreach (string artikel in einkaufsliste)
        {
            if (artikel.Length > 4)
            {
                Console.WriteLine("Gefunden: " + artikel);
                break; 
            }
        }
    }
}
