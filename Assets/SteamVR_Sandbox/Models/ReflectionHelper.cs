using System;
using System.Collections.Generic;

namespace SteamVR_Sandbox.Models
{
    public static class ReflectionHelper
    {
        public static T GetEnumValue<T>(string key) where T : Enum
        {
            if (EnumCache<T>.Value.ContainsKey(key))
                return EnumCache<T>.Value[key];

            var value = (T) Enum.Parse(typeof(T), key);
            EnumCache<T>.Value.Add(key, value);
            return value;
        }

        private static class EnumCache<T>
        {
            public static Dictionary<string, T> Value { get; }

            static EnumCache()
            {
                Value = new Dictionary<string, T>();
            }
        }
    }
}