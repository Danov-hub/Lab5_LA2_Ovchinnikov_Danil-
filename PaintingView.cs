namespace HermitageApp.Models;

public class PaintingView
{
    private int _id;
    private string _title;
    private string _artistName;
    private int? _partOfHermitage;
    private string? _year;
    private string _styleName;

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

    public string ArtistName
    {
        get { return _artistName; }
        set { _artistName = value; }
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

    public string StyleName
    {
        get { return _styleName; }
        set { _styleName = value; }
    }

    public PaintingView(int id, string title, string artistName,
        int? partOfHermitage, string? year, string styleName)
    {
        _id = id;
        _title = title;
        _artistName = artistName;
        _partOfHermitage = partOfHermitage;
        _year = year;
        _styleName = styleName;
    }

    public override string ToString()
    {
        return $"[{Id}] {Title} | {ArtistName} | " +
               $"Часть: {PartOfHermitage?.ToString() ?? "-"} | " +
               $"Год: {Year ?? "-"} | {StyleName}";
    }
}