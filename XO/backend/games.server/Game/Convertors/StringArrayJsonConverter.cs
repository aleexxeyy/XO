using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game.Convertors
{
    public class StringArrayJsonConverter : JsonConverter<string[,]>
    {
        public override string[,]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = JsonSerializer.Deserialize<List<List<string>>>(ref reader, options);
            if (list == null) return null;

            int rows = list.Count;
            int cols = rows > 0 ? list[0].Count : 0;
            var result = new string[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = list[i][j];

            return result;
        }

        public override void Write(Utf8JsonWriter writer, string[,] value, JsonSerializerOptions options)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);
            var list = new List<List<string>>();

            for (int i = 0; i < rows; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < cols; j++)
                    row.Add(value[i, j]);
                list.Add(row);
            }

            JsonSerializer.Serialize(writer, list, options);
        }
    }

}
