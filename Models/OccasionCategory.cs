namespace Giftify.Models
{
    public class OccasionCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int OccasionId { get; set; }

        public Category Category { get; set; }
        public Occasion Occasion { get; set; }  
    }
}
