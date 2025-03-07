using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game.Convertors
{
    public class StringArrayJsonConverter : JsonConverter<List<List<string>>>
    {
        public override List<List<string>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<List<List<string>>>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, List<List<string>> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}