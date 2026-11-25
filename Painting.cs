namespace HermitageApp.Models;

public class Painting
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? ArtistId { get; set; }
    public int? PartOfHermitage { get; set; }
    public string? Year { get; set; }
    public int? StyleId { get; set; }

    public Painting() { }

    public Painting(int id, string title, int? artistId, 
        int? part, string? year, int? styleId)
    {
        Id = id;
        Title = title;
        ArtistId = artistId;
        PartOfHermitage = part;
        Year = year;
        StyleId = styleId;
    }

    public override string ToString() =>
        $"[{Id}] {Title} | Художник: {ArtistId?.ToString() ?? "-"} | " +
        $"Часть: {PartOfHermitage?.ToString() ?? "-"} | " +
        $"Год: {Year ?? "-"} | Стиль: {StyleId?.ToString() ?? "-"}";
}