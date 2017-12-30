#include "CommandsCamera.hpp"

namespace S2VX {
	CommandCameraMove::CommandCameraMove(const Time& start, const Time& end, EasingType pEasing, float startX, float startY, float endX, float endY)
		: Command{ CommandType::CommandCameraMove, ElementType::Camera, start, end },
		easing{ pEasing },
		startCoordinate{ glm::vec2(startX, startY) },
		endCoordinate{ glm::vec2(endX, endY) } {}

	CommandCameraRotate::CommandCameraRotate(const Time& start, const Time& end, EasingType pEasing, float pStartRoll, float pEndRoll)
		: Command{ CommandType::CommandCameraRotate, ElementType::Camera, start, end },
		easing{ pEasing },
		startRoll{ pStartRoll },
		endRoll{ pEndRoll } {}

	CommandCameraZoom::CommandCameraZoom(const Time& start, const Time& end, EasingType pEasing, float pStartScale, float pEndScale)
		: Command{ CommandType::CommandCameraZoom, ElementType::Camera, start, end },
		easing{ pEasing },
		startScale{ pStartScale },
		endScale{ pEndScale } {}
}