

using System.ComponentModel.DataAnnotations;

namespace HotelRoomManagementApp.Models
{
    public class PostClientsInfoRequest
    {
        [Required]
        public string? ClientName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9-]*$", ErrorMessage = "Please enter a valid value containing only numeric digits and '-' character.")]
        public string? ClientIcPassport { get; set; }

        [Required]
        [RegularExpression(@"^[0-9-]*$", ErrorMessage = "Please enter a valid value containing only numeric digits and '-' character.")]
        public string? ClientPhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", ErrorMessage = "Please enter a valid email address.")]
        public string? ClientEmail { get; set; }
        [Required]
        public string? ClientGender { get; set; }

        [Required]
        public Nullable<System.DateTime> ClientBirthDate { get; set; }

        [Required]
        public string? ClientAddress { get; set; }

        public Nullable<long> CreatedBy { get; set; }
    }
}
