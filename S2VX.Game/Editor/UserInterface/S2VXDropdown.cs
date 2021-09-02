using osu.Framework.Graphics.UserInterface;

namespace S2VX.Game.Editor.UserInterface {
    public class S2VXDropdown<T> : BasicDropdown<T> {

        //protected override DropdownHeader CreateHeader() => new OsuDropdownHeader();

        protected override DropdownMenu CreateMenu() => new S2VXDropdownMenu();

        protected class S2VXDropdownMenu : BasicDropdownMenu {
            public S2VXDropdownMenu() => MaxHeight = 300;
        }
    }
}
