#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct CameraMoveCommand : Command {
		CameraMoveCommand(int start, int end, EasingType pEasing, float startX, float startY, float endX, float endY)
			: Command{ CommandType::CameraMove, start, end },
			easing{ pEasing },
			startCoordinate{ glm::vec2(startX, startY) },
			endCoordinate{ glm::vec2(endX, endY) } {}
		EasingType easing;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
	};
	struct CameraRotateCommand : Command {
		CameraRotateCommand(int start, int end, EasingType pEasing, float pStartRoll, float pEndRoll)
			: Command{ CommandType::CameraRotate, start, end },
			easing{ pEasing },
			startRoll{ pStartRoll },
			endRoll{ pEndRoll } {}
		EasingType easing;
		float startRoll;
		float endRoll;
	};
	struct CameraZoomCommand : Command {
		CameraZoomCommand(int start, int end, EasingType pEasing, float pStartScale, float pEndScale)
			: Command{ CommandType::CameraZoom, start, end },
			easing{ pEasing },
			startScale{ pStartScale },
			endScale{ pEndScale } {}
		EasingType easing;
		float startScale;
		float endScale;
	};
}