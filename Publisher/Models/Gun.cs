namespace RabbitMQ_v6.Models;

public class Gun : IComparable
{
    public string Id { get; set; }

    public Model Model { get; set; } = new();

    public Handy Handy { get; set; }

    public Origin Origin { get; set; } = new();

    public TTC Ttc { get; set; } = new();

    public List<string> Materials { get; set; } = new List<string>
    {
        "Steel",
        "Brass",
        "Iron",
        "Aluminium"
    };

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(this, obj)) return 0;
        if (ReferenceEquals(this, null)) return -1;
        if (ReferenceEquals(obj, null)) return 1;
        return string.Compare(this.Id, (obj as Gun)!.Id, StringComparison.Ordinal);
    }
}