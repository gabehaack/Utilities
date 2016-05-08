using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GHaack.Utilities
{
    public static class ObjectCloner
    {
        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialisation method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T CloneObject<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            var serialized = JsonConvert.SerializeObject(source);
            var clone = JsonConvert.DeserializeObject<T>(serialized, new VersionConverter());
            return clone;
        }
    }

    public class VersionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // default serialization
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // create a new Version instance and pass the properties to the constructor
            var dict = serializer.Deserialize<Dictionary<string, int>>(reader);
            int major = dict["Major"];
            int minor = dict["Minor"];
            int build = dict["Build"];
            int revision = dict["Revision"];

            if (revision < 0)
            {
                if (build < 0)
                {
                    return new Version(major, minor);
                }
                return new Version(major, minor, build);
            }
            return new Version(major, minor, build, revision);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Version);
        }
    }
}
