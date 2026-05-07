namespace Giftify.Models;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    // cartItem refer to one product and one cart, but one product can be in many cartItems and one cart can have many cartItems
    public Product Product { get; set; } // written by mohamed mostafa till we understand the relation between cart and cartItem, we will write this line to avoid errors, but after that we will remove it and use the relation between cart and cartItem to access the product through the cartItem
    public Cart Cart { get; set; }
}
