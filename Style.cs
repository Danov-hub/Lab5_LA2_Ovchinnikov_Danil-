namespace HermitageApp.Models;

public class Style
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Style() { }

    public Style(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => $"[{Id}] {Name}";
}