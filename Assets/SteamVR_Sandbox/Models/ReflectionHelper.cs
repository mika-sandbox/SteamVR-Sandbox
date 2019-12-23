using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

namespace SteamVR_Sandbox.Models
{
    public static class ReflectionHelper
    {
        public static T GetEnumValue<T>(string key) where T : Enum
        {
            if (Caches<T>.Values.ContainsKey(key))
                return (T) Caches<T>.Values[key];

            var value = (T) Enum.Parse(typeof(T), key);
            Caches<T>.Values.Add(key, value);

            return value;
        }

        public static string GetEnumMemberValue<T>(this T @enum) where T : Enum
        {
            var key = Enum.GetName(typeof(T), @enum);
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException();

            if (Caches<T>.Values.ContainsKey(key))
                return (string) Caches<T>.Values[key];

            var value = typeof(T).GetField(key).GetCustomAttribute<EnumMemberAttribute>().Value;
            Caches<T>.Values.Add(key, value);

            return value;
        }

        private static class Caches<T>
        {
            public static readonly Hashtable Values;

            static Caches()
            {
                Values = new Hashtable();
            }
        }
    }
}