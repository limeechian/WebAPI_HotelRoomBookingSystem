﻿using Swashbuckle.Swagger;

namespace HotelRoomManagementApp
{
    internal class NonBodyParameter : Parameter
    {
        public string Name { get; set; }
        public string In { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public string Type { get; set; }
    }
}