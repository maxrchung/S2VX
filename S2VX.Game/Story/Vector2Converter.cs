using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osuTK;
using System;

namespace S2VX.Game.Story {
    // Special converter for serialization since we only care about the X and Y values of Vector2
    public class Vector2Converter : JsonConverter<Vector2> {
        public override void WriteJson(JsonWriter writer, Vector2 vector2, JsonSerializer serializer) {
            var obj = new JObject {
                { "X", vector2.X },
                { "Y", vector2.Y }
            };
            obj.WriteTo(writer);
        }

        // Don't need to implement deserialization because the default behavior is sufficient for us
        public override Vector2 ReadJson(
            JsonReader reader,
            Type objectType,
            Vector2 existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) => throw new NotSupportedException();
    }
}
