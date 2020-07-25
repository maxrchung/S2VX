using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osuTK;

namespace S2VX.Game
{
    // Special converter for Note serialization since we only care about EndTime and Coordinates
    public class NoteConverter : JsonConverter<Note>
    {
        public override void WriteJson(JsonWriter writer, Note note, JsonSerializer serializer)
        {
            var obj = new JObject();
            obj.Add("EndTime", note.EndTime);
            var coordinates = new JObject();
            coordinates.Add("X", note.Coordinates.X);
            coordinates.Add("Y", note.Coordinates.Y);
            obj.Add("Coordinates", coordinates);
            obj.WriteTo(writer);
        }

        // Don't need to implement deserialization because the default behavior is sufficient for us
        public override Note ReadJson(JsonReader reader, Type objectType, Note existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
