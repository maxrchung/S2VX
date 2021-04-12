using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S2VX.Game.Story.Note;
using System;

namespace S2VX.Game.Story.JSONConverters {
    public class HoldNoteConverter : JsonConverter<HoldNote> {
        public override void WriteJson(JsonWriter writer, HoldNote value, JsonSerializer serializer) {
            var obj = new JObject {
                { "HitTime", value.HitTime },
                { "EndTime", value.EndTime },
            };
            var coordinates = new JObject {
                { "x", value.Coordinates.X },
                { "y", value.Coordinates.Y }
            };
            obj.Add("Coordinates", coordinates);

            var endCoordinates = new JObject {
                { "x", value.EndCoordinates.X },
                { "y", value.EndCoordinates.Y }
            };
            obj.Add("EndCoordinates", endCoordinates);
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
