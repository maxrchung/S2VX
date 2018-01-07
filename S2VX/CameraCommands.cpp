#include "CameraCommands.hpp"
namespace S2VX {
	CameraMoveCommand::CameraMoveCommand(int start, int end, EasingType pEasing, float startX, float startY, float endX, float endY)
		: Command{ CommandType::CameraMove, start, end },
		easing{ pEasing },
		startCoordinate{ glm::vec2(startX, startY) },
		endCoordinate{ glm::vec2(endX, endY) } {}
	CameraRotateCommand::CameraRotateCommand(int start, int end, EasingType pEasing, float pStartRoll, float pEndRoll)
		: Command{ CommandType::CameraRotate, start, end },
		easing{ pEasing },
		startRoll{ pStartRoll },
		endRoll{ pEndRoll } {}
	CameraZoomCommand::CameraZoomCommand(int start, int end, EasingType pEasing, float pStartScale, float pEndScale)
		: Command{ CommandType::CameraZoom, start, end },
		easing{ pEasing },
		startScale{ pStartScale },
		endScale{ pEndScale } {
		validateScale(startScale);
		validateScale(endScale);
	}
}