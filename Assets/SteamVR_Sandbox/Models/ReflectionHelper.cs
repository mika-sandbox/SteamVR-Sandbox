using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace SteamVR_Sandbox.Models
{
    public static class ReflectionHelper
    {
        public static T GetEnumValue<T>(string key) where T : Enum
        {
            if (Caches<T>.Values.TryGetValue(key, out var existsValue))
                return existsValue;

            var value = (T) Enum.Parse(typeof(T), key);
            Caches<T>.Values.Add(key, value);

            return value;
        }

        public static string GetEnumMemberValue<T>(this T @enum) where T : Enum
        {
            var key = Enum.GetName(typeof(T), @enum);
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException();

            if (Caches<T, string>.Values.TryGetValue(key, out var existsValue))
                return existsValue;

            var value = typeof(T).GetField(key).GetCustomAttribute<EnumMemberAttribute>().Value;
            Caches<T, string>.Values.Add(key, value);

            return value;
        }

        private static class Caches<T>
        {
            public static readonly Dictionary<string, T> Values;

            static Caches()
            {
                Values = new Dictionary<string, T>();
            }
        }

        private static class Caches<T, TU>
        {
            public static readonly Dictionary<string, TU> Values;

            static Caches()
            {
                Values = new Dictionary<string, TU>();
            }
        }
    }
}