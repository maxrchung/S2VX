#pragma once
#include "CameraCommand.hpp"
namespace S2VX {
	class CameraZoomCommand : public CameraCommand {
	public:
		explicit CameraZoomCommand(Camera* const camera, const int start, const int end, const EasingType easing, const float pStartScale, const float pEndScale);
		void update(const float easing);
	private:
		void validateCameraZoom(const float scale) const;
		const float endScale;
		const float startScale;
	};
}