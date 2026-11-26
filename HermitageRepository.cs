using ClosedXML.Excel;
using HermitageApp.Models;

namespace HermitageApp.Services;

public class HermitageRepository
{
    private readonly string _filePath;

    public List<Artist> Artists { get; } = new();
    public List<Style> Styles { get; } = new();
    public List<Painting> Paintings { get; } = new();

    public HermitageRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void Load()
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException
                ($"Файл не найден: {_filePath}");

        using var workbook = new XLWorkbook(_filePath);

        Artists.Clear();
        Styles.Clear();
        Paintings.Clear();

        var wsPaintings = workbook.Worksheet("Картины");
        foreach (var row in wsPaintings.RowsUsed().Skip(1))
        {
            if (row.Cell(1).IsEmpty()) continue;

            Paintings.Add(new Painting(
                row.Cell(1).GetValue<int>(),
                row.Cell(2).GetString().Trim(),
                ParseNullableInt(row.Cell(3)),
                ParseNullableInt(row.Cell(4)),
                row.Cell(5).IsEmpty() ? null : 
                row.Cell(5).GetString().Trim(),
                ParseNullableInt(row.Cell(6))
            ));
        }

        if (workbook.Worksheets.TryGetWorksheet("Художники", out var wsArtists))
        {
            string idStr;
            int artistId;
            string name;

            foreach (var row in wsArtists.RowsUsed().Skip(1))
            {
                if (row.Cell(1).IsEmpty()) continue;

                idStr = row.Cell(1).GetString().Trim();

                if (!int.TryParse(idStr, out artistId)) continue;

                name = row.Cell(2).GetString().Trim();
                if (string.IsNullOrWhiteSpace(name)) continue;

                Artists.Add(new Artist(artistId, name));
            }
        }

        if (workbook.Worksheets.TryGetWorksheet
            ("Стиль", out var wsStyles))
        {
            string idStr;
            int styleId;
            string name;

            foreach (var row in wsStyles.RowsUsed().Skip(1))
            {
                if (row.Cell(1).IsEmpty()) continue;

                idStr = row.Cell(1).GetString().Trim();
                if (!int.TryParse(idStr, out styleId)) continue;

                name = row.Cell(2).GetString().Trim();
                if (string.IsNullOrWhiteSpace(name)) continue;

                Styles.Add(new Style(styleId, name));
            }
        }

        Console.WriteLine($"[Загрузка] Картин: " +
            $"{Paintings.Count}, Художников: {Artists.Count}, " +
            $"Стилей: {Styles.Count}");
    }

    private static int? ParseNullableInt(IXLCell cell)
    {
        if (cell.IsEmpty()) return null;
        var text = cell.GetString().Trim();
        return int.TryParse(text, out var value) ? value : null;
    }

    public void Save()
    {
        using var workbook = File.Exists(_filePath)
            ? new XLWorkbook(_filePath)
            : new XLWorkbook();

        SaveSheet(workbook, "Картины", new[]
        {
            "ID", "Название", "ID Художника", 
            "Часть эрмитажа", "Год", "ID стиля"
        }, Paintings.Select(p => new object?[]
        {
            p.Id, p.Title, p.ArtistId, 
            p.PartOfHermitage, p.Year, p.StyleId
        }));

        SaveSheet(workbook, "Художники", new[] { "ID", "Имя" },
            Artists.Select(a => new object?[] { a.Id, a.Name }));

        SaveSheet(workbook, "Стиль", new[] { "ID", "Название" },
            Styles.Select(s => new object?[] { s.Id, s.Name }));

        workbook.SaveAs(_filePath);
    }

    private static void SaveSheet(XLWorkbook wb, 
        string sheetName, string[] headers, 
        IEnumerable<object?[]> rows)
    {
        if (wb.Worksheets.TryGetWorksheet(sheetName, out var ws))
            ws.Clear();
        else
            ws = wb.Worksheets.Add(sheetName);

        int i;
        int r;
        int c;

        for (i = 0; i < headers.Length; i++)
            ws.Cell(1, i + 1).Value = headers[i];

        r = 2;
        foreach (var row in rows)
        {
            for (c = 0; c < row.Length; c++)
                ws.Cell(r, c + 1).Value = XLCellValue.FromObject(row[c]);
            r++;
        }
    }

    public List<PaintingView> GetPaintingsView()
    {
        return (from p in Paintings
                join a in Artists on p.ArtistId equals a.Id into ag
                from a in ag.DefaultIfEmpty()
                join s in Styles on p.StyleId equals s.Id into sg
                from s in sg.DefaultIfEmpty()
                select new PaintingView(
                    p.Id,
                    p.Title,
                    a?.Name ?? "—",
                    p.PartOfHermitage,
                    p.Year,
                    s?.Name ?? "—"
                )).ToList();
    }

    public bool DeletePainting(int id)
    {
        var item = Paintings.FirstOrDefault(p => p.Id == id);
        if (item == null) return false;
        Paintings.Remove(item);
        return true;
    }

    public void AddPainting(Painting painting)
    {
        if (Paintings.Any(p => p.Id == painting.Id))
            throw new InvalidOperationException
                ($"Картина с ID={painting.Id} уже существует.");
        Paintings.Add(painting);
    }

    public int NextPaintingId() => Paintings.Count == 0 ? 1 : 
        Paintings.Max(p => p.Id) + 1;

    // Запрос 1
    public List<Painting> Query1_PaintingsOf19thCentury()
    {
        return (from p in Paintings
                where p.Year != null
                let y = int.TryParse(p.Year, out var v) ? v : 0
                where y >= 1800 && y <= 1899
                orderby y
                select p).ToList();
    }

    // Запрос 2
    public List<string> Query2_OldPaintingsWithArtists()
    {
        return (from p in Paintings
                join a in Artists on p.ArtistId equals a.Id
                where p.Year != null
                let y = int.TryParse(p.Year, 
                out var v) ? v : int.MaxValue
                where y < 1600
                orderby y
                select $"{p.Title} — {a.Name}" +
                $" ({p.Year})").ToList();
    }

    // Запрос 3
    public List<PaintingView> Query3_Part1WithArtistAndStyle()
    {
        return (from p in Paintings
                join a in Artists on p.ArtistId equals a.Id
                join s in Styles on p.StyleId equals s.Id
                where p.PartOfHermitage == 1
                select new PaintingView(
                    p.Id,
                    p.Title,
                    a.Name,
                    p.PartOfHermitage,
                    p.Year,
                    s.Name
                )).ToList();
    }

    // Запрос 4
    public int Query4_ArtistsWithMoreThan5InPart2()
    {
        return (from p in Paintings
                join a in Artists on p.ArtistId equals a.Id
                join s in Styles on p.StyleId equals s.Id
                where p.PartOfHermitage == 2
                group p by a.Id into g
                where g.Count() > 5
                select g.Key).Count();
    }

    // Запрос 5
    public int Query5_PaintingsWithoutYear()
    {
        return Paintings.Count(p => 
        string.IsNullOrWhiteSpace(p.Year));
    }
}