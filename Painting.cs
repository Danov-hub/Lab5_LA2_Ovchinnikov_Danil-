namespace HermitageApp.Models;

public class Painting
{
    private int _id;
    private string _title;
    private int? _artistId;
    private int? _partOfHermitage;
    private string? _year;
    private int? _styleId;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public int? ArtistId
    {
        get { return _artistId; }
        set { _artistId = value; }
    }

    public int? PartOfHermitage
    {
        get { return _partOfHermitage; }
        set { _partOfHermitage = value; }
    }

    public string? Year
    {
        get { return _year; }
        set { _year = value; }
    }

    public int? StyleId
    {
        get { return _styleId; }
        set { _styleId = value; }
    }

    public Painting(int id, string title, int? artistId,
        int? part, string? year, int? styleId)
    {
        _id = id;
        _title = title;
        _artistId = artistId;
        _partOfHermitage = part;
        _year = year;
        _styleId = styleId;
    }

    public override string ToString()
    {
        return $"[{Id}] {Title} | Художник: " +
            $"{ArtistId?.ToString() ?? "-"} | " +
               $"Часть: {PartOfHermitage?.ToString() ?? "-"} | " +
               $"Год: {Year ?? "-"} | Стиль: " +
               $"{StyleId?.ToString() ?? "-"}";
    }
}