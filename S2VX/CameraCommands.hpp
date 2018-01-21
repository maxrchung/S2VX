#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct CameraMoveCommand : Command {
		explicit CameraMoveCommand(const int start, const int end, const EasingType pEasing, const int startX, const int startY, const int endX, const int endY);
		const EasingType easing;
		const glm::vec2 startCoordinate;
		const glm::vec2 endCoordinate;
	};
	struct CameraRotateCommand : Command {
		explicit CameraRotateCommand(const int start, const int end, const EasingType pEasing, const float pStartRotation, const float pEndRotation);
		const EasingType easing;
		const float startRotation;
		const float endRotation;
	};
	struct CameraZoomCommand : Command {
	public:
		explicit CameraZoomCommand(const int start, const int end, const EasingType pEasing, const float pStartScale, const float pEndScale);
		const EasingType easing;
		const float startScale;
		const float endScale;
	private:
		void validateZoom(const float scale) const;
	};
}