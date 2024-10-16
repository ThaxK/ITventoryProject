using Itventory.web.Entidades;

namespace WebApplication1.Model
{
    public class SubcategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public int CategoryId { get; set; }
    }
}
