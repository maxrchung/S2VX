#pragma once

#include <string>
#include <vector>

enum CommandType {
	CommandGrid_ColorBack
};

// Generic class for parameters
// The idea is that we parse a generic command down to a specific command
class CommandParameter {
public:
	CommandParameter(std::string pValue, std::vector<std::string> pValues, std::vector<CommandParameter> pChildren);
	std::string value;
	std::vector<std::string> values;
	std::vector<CommandParameter> children;
};

// Base class that all commands inherit from
class Command {
public:
	Command(CommandType pType, int pStart, int pEnd, CommandParameter pParameter);

	CommandType type;
	int start;
	int end;
	CommandParameter parameter;
};