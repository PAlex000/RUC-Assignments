using DataLayer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class OrderDetails
{
    [Key, Column(Order = 0)]
    public int OrderId { get; set; }
    public Order Order { get; set; }

    [Key, Column(Order = 1)]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public double UnitPrice { get; set; }
    public double Quantity { get; set; }
    public double Discount { get; set; }
}
