namespace Schulverwaltung.Models;

public class Schueler
{
    public int Id { get; set; }
    public string Vorname { get; set; } = string.Empty;
    public string Nachname { get; set; } = string.Empty;
    public DateTime Geburtsdatum { get; set; }
    public string Email { get; set; } = string.Empty;

    public ICollection<Kurs> Kurse { get; set; } = new List<Kurs>();
}
