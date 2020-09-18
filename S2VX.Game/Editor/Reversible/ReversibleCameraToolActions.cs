using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleCameraToolActions : IReversible {
        private S2VXStory Story { get; set; }
        private S2VXEditor Editor { get; set; }

        private CameraMoveCommand MoveCommand { get; set; }
        private CameraScaleCommand ScaleCommand { get; set; }
        private CameraRotateCommand RotateCommand { get; set; }

        public ReversibleCameraToolActions(
            S2VXStory story,
            S2VXEditor editor,
            CameraMoveCommand moveCommand,
            CameraScaleCommand scaleCommand,
            CameraRotateCommand rotateCommand
        ) {
            Story = story;
            Editor = editor;
            MoveCommand = moveCommand;
            ScaleCommand = scaleCommand;
            RotateCommand = rotateCommand;
        }

        public void Undo() {
            if (MoveCommand != null) {
                Story.RemoveCommand(MoveCommand);
                Editor.CommandPanel.LoadCommandsList();
            }
            if (ScaleCommand != null) {
                Story.RemoveCommand(ScaleCommand);
                Editor.CommandPanel.LoadCommandsList();
            }
            if (RotateCommand != null) {
                Story.RemoveCommand(RotateCommand);
                Editor.CommandPanel.LoadCommandsList();
            }
        }

        public void Redo() {
            if (MoveCommand != null) {
                Story.AddCommand(MoveCommand);
                Editor.CommandPanel.LoadCommandsList();
            }
            if (ScaleCommand != null) {
                Story.AddCommand(ScaleCommand);
                Editor.CommandPanel.LoadCommandsList();
            }
            if (RotateCommand != null) {
                Story.AddCommand(RotateCommand);
                Editor.CommandPanel.LoadCommandsList();
            }
        }
    }
}
