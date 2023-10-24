namespace Webserver.Models
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public int UnitPrice { get; set; }
        public string QUantityPerUnit { get; set; }
        public int UnitsInStick { get; set; }
    }
}
