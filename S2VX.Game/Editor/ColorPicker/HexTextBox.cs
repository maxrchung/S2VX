// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.

using osu.Framework.Graphics.UserInterface;
using System;

namespace S2VX.Game.Editor.ColorPicker {
    public class HexTextBox : BasicTextBox {
        // Disable this so that tabbing in command panel goes naturally from
        // StartTime -> EndTime -> StartValue -> EndValue -> StartTime
        public override bool CanBeTabbedTo { get; }

        /// <summary>
        /// Only support Hex and start with `#`
        /// </summary>
        /// <param name="character">Characters should be filter</param>
        /// <returns></returns>
        protected override bool CanAddCharacter(char character) =>
            string.IsNullOrEmpty(Text) && character == '#' || Uri.IsHexDigit(character);
    }
}
