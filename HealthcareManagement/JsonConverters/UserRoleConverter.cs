using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Utils;

namespace HealthcareManagement.JsonConverters;

public class UserRoleConverter : JsonConverter<RolesEnum>
{
    public override RolesEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var roleString = reader.GetString();
        
        if (string.IsNullOrWhiteSpace(roleString))
            throw new JsonException("Role cannot be empty.");

        if (Enum.TryParse(roleString, true, out RolesEnum role))
        {
            return role;
        }

        throw new JsonException($"Invalid role: {roleString}");
    }

    public override void Write(Utf8JsonWriter writer, RolesEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}