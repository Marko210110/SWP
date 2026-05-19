using Microsoft.EntityFrameworkCore;
using Schulverwaltung.Data;
using Schulverwaltung.Models;

// Datenbank erstellen und sicherstellen, dass sie existiert
using var context = new SchulDbContext();
context.Database.EnsureCreated();

Console.WriteLine("=== Schulverwaltungssystem ===");
Console.WriteLine("SQLite-Datenbank: schulverwaltung.db");

bool running = true;
while (running)
{
    Console.WriteLine();
    Console.WriteLine("Hauptmenü:");
    Console.WriteLine("1 - Schüler verwalten");
    Console.WriteLine("2 - Kurse verwalten");
    Console.WriteLine("3 - Schüler einem Kurs zuweisen");
    Console.WriteLine("4 - Programm beenden");
    Console.Write("Auswahl: ");

    var hauptwahl = Console.ReadLine();

    switch (hauptwahl)
    {
        case "1":
            SchuelerVerwalten(context);
            break;
        case "2":
            KurseVerwalten(context);
            break;
        case "3":
            SchuelerZuKursZuweisen(context);
            break;
        case "4":
            running = false;
            Console.WriteLine("Programm wird beendet.");
            break;
        default:
            Console.WriteLine("Ungültige Eingabe.");
            break;
    }
}

// --- Schüler CRUD ---
static void SchuelerVerwalten(SchulDbContext context)
{
    Console.WriteLine();
    Console.WriteLine("Schüler-Verwaltung:");
    Console.WriteLine("a - Alle Schüler anzeigen");
    Console.WriteLine("c - Schüler erstellen");
    Console.WriteLine("u - Schüler aktualisieren");
    Console.WriteLine("d - Schüler löschen");
    Console.Write("Auswahl: ");

    var wahl = Console.ReadLine();

    switch (wahl)
    {
        case "a":
            var schuelerListe = context.Schueler.Include(s => s.Kurse).ToList();
            Console.WriteLine("--- Alle Schüler ---");
            foreach (var s in schuelerListe)
            {
                var kurse = string.Join(", ", s.Kurse.Select(k => k.Name));
                Console.WriteLine($"ID: {s.Id}, {s.Vorname} {s.Nachname}, Geb: {s.Geburtsdatum:yyyy-MM-dd}, Email: {s.Email}, Kurse: [{kurse}]");
            }
            break;

        case "c":
            Console.Write("Vorname: ");
            var vorname = Console.ReadLine() ?? "";
            Console.Write("Nachname: ");
            var nachname = Console.ReadLine() ?? "";
            Console.Write("Geburtsdatum (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out var geb))
            {
                Console.Write("Email: ");
                var email = Console.ReadLine() ?? "";
                context.Schueler.Add(new Schueler { Vorname = vorname, Nachname = nachname, Geburtsdatum = geb, Email = email });
                context.SaveChanges();
                Console.WriteLine("Schüler erfolgreich erstellt.");
            }
            else
            {
                Console.WriteLine("Ungültiges Datum.");
            }
            break;

        case "u":
            Console.Write("ID des zu aktualisierenden Schülers: ");
            if (int.TryParse(Console.ReadLine(), out int sid))
            {
                var sUpdate = context.Schueler.Find(sid);
                if (sUpdate != null)
                {
                    Console.Write($"Neuer Vorname ({sUpdate.Vorname}): ");
                    var nv = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nv)) sUpdate.Vorname = nv;

                    Console.Write($"Neuer Nachname ({sUpdate.Nachname}): ");
                    var nn = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nn)) sUpdate.Nachname = nn;

                    Console.Write($"Neue Email ({sUpdate.Email}): ");
                    var ne = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(ne)) sUpdate.Email = ne;

                    context.SaveChanges();
                    Console.WriteLine("Schüler aktualisiert.");
                }
                else Console.WriteLine("Schüler nicht gefunden.");
            }
            break;

        case "d":
            Console.Write("ID des zu löschenden Schülers: ");
            if (int.TryParse(Console.ReadLine(), out int did))
            {
                var sDel = context.Schueler.Find(did);
                if (sDel != null)
                {
                    context.Schueler.Remove(sDel);
                    context.SaveChanges();
                    Console.WriteLine("Schüler gelöscht.");
                }
                else Console.WriteLine("Schüler nicht gefunden.");
            }
            break;

        default:
            Console.WriteLine("Ungültige Auswahl.");
            break;
    }
}

