#include "CameraRotateCommand.hpp"
namespace S2VX {
	CameraRotateCommand::CameraRotateCommand(Camera* const camera, int start, int end, EasingType easing, float pStartRotate, float pEndRotate)
		: CameraCommand{ camera, start, end, easing },
		startRotation{ pStartRotate },
		endRotation{ pEndRotate } {}
	void CameraRotateCommand::update(const int time) {
	}
}