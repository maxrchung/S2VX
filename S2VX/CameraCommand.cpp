#include "CameraCommand.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	CameraCommand::CameraCommand(Camera& pCamera, const int start, const int end, const EasingType easing)
		: Command{ start, end, easing},
		camera{ pCamera } {}
}