using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Utils
{
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string timeString = reader.GetString();
            const int timeWithoutSecondsLength = 5;

            if (timeString.Length == timeWithoutSecondsLength) // HH:mm format
            {
                timeString += ":00";
            }
            return TimeOnly.ParseExact(timeString, "HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("HH:mm:ss"));
        }
    }
}