using osu.Framework.Configuration;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace S2VX.Game.Configuration {
    // This is the global settings manager, settings are saved on client machines
    // This class is initialized and cached in S2VXGameBase and S2VXTestSceneTestRunner for headless tests
    [ExcludeFromDynamicCompile]
    public class S2VXConfigManager : IniConfigManager<S2VXSetting> {

        protected override void InitialiseDefaults() {
            // Gameplay
            SetDefault(S2VXSetting.HitErrorBarVisibility, false);
            SetDefault(S2VXSetting.ScoreVisibility, true);
        }

        public S2VXConfigManager(Storage storage)
            : base(storage) { }

    }

    // Lists all settings
    public enum S2VXSetting {
        HitErrorBarVisibility,
        ScoreVisibility
    }
}
