using Itventory.web.Entidades;

namespace Itventory.web.Models
{
    public class PeripheralModel
    {
        public int Id { get; set; }
        public int PeripheralTypeId { get; set; }
        public int PeripheralBrandId { get; set; }
        public string Model { get; set; }
        public string Series { get; set; }
        public decimal Price { get; set; }
        public Status Status { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
