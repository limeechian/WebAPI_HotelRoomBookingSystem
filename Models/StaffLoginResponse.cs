using Newtonsoft.Json;

namespace HotelRoomManagementApp.Models
{
    public class StaffLoginResponse
    {
        //[JsonProperty("Id")]
        public long Id { get; set; }

        //[JsonProperty("Username")]
        public string? Username { get; set; }

        //[JsonProperty("Password")]
        public string? Password { get; set; }
    }
}
