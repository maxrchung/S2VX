using System;
using osu.Framework.Graphics.UserInterface;

namespace S2VX.Game.Editor.ColorPicker {

    public class HexTextBox : BasicTextBox {
        /// <summary>
        /// Only support Hex and start with `#`
        /// </summary>
        /// <param name="character">Characters should be filter</param>
        /// <returns></returns>
        protected override bool CanAddCharacter(char character) =>
            string.IsNullOrEmpty(Text) && character == '#' || Uri.IsHexDigit(character);
    }
}
