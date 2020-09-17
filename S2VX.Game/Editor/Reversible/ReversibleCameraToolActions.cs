using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleCameraToolActions : IReversible {
        private S2VXStory Story { get; set; }

        private CameraMoveCommand MoveCommand { get; set; }
        private CameraScaleCommand ScaleCommand { get; set; }
        private CameraRotateCommand RotateCommand { get; set; }

        public ReversibleCameraToolActions(
            S2VXStory story,
            CameraMoveCommand moveCommand,
            CameraScaleCommand scaleCommand,
            CameraRotateCommand rotateCommand
        ) {
            Story = story;
            MoveCommand = moveCommand;
            ScaleCommand = scaleCommand;
            RotateCommand = rotateCommand;
        }

        public void Undo() {
            if (MoveCommand != null) {
                Story.RemoveCommand(MoveCommand);
            }
            if (ScaleCommand != null) {
                Story.RemoveCommand(ScaleCommand);
            }
            if (RotateCommand != null) {
                Story.RemoveCommand(RotateCommand);
            }
        }

        public void Redo() {
            if (MoveCommand != null) {
                Story.AddCommand(MoveCommand);
            }
            if (ScaleCommand != null) {
                Story.AddCommand(ScaleCommand);
            }
            if (RotateCommand != null) {
                Story.AddCommand(RotateCommand);
            }
        }
    }
}
