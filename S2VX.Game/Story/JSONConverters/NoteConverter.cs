﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S2VX.Game.Story.Note;
using System;

namespace S2VX.Game.Story.JSONConverters {
    // Special converter for Note serialization since we only care about HitTime and Coordinates
    public class NoteConverter : JsonConverter<S2VXNote> {
        public override void WriteJson(JsonWriter writer, S2VXNote value, JsonSerializer serializer) {
            var obj = new JObject {
                { "HitTime", value.HitTime }
            };
            var coordinates = new JObject {
                { "x", value.Coordinates.X },
                { "y", value.Coordinates.Y }
            };
            obj.Add("Coordinates", coordinates);
            obj.WriteTo(writer);
        }

        // Don't need to implement deserialization because the default behavior is sufficient for us
        public override S2VXNote ReadJson(
            JsonReader reader,
            Type objectType,
            S2VXNote existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) => throw new NotSupportedException();
    }
}
