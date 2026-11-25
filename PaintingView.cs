namespace HermitageApp.Models;

public class PaintingView
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public int? PartOfHermitage { get; set; }
    public string? Year { get; set; }
    public string StyleName { get; set; } = string.Empty;

    public override string ToString() =>
        $"[{Id}] {Title} | {ArtistName} | " +
        $"Часть: {PartOfHermitage?.ToString() ?? "-"} | " +
        $"Год: {Year ?? "-"} | {StyleName}";
}