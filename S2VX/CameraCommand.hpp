#pragma once
#include "Command.hpp"
namespace S2VX {
	class Camera;
	class CameraCommand : Command {
	public:
		explicit CameraCommand(Camera* const pCamera, const int start, const int end, const EasingType easing);
		virtual ~CameraCommand() {};
		virtual void update(const int time) = 0;
	private:
		Camera* const camera;
	};
}