using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace S2VX.Game.Editor.UserInterface {
    public class TapReceptor : CompositeDrawable {
        private static Vector2 ContainerSize = new(100);
        private static Color4 DefaultColor = S2VXColorConstants.DarkBlack;
        private static Color4 HoverColor = S2VXColorConstants.LightBlack;
        private const double IndicatorTime = 200;

        public Bindable<string> TapsLabel { get; set; } = new("Taps: 0");
        public Bindable<string> BPMLabel { get; set; } = new("BPM: 0");

        public Box BackgroundBox { get; set; } = new() {
            Colour = S2VXColorConstants.DarkBlack,
            RelativeSizeAxes = Axes.Both
        };
        public Container HitIndicator { get; } = new() {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            BorderThickness = 10,
            BorderColour = S2VXColorConstants.BrickRed,
            Masking = true,
            Child = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Transparent
            }
        };

        private int TotalTaps { get; set; }
        private double StartTime { get; set; }
        private double CurrentBPM { get; set; }

        // We want to block input if ZXCVASDF is pressed so that, for example,
        // we don't repeatedly bring up the command panel with C. However, we
        // also don't want to block all inputs because then we can't Escape out
        // of the editor screen.
        private bool IsHotKeyPressed { get; set; }
        protected override bool OnKeyDown(KeyDownEvent e) {
            IsHotKeyPressed = IsHovered && e.Key is Key.Z or Key.X or Key.C or Key.V or Key.A or Key.S or Key.D or Key.F;
            return IsHotKeyPressed;
        }
        // ProcessTap() on key up instead of key down so we lessen the chance of
        // held presses, i.e. if you hold down the Z key, it will continuously
        // trigger OnKeyDown
        protected override void OnKeyUp(KeyUpEvent e) {
            if (IsHotKeyPressed) {
                ProcessTap();
            }
        }

        protected override void OnMouseUp(MouseUpEvent e) {
            if (e.Button is MouseButton.Left or MouseButton.Right) {
                ProcessTap();
            }
        }

        protected override bool OnHover(HoverEvent e) {
            BackgroundBox.Colour = HoverColor;
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e) => BackgroundBox.Colour = DefaultColor;


        private void ProcessTap() {
            TapsLabel.Value = $"Taps: {++TotalTaps}";

            var time = Time.Current;
            if (TotalTaps > 1) {
                var totalTime = time - StartTime;
                // (TotalTaps - 1) to account for the fact that we start tap
                // calculations on the 1st tap after resetting
                var averageTapInMilliseconds = totalTime / (TotalTaps - 1);
                CurrentBPM = 60000 / averageTapInMilliseconds;
                BPMLabel.Value = "BPM: " + S2VXUtils.DoubleToString(CurrentBPM, 2);
                HitIndicator.Size = Vector2.Zero;
                HitIndicator.ClearTransforms();
                HitIndicator.Size = Vector2.Zero;
                HitIndicator.ResizeTo(ContainerSize * 2, IndicatorTime);
            } else {
                StartTime = time;
            }
        }

        public void Reset() {
            TotalTaps = 0;
            BPMLabel.SetDefault();
            TapsLabel.SetDefault();
        }


        [BackgroundDependencyLoader]
        private void Load() {
            Size = ContainerSize;
            Masking = true;
            AddInternal(BackgroundBox);
            AddInternal(HitIndicator);
        }
    }
}
