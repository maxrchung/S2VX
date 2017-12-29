#include "Command.hpp"

namespace S2VX {
	Command::Command(CommandType pCommandType, ElementType pElementType, EasingType pEasingType, const Time& pStart, const Time& pEnd)
		: commandType{ pCommandType }, elementType{ pElementType }, easingType{ pEasingType }, start{ pStart }, end{ pEnd } {}
}