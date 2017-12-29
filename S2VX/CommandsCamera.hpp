#pragma once

#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>

namespace S2VX {
	class CommandCameraMove : public Command {
	public:
		CommandCameraMove(const Time& start, const Time& end, EasingType pEasing, float startX, float startY, float endX, float endY);
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
	};

	class CommandCameraRotate : public Command {
	public:
		CommandCameraRotate(const Time& start, const Time& end, EasingType pEasing, float pStartRoll, float pEndRoll);
		float startRoll;
		float endRoll;
	};

	class CommandCameraZoom : public Command {
	public:
		CommandCameraZoom(const Time& start, const Time& end, EasingType pEasing, float pStartScale, float pEndScale);
		float startScale;
		float endScale;
	};
}