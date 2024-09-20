using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Requests
{
    public class CreateOrderRequest
    {

        [Required(ErrorMessage = "Order Name is empty")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Order Description is empty")]
        [StringLength(100, MinimumLength = 2)]
        public string Description { get; set; } = null!;
    }
}
