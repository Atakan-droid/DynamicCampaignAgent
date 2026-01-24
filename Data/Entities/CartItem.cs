using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string? Name { get; set; }
        public string? SKU { get; set; }
    }
}