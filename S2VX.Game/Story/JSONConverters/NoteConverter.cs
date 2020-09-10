using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace S2VX.Game.Story.JSONConverters {
    // Special converter for Note serialization since we only care about EndTime and Coordinates
    public class NoteConverter : JsonConverter<Note> {
        public override void WriteJson(JsonWriter writer, Note note, JsonSerializer serializer) {
            var obj = new JObject {
                { "EndTime", note.EndTime }
            };
            var coordinates = new JObject {
                { "X", note.Coordinates.X },
                { "Y", note.Coordinates.Y }
            };
            obj.Add("Coordinates", coordinates);
            obj.WriteTo(writer);
        }

        // Don't need to implement deserialization because the default behavior is sufficient for us
        public override Note ReadJson(
            JsonReader reader,
            Type objectType,
            Note existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) => throw new NotSupportedException();
    }
}
