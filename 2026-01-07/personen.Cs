using System;
using System.IO;
using System.Linq;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes; 


public class Person
{
    [Index(0)] public string Fullname { get; set; }
    [Index(1)] public string Email { get; set; }
    [Index(2)] public string Telefon { get; set; }
    [Index(3)] public string Adresse { get; set; }
    [Index(4)] public string Unicode { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true, 
            BadDataFound = null, 
            MissingFieldFound = null 
        };

        try 
        {
           
            using (var reader = new StreamReader("persons.csv"))
            using (var csv = new CsvReader(reader, config))
            {
               
                Person[] personenArray = csv.GetRecords<Person>().ToArray();

             
                Console.WriteLine($"Gelesene Personen: {personenArray.Length}\n");

                foreach (var p in personenArray)
                {
                    Console.WriteLine($"Name: {p.Fullname}");
                    Console.WriteLine($"Email: {p.Email}");
                    Console.WriteLine($"Telefon: {p.Telefon}");
                    Console.WriteLine($"Adresse: {p.Adresse}");
                    Console.WriteLine($"Unicode: {p.Unicode}");
                    Console.WriteLine($"------------------------------");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Lesen: {ex.Message}");
        }
    }
}
