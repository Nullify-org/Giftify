namespace Giftify.ViewModels.Categories
{
    public class CategoryCreateVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }


        public string Description { get; set; }

        public bool IsActive { get; set; } = false;

       

    }
}
