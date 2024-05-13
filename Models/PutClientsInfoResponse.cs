using System.ComponentModel.DataAnnotations;

namespace HotelRoomManagementApp.Models
{
    public class PutClientsInfoResponse
    {
        public string? Message { get; set; }
        
		public long Id { get; set; }

        public string? ClientName { get; set; }
      
        public string? ClientIcPassport { get; set; }

        public string? ClientPhoneNumber { get; set; }

        public string? ClientEmail { get; set; }
    
        public string? ClientGender { get; set; }

        public Nullable<System.DateTime> ClientBirthDate { get; set; }

        public string? ClientAddress { get; set; }

        public Nullable<System.DateTime> CreatedAt { get; set; }

        public Nullable<long> CreatedBy { get; set; }

        public Nullable<System.DateTime> ModifiedAt { get; set; }

        public Nullable<long> ModifiedBy { get; set; }

        public short Status { get; set; }
    }
}
