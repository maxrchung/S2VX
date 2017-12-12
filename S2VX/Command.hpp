#pragma once

#include "Element.hpp"
#include <string>
#include <vector>

enum class CommandType {
	CommandGrid_ColorBack
};

// Base class that all commands inherit from
class Command {
public:
	Command(CommandType pCommandType, ElementType pElementType, int pStart, int pEnd);

	CommandType commandType;
	ElementType elementType;

	int start;
	int end;
};