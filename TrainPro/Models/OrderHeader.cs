using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainPro.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpPhoneNumber { get; set; }
        [Required]
        public string PickUpEmail { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public AppUser User { get; set; }
        public string OrderTotal { get; set; }

        public DateTime OrderDate { get; set; }
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
