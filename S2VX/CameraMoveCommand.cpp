#include "CameraMoveCommand.hpp"
namespace S2VX {
	CameraMoveCommand::CameraMoveCommand(Camera* const camera, int start, int end, EasingType easing, int startX, int startY, int endX, int endY)
		: CameraCommand{ camera, start, end, easing },
		startCoordinate{ glm::vec2{ startX, startY } },
		endCoordinate{ glm::vec2{ endX, endY } } {}
	void CameraMoveCommand::update(const int time) {
	}
}