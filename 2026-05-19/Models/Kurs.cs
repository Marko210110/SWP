namespace Schulverwaltung.Models;

public class Kurs
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Fach { get; set; } = string.Empty;
    public string Raumnummer { get; set; } = string.Empty;

    public ICollection<Schueler> Schueler { get; set; } = new List<Schueler>();
}
