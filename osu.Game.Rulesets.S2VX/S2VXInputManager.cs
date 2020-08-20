// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using System.ComponentModel;

namespace osu.Game.Rulesets.S2VX {
    public class S2VXInputManager : RulesetInputManager<S2VXAction> {
        public S2VXInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique) {
        }
    }

    public enum S2VXAction {
        [Description("Button 1")]
        Button1,

        [Description("Button 2")]
        Button2,
    }
}
