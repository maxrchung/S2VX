#include "Command.hpp"

namespace S2VX {
	Command::Command(CommandType pCommandType, ElementType pElementType, const Time& pStart, const Time& pEnd)
		: commandType{ pCommandType }, elementType{ pElementType }, start{ pStart }, end{ pEnd } {}
}