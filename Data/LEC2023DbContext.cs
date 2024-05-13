using Microsoft.EntityFrameworkCore;  // refer to the installed `Microsoft.EntityFrameworkCore.SqlServer` or `Microsoft.EntityFrameworkCore.Tools` from Manage NuGet Packages

using HotelRoomManagementApp.Models;  // refer to Models folder 

namespace HotelRoomManagementApp.Data
{
    public class LEC2023DbContext : DbContext
    {
        public LEC2023DbContext(DbContextOptions<LEC2023DbContext> options)
            : base(options) 
        { 
        
        }

        public DbSet<ClientsInfo> ClientsInfo { get; set; }
    
        public DbSet<RoomsInfo> RoomsInfo { get; set; }

        public DbSet<StaffLoginResponse> StaffsInfo { get; set; }
      
    }
}
