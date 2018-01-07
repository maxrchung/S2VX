#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct CameraMoveCommand : Command {
		CameraMoveCommand(int start, int end, EasingType pEasing, float startX, float startY, float endX, float endY);
		EasingType easing;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
	};
	struct CameraRotateCommand : Command {
		CameraRotateCommand(int start, int end, EasingType pEasing, float pStartRotation, float pEndRotation);
		EasingType easing;
		float startRotation;
		float endRotation;
	};
	struct CameraZoomCommand : Command {
		CameraZoomCommand(int start, int end, EasingType pEasing, float pStartScale, float pEndScale);
		EasingType easing;
		float startScale;
		float endScale;
	};
}