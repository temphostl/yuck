using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace yuck;

public class DescriptionConverter : JsonConverter<Description>
{
    public override Description? ReadJson(JsonReader reader, Type objectType, Description? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            return new Description { Text = (string?)reader.Value };
        }

        if (reader.TokenType == JsonToken.StartObject)
        {
            var obj = JObject.Load(reader);
            return new Description { Text = (string?)obj["text"] };
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, Description? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.Text);
    }
}
