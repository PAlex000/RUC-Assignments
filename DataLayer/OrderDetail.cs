using DataLayer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class OrderDetails
{
    [Key, Column(Order = 0)]
    public int OrderId { get; set; } = 0;
    public Order Order { get; set; } = null;

    [Key, Column(Order = 1)]
    public int ProductId { get; set; } = 0;
    public Product Product { get; set; } = null;

    public double UnitPrice { get; set; } = 0.0;
    public double Quantity { get; set; } = 0.0;
    public double Discount { get; set; } = 0.0;
}
