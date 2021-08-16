using Newtonsoft.Json;
using System.IO;

namespace S2VX.Game.Story.Settings {
    public class MetadataSettings {
        public string SongTitle { get; set; } = "";
        public string SongArtist { get; set; } = "";
        public string StoryAuthor { get; set; } = "";
        public string MiscDescription { get; set; } = "";

        private static string MetadataPath { get; } = "metadata.json";
        private string StoryDirectory { get; set; }

        public static MetadataSettings Load(string storyDirectory) {
            var metadata = new MetadataSettings();
            if (storyDirectory == null) {
                return metadata;
            }

            var metadataPath = Path.Combine(storyDirectory, MetadataPath);
            if (File.Exists(metadataPath)) {
                var text = File.ReadAllText(metadataPath);
                metadata = JsonConvert.DeserializeObject<MetadataSettings>(text);
            }
            metadata.StoryDirectory = storyDirectory;
            return metadata;
        }
        public void Save() {
            var metadataPath = Path.Combine(StoryDirectory, MetadataPath);
            var contents = JsonConvert.SerializeObject(this);
            File.WriteAllText(metadataPath, contents);
        }
    }
}
