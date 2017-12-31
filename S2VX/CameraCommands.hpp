#pragma once

#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>

namespace S2VX {
	class CameraMoveCommand : public Command {
	public:
		CameraMoveCommand(const Time& start, const Time& end, EasingType pEasing, float startX, float startY, float endX, float endY);
		EasingType easing;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
	};

	class CameraRotateCommand : public Command {
	public:
		CameraRotateCommand(const Time& start, const Time& end, EasingType pEasing, float pStartRoll, float pEndRoll);
		EasingType easing;
		float startRoll;
		float endRoll;
	};

	class CameraZoomCommand : public Command {
	public:
		CameraZoomCommand(const Time& start, const Time& end, EasingType pEasing, float pStartScale, float pEndScale);
		EasingType easing;
		float startScale;
		float endScale;
	};
}