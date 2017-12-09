#include "CommandsGrid.hpp"

CommandGrid_ColorBack::CommandGrid_ColorBack(CommandType type, int start, int end, const CommandParameter& parameter) 
	: Command(type, start, end, parameter), color(std::stof(parameter.values[0]), std::stof(parameter.values[1]), std::stof(parameter.values[2])) {}