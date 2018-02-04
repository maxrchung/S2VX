#include "CameraMoveCommand.hpp"
#include "Camera.hpp"
namespace S2VX {
	CameraMoveCommand::CameraMoveCommand(Camera& camera, int start, int end, EasingType easing, int startX, int startY, int endX, int endY)
		: CameraCommand{ camera, start, end, easing },
		startCoordinate{ glm::vec2{ startX, startY } },
		endCoordinate{ glm::vec2{ endX, endY } } {}
	void CameraMoveCommand::update(const float easing) {
		const auto position = glm::mix(startCoordinate, endCoordinate, easing);
		const auto cameraPosition = camera.getPosition();
		camera.move(glm::vec3{ position, cameraPosition.z });
	}
}