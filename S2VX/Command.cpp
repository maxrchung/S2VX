#include "Command.hpp"

Command::Command(CommandType pCommandType, ElementType pElementType, int pStart, int pEnd)
	: commandType{ pCommandType }, elementType{ pElementType }, start{ pStart }, end{ pEnd } {}