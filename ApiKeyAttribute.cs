using System;

namespace HotelRoomManagementApp
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ApiKeyAttribute : System.Attribute
    {

    }
}
