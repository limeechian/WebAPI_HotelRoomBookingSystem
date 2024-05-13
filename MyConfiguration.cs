using HotelRoomManagementApp.Controllers;
using Humanizer;
using System;

namespace HotelRoomManagementApp
{
    public class MyConfiguration
    {
        public JwtBearerConfiguration? JwtBearer { get; set; }
        public TokenSettingsConfiguration? TokenSettings { get; set; }
    }

    public class JwtBearerConfiguration
    {
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public string? IssuerSigningKey { get; set; }
    }

    public class TokenSettingsConfiguration
    {
        public int TokenExpirationMinutes { get; set; }
    }
}