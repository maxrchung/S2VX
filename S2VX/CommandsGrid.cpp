#include "CommandsGrid.hpp"

Grid_ColorBack::Grid_ColorBack(CommandType type, int start, int end, const CommandParameter& parameter) 
	: Command{ type, start, end, parameter },
	startColor{	std::stof(parameter.values[0]), std::stof(parameter.values[1]), std::stof(parameter.values[2]) },
	endColor{ std::stof(parameter.values[3]), std::stof(parameter.values[4]), std::stof(parameter.values[5]) } {}