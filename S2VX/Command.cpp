#include "Command.hpp"

Command::Command(CommandType pCommandType, ElementType pElementType, const Time& pStart, const Time& pEnd)
	: commandType{ pCommandType }, elementType{ pElementType }, start{ pStart }, end{ pEnd } {}