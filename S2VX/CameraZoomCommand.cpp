#include "CameraZoomCommand.hpp"
#include "Camera.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	CameraZoomCommand::CameraZoomCommand(Camera* const camera, int start, int end, EasingType easing, float pStartScale, float pEndScale)
		: CameraCommand{ camera, start, end, easing },
		startScale{ pStartScale },
		endScale{ pEndScale } {
		validateCameraZoom(pStartScale);
		validateCameraZoom(pEndScale);
	}
	void CameraZoomCommand::validateCameraZoom(const float scale) const {
		if (scale < 1.0f) {
			throw ScriptError("Camera zoom must be >= 1. Given: " + std::to_string(scale));
		}
	}
	void CameraZoomCommand::update(const float easing) {
		const auto scale = glm::mix(startScale, endScale, easing);
		camera->zoom(scale);
	}
}