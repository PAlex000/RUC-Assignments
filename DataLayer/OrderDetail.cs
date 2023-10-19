using DataLayer;

public class OrderDetails
{
    public int OrderId { get; set; } = 0;
    public Order Order { get; set; } = null;
    public int ProductId { get; set; } = 0;
    public Product Product { get; set; } = null;
    public double UnitPrice { get; set; } = 0.0;
    public double Quantity { get; set; } = 0.0;
    public double Discount { get; set; } = 0.0;
}
