using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleCameraToolActions : IReversible {
        private EditorScreen Editor { get; set; }

        private CameraMoveCommand MoveCommand { get; set; }
        private CameraScaleCommand ScaleCommand { get; set; }
        private CameraRotateCommand RotateCommand { get; set; }

        public ReversibleCameraToolActions(
            EditorScreen editor,
            CameraMoveCommand moveCommand,
            CameraScaleCommand scaleCommand,
            CameraRotateCommand rotateCommand
        ) {
            Editor = editor;
            MoveCommand = moveCommand;
            ScaleCommand = scaleCommand;
            RotateCommand = rotateCommand;
        }

        public void Undo() {
            if (MoveCommand != null) {
                Editor.CommandPanel.HandleRemoveCommand(MoveCommand);
            }
            if (ScaleCommand != null) {
                Editor.CommandPanel.HandleRemoveCommand(ScaleCommand);
            }
            if (RotateCommand != null) {
                Editor.CommandPanel.HandleRemoveCommand(RotateCommand);
            }
        }

        public void Redo() {
            if (MoveCommand != null) {
                Editor.CommandPanel.HandleAddCommand(MoveCommand);
            }
            if (ScaleCommand != null) {
                Editor.CommandPanel.HandleAddCommand(ScaleCommand);
            }
            if (RotateCommand != null) {
                Editor.CommandPanel.HandleAddCommand(RotateCommand);
            }
        }
    }
}
