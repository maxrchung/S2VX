using osu.Framework.Configuration;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace S2VX.Game.Configuration {
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

    public enum S2VXSetting {
        HitErrorBarVisibility,
        ScoreVisibility
    }
}
