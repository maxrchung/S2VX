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
                { "Coordinates", new JObject {
                    { "x", value.Coordinates.X },
                    { "y", value.Coordinates.Y }
                } },
                { "EndCoordinates", new JObject {
                    { "x", value.EndCoordinates.X },
                    { "y", value.EndCoordinates.Y }
                } },
            };

            var midCoordinates = new JArray();
            foreach (var coordinates in value.MidCoordinates) {
                midCoordinates.Add(new JObject {
                    { "x", coordinates.X },
                    { "y", coordinates.Y }
                });
            }
            obj.Add("MidCoordinates", midCoordinates);

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
