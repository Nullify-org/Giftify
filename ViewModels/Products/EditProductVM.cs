namespace Giftify.ViewModels.Products;

public class EditProductVM : CreateProductVM
{
    public int Id { get; set; }
    public ICollection<ProductImageVM> ExistingImages = new List<ProductImageVM>();
}
