using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Entities
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime EntryDate { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public bool IsInvoiced { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
    }
}
