#pragma once
#include "CameraCommand.hpp"
namespace S2VX {
	class CameraRotateCommand : public CameraCommand {
	public:
		explicit CameraRotateCommand(Camera* const camera, const int start, const int end, const EasingType easing, const float pStartRotation, const float pEndRotation);
		void update(const int time);
	private:
		const float endRotation;
		const float startRotation;
	};
}