namespace Giftify.Models;

public class Occasion
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public ICollection<OccasionProduct> OccasionProducts { get; set; } = new List<OccasionProduct>();
}
