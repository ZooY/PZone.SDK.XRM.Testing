using System;

namespace PZone.Xrm.Testing
{
    public static class IntExtensions
    {
        public static Guid ToGuid(this int value)
        {
            return new Guid("00000000-0000-0000-0000"+value.ToString("000000000000"));
        }
    }
}