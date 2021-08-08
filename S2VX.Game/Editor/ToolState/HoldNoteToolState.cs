using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.ToolState {
    public class HoldNoteToolState : S2VXToolState {
        private Notes PreviewContainer { get; } = new Notes();
        private EditorHoldNote Preview { get; set; } = new EditorHoldNote();

        [Resolved]
        private EditorScreen Editor { get; set; } = null;

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private bool IsRecording_;
        private bool IsRecording {
            get => IsRecording_;
            set {
                IsRecording_ = value;
                Editor.EditorInfoBar.ToolDisplay.UpdateDisplay();
            }
        }

        [BackgroundDependencyLoader]
        private void Load() {
            PreviewContainer.AddNote(Preview);
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;
            Child = PreviewContainer;
        }

        public override void OnToolMouseUp(MouseUpEvent e) {
            if (!IsRecording) {
                IsRecording = true;
                return;
            }

            var endTime = Time.Current;
            if (endTime > Preview.HitTime) {
                if (e.Button == MouseButton.Left) {
                    Preview.MidCoordinates.Add(Editor.MousePosition);
                } else if (e.Button == MouseButton.Right) {
                    AddHoldNote(endTime);
                    IsRecording = !IsRecording;
                }
            }
        }

        private void AddHoldNote(double endTime) {
            var note = new EditorHoldNote {
                Coordinates = Preview.Coordinates,
                HitTime = Preview.HitTime,
                EndTime = endTime,
                EndCoordinates = Editor.MousePosition,
            };
            note.MidCoordinates.AddRange(Preview.MidCoordinates);
            Editor.Reversibles.Push(new ReversibleAddHoldNote(Story, note, Editor));
        }

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Escape:
                    if (IsRecording) {
                        IsRecording = false;
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        protected override void Update() {
            Preview.InnerColor = Story.Notes.HoldNoteColor;
            Preview.OutlineColor = Story.Notes.HoldNoteOutlineColor;
            Preview.OutlineThickness = Story.Notes.HoldNoteOutlineThickness;

            if (IsRecording) {
                Preview.EndTime = Time.Current;
                Preview.EndCoordinates = Editor.MousePosition;
            } else {
                Preview.HitTime = Time.Current;
                Preview.EndTime = Time.Current;
                Preview.Coordinates = Editor.MousePosition;
                Preview.EndCoordinates = Editor.MousePosition;
                Preview.MidCoordinates.Clear();
                Preview.MidAnchors.Clear();
            }
        }

        public override string DisplayName() =>
            IsRecording ? "Hold Note (Click to End, Esc to Cancel)" : "Hold Note (Click to Start)";
    }
}
