#pragma once
#include "Command.hpp"
namespace S2VX {
	class Camera;
	class CameraCommand : public Command {
	public:
		explicit CameraCommand(Camera* const pCamera, const int start, const int end, const EasingType easing);
		virtual ~CameraCommand() {};
		virtual void update(const float easing) = 0;
	protected:
		Camera* const camera;
	};
}