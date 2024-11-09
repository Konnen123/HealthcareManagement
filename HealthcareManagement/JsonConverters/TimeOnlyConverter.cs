using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthcareManagement.JsonConverters;

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormatWithoutSeconds = "HH:mm";
    private const string TimeFormatWithSeconds = "HH:mm:ss";
    
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var timeString = reader.GetString();
        
        if (TimeOnly.TryParseExact(timeString, TimeFormatWithoutSeconds, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
        {
            return time;
        }
        if (TimeOnly.TryParseExact(timeString, TimeFormatWithSeconds, CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
        {
            return time;
        }

        throw new JsonException("Invalid time format. Expected format: 'HH:mm' or 'HH:mm:ss'");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(TimeFormatWithoutSeconds));
    }
}