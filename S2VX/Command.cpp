#include "Command.hpp"

namespace S2VX {
	Command::Command(CommandType pCommandType, ElementType pElementType, EasingType pEasing, const Time& pStart, const Time& pEnd)
		: commandType{ pCommandType }, elementType{ pElementType }, easing{ pEasing }, start{ pStart }, end{ pEnd } {
	}
}