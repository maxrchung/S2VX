using Newtonsoft.Json;
using System.IO;

namespace S2VX.Game.Story.Settings {
    public class MetadataSettings {
        public string SongTitle { get; set; }
        public string SongArtist { get; set; }
        public string StoryAuthor { get; set; }
        public string MiscDescription { get; set; }

        private static string MetadataPath { get; } = "metadata.json";
        public static MetadataSettings Load(string storyDirectory) {
            var metadataPath = Path.Combine(storyDirectory, MetadataPath);
            var metadata = new MetadataSettings();
            if (File.Exists(metadataPath)) {
                var text = File.ReadAllText(metadataPath);
                metadata = JsonConvert.DeserializeObject<MetadataSettings>(text);
            }
            return metadata;
        }
        public void Save(string storyDirectory) {
            var metadataPath = Path.Combine(storyDirectory, MetadataPath);
            var contents = JsonConvert.SerializeObject(this);
            File.WriteAllText(metadataPath, contents);
        }
    }
}
