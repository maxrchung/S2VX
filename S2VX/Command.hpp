#pragma once

#include "CommandType.hpp"
#include "ElementType.hpp"
#include <memory>
#include <string>
#include <vector>

// Base class that all commands inherit from
class Command {
public:
	Command(CommandType pCommandType, ElementType pElementType, int pStart, int pEnd);

	CommandType commandType;
	ElementType elementType;

	int start;
	int end;
};

class CommandUniquePointerComparison {
public:
	bool operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) {
		return lhs->start < rhs->start;
	}
};