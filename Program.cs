using HermitageApp.Models;
using HermitageApp.Services;

namespace HermitageApp;

public static class Program
{
    private const string FilePath = 
        @"C:\\Users\\odani\\Documents\\лаба 5\\LR5-var11.xlsx";

    public static void Main()
    {
        try
        {
            var repo = new HermitageRepository(FilePath);
            repo.Load();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Эрмитаж ===");
                Console.WriteLine("1. Просмотр базы данных");
                Console.WriteLine("2. Удалить картину по ID");
                Console.WriteLine("3. Добавить картину");
                Console.WriteLine("4. Запрос 1: " +
                    "картины 19 века (1 таблица)");
                Console.WriteLine("5. Запрос 2: " +
                    "картины до 1600 г. с художниками " +
                    "(2 таблицы)");
                Console.WriteLine("6. Запрос 3: " +
                    "картины 1-й части с художником и стилем" +
                    " (3 таблицы)");
                Console.WriteLine("7. Запрос 4: " +
                    "художники с >5 картин во 2-й части " +
                    "(3 таблицы, 1 значение)");
                Console.WriteLine("8. Запрос 5: " +
                    "количество картин без года " +
                    "(1 таблица, 1 значение)");
                Console.WriteLine("9. Сохранить изменения в Excel");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAll(repo); break;
                    case "2": DeletePainting(repo); break;
                    case "3": AddPainting(repo); break;
                    case "4":
                        PrintList("Картины 19 века",
                            repo.Query1_PaintingsOf19thCentury().
                            Select(p => p.ToString()));
                        break;
                    case "5":
                        PrintList("Картины до 1600 г.",
                            repo.Query2_OldPaintingsWithArtists());
                        break;
                    case "6":
                        PrintList("Картины 1-й части",
                            repo.Query3_Part1WithArtistAndStyle().
                            Select(p => p.ToString()));
                        break;
                    case "7":
                        Console.WriteLine($"Художников " +
                            $"с >5 картин во 2-й части: " +
                                          $"{repo.Query4_ArtistsWithMoreThan5InPart2()}");
                        break;
                    case "8":
                        Console.WriteLine($"Картин без года: " +
                            $"{repo.Query5_PaintingsWithoutYear()}");
                        break;
                    case "9":
                        repo.Save();
                        Console.WriteLine("Данные " +
                            "сохранены в файл.");
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный " +
                            "пункт меню.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void ShowAll(HermitageRepository repo)
    {
        PrintList("Все картины", 
            repo.GetPaintingsView().Select(p => p.ToString()));
        PrintList("\n Художники", 
            repo.Artists.Select(a => a.ToString()));
        PrintList("\n Стили", 
            repo.Styles.Select(s => s.ToString()));
    }

    private static void PrintList(string title, 
        IEnumerable<string> items)
    {
        Console.WriteLine($"{title}");
        int count = 0;
        foreach (var item in items)
        {
            Console.WriteLine(item);
            count++;
        }
        if (count == 0) Console.WriteLine("(пусто)");
        Console.WriteLine($"Всего: {count}");
    }

    private static void DeletePainting(HermitageRepository repo)
    {
        Console.Write("Введите ID картины для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Некорректный ID.");
            return;
        }
        Console.WriteLine(repo.DeletePainting(id) ? 
            "Удалено." : "Картина не найдена.");
    }

    private static void AddPainting(HermitageRepository repo)
    {
        try
        {
            int id = repo.NextPaintingId();
            Console.Write($"Название (ID будет {id}): ");
            var title = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException
                    ("Название не может быть пустым.");

            Console.Write("ID художника: ");
            int artistId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Часть Эрмитажа: ");
            int part = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Год (Enter — пусто): ");
            var yearInput = Console.ReadLine();
            string? year = string.IsNullOrWhiteSpace(yearInput)
                ? null : yearInput.Trim();

            Console.Write("ID стиля: ");
            int styleId = int.Parse(Console.ReadLine() ?? "0");

            repo.AddPainting(new Painting
                (id, title, artistId, part, year, styleId));
            Console.WriteLine("Картина добавлена. " +
                "Не забудьте сохранить (п. 9).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка добавления: {ex.Message}");
        }
    }
}