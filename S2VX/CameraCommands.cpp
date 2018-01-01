#include "CameraCommands.hpp"
namespace S2VX {
	CameraMoveCommand::CameraMoveCommand(const Time& start, const Time& end, EasingType pEasing, float startX, float startY, float endX, float endY)
		: Command{ CommandType::CameraMove, ElementType::Camera, start, end },
		easing{ pEasing },
		startCoordinate{ glm::vec2(startX, startY) },
		endCoordinate{ glm::vec2(endX, endY) } {}
	CameraRotateCommand::CameraRotateCommand(const Time& start, const Time& end, EasingType pEasing, float pStartRoll, float pEndRoll)
		: Command{ CommandType::CameraRotate, ElementType::Camera, start, end },
		easing{ pEasing },
		startRoll{ pStartRoll },
		endRoll{ pEndRoll } {}
	CameraZoomCommand::CameraZoomCommand(const Time& start, const Time& end, EasingType pEasing, float pStartScale, float pEndScale)
		: Command{ CommandType::CameraZoom, ElementType::Camera, start, end },
		easing{ pEasing },
		startScale{ pStartScale },
		endScale{ pEndScale } {}
}