#pragma once
#include "CameraCommand.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class CameraMoveCommand : public CameraCommand {
	public:
		explicit CameraMoveCommand(Camera& camera, const int start, const int end, const EasingType easing, const float startX, const float startY, const float endX, const float endY);
		void update(const float easing);
		const glm::vec2& getEndCoordinate() const { return endCoordinate; }
		const glm::vec2& getStartCoordinate() const { return startCoordinate; }
	private:
		const glm::vec2 endCoordinate;
		const glm::vec2 startCoordinate;
	};
}