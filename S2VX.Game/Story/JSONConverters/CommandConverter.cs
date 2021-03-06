﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S2VX.Game.Story.Command;
using System;

namespace S2VX.Game.Story.JSONConverters {
    // Specially converts Command so that a Type field is added based on the Type name
    public class CommandConverter : JsonConverter<S2VXCommand> {

        public override void WriteJson(JsonWriter writer, S2VXCommand value, JsonSerializer serializer) {
            var serializedCommand = JsonConvert.SerializeObject(value);
            var obj = JObject.Parse(serializedCommand);
            var commandName = value.GetCommandName();
            obj.Add("Type", commandName);
            obj.WriteTo(writer);
        }

        public override S2VXCommand ReadJson(
            JsonReader reader,
            Type objectType,
            S2VXCommand existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) => throw new NotSupportedException();
    }
}
