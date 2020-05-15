using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ClearScript;

namespace ZoneServer
{
    public static class ScriptObjectExtension
    {
        public static bool TryGetProperty<PropertyType>(this ScriptObject obj, string name, ref PropertyType value)
        {
            object property = obj.GetProperty(name);
            if (property.GetType() != typeof(Undefined))
            {
                value = (PropertyType)property;
                return true;
            }
            return false;
        }
        public static bool TryGetProperty(this ScriptObject obj, string name, ref byte value)
        {
            object property = obj.GetProperty(name);
            if (property.GetType() != typeof(Undefined))
            {
                value = (byte)(int)property;
                return true;
            }
            return false;
        }
    }
}
