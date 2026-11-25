namespace HermitageApp.Models;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Artist() { }

    public Artist(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => $"[{Id}] {Name}";
}