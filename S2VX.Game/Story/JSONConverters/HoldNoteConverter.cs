using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S2VX.Game.Story.Note;
using System;

namespace S2VX.Game.Story.JSONConverters {
    public class HoldNoteConverter : JsonConverter<HoldNote> {
        public override void WriteJson(JsonWriter writer, HoldNote note, JsonSerializer serializer) {
            var obj = new JObject {
                { "HitTime", note.HitTime },
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
        public override HoldNote ReadJson(
            JsonReader reader,
            Type objectType,
            HoldNote existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) => throw new NotSupportedException();
    }
}
