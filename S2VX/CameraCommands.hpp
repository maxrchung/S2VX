#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct CameraMoveCommand : Command {
		CameraMoveCommand(const Time& start, const Time& end, EasingType pEasing, float startX, float startY, float endX, float endY)
			: Command{ CommandType::CameraMove, ElementType::Camera, start, end },
			easing{ pEasing },
			startCoordinate{ glm::vec2(startX, startY) },
			endCoordinate{ glm::vec2(endX, endY) } {}
		EasingType easing;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
	};
	struct CameraRotateCommand : Command {
		CameraRotateCommand(const Time& start, const Time& end, EasingType pEasing, float pStartRoll, float pEndRoll)
			: Command{ CommandType::CameraRotate, ElementType::Camera, start, end },
			easing{ pEasing },
			startRoll{ pStartRoll },
			endRoll{ pEndRoll } {}
		EasingType easing;
		float startRoll;
		float endRoll;
	};
	struct CameraZoomCommand : Command {
		CameraZoomCommand(const Time& start, const Time& end, EasingType pEasing, float pStartScale, float pEndScale)
			: Command{ CommandType::CameraZoom, ElementType::Camera, start, end },
			easing{ pEasing },
			startScale{ pStartScale },
			endScale{ pEndScale } {}
		EasingType easing;
		float startScale;
		float endScale;
	};
}