// --- Kurse CRUD ---
static void KurseVerwalten(SchulDbContext context)
{
    Console.WriteLine();
    Console.WriteLine("Kurs-Verwaltung:");
    Console.WriteLine("a - Alle Kurse anzeigen");
    Console.WriteLine("c - Kurs erstellen");
    Console.WriteLine("u - Kurs aktualisieren");
    Console.WriteLine("d - Kurs löschen");
    Console.Write("Auswahl: ");

    var wahl = Console.ReadLine();

    switch (wahl)
    {
        case "a":
            var kurseListe = context.Kurse.Include(k => k.Schueler).ToList();
            Console.WriteLine("--- Alle Kurse ---");
            foreach (var k in kurseListe)
            {
                var schueler = string.Join(", ", k.Schueler.Select(s => $"{s.Vorname} {s.Nachname}"));
                Console.WriteLine($"ID: {k.Id}, Name: {k.Name}, Fach: {k.Fach}, Raum: {k.Raumnummer}, Schüler: [{schueler}]");
            }
            break;

        case "c":
            Console.Write("Kursname: ");
            var kname = Console.ReadLine() ?? "";
            Console.Write("Fach: ");
            var fach = Console.ReadLine() ?? "";
            Console.Write("Raumnummer: ");
            var raum = Console.ReadLine() ?? "";
            context.Kurse.Add(new Kurs { Name = kname, Fach = fach, Raumnummer = raum });
            context.SaveChanges();
            Console.WriteLine("Kurs erfolgreich erstellt.");
            break;

        case "u":
            Console.Write("ID des zu aktualisierenden Kurses: ");
            if (int.TryParse(Console.ReadLine(), out int kid))
            {
                var kUpdate = context.Kurse.Find(kid);
                if (kUpdate != null)
                {
                    Console.Write($"Neuer Name ({kUpdate.Name}): ");
                    var nn = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nn)) kUpdate.Name = nn;

                    Console.Write($"Neues Fach ({kUpdate.Fach}): ");
                    var nf = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nf)) kUpdate.Fach = nf;

                    Console.Write($"Neue Raumnummer ({kUpdate.Raumnummer}): ");
                    var nr = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nr)) kUpdate.Raumnummer = nr;

                    context.SaveChanges();
                    Console.WriteLine("Kurs aktualisiert.");
                }
                else Console.WriteLine("Kurs nicht gefunden.");
            }
            break;

        case "d":
            Console.Write("ID des zu löschenden Kurses: ");
            if (int.TryParse(Console.ReadLine(), out int kdid))
            {
                var kDel = context.Kurse.Find(kdid);
                if (kDel != null)
                {
                    context.Kurse.Remove(kDel);
                    context.SaveChanges();
                    Console.WriteLine("Kurs gelöscht.");
                }
                else Console.WriteLine("Kurs nicht gefunden.");
            }
            break;

        default:
            Console.WriteLine("Ungültige Auswahl.");
            break;
    }
}

// --- Many-to-Many: Zuweisung ---
static void SchuelerZuKursZuweisen(SchulDbContext context)
{
    Console.WriteLine();
    Console.Write("Schüler-ID: ");
    if (int.TryParse(Console.ReadLine(), out int sid))
    {
        Console.Write("Kurs-ID: ");
        if (int.TryParse(Console.ReadLine(), out int kid))
        {
            var schueler = context.Schueler.Include(s => s.Kurse).FirstOrDefault(s => s.Id == sid);
            var kurs = context.Kurse.Find(kid);

            if (schueler != null && kurs != null)
            {
                if (!schueler.Kurse.Contains(kurs))
                {
                    schueler.Kurse.Add(kurs);
                    context.SaveChanges();
                    Console.WriteLine($"'{schueler.Vorname} {schueler.Nachname}' wurde dem Kurs '{kurs.Name}' zugewiesen.");
                }
                else
                {
                    Console.WriteLine("Der Schüler ist bereits in diesem Kurs eingeschrieben.");
                }
            }
            else
            {
                Console.WriteLine("Schüler oder Kurs nicht gefunden.");
            }
        }
    }
}
