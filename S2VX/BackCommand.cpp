#include "BackCommand.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	BackCommand::BackCommand(Back& pBack, const int start, const int end, const EasingType easing)
		: Command{ start, end, easing },
		back{ pBack } {}
}