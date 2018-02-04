#include "CameraRotateCommand.hpp"
#include "Camera.hpp"
namespace S2VX {
	CameraRotateCommand::CameraRotateCommand(Camera& camera, int start, int end, EasingType easing, float pStartRotate, float pEndRotate)
		: CameraCommand{ camera, start, end, easing },
		startRotation{ pStartRotate },
		endRotation{ pEndRotate } {}
	void CameraRotateCommand::update(const float easing) {
		const auto rotation = glm::mix(startRotation, endRotation, easing);
		camera.rotateZ(rotation);
	}
}