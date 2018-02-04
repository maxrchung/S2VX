#pragma once
#include "CameraCommand.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class CameraMoveCommand : public CameraCommand {
	public:
		explicit CameraMoveCommand(Camera& camera, const int start, const int end, const EasingType easing, const int startX, const int startY, const int endX, const int endY);
		void update(const float easing);
	private:
		const glm::vec2 endCoordinate;
		const glm::vec2 startCoordinate;
	};
}