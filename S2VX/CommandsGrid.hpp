#pragma once

#include "Command.hpp"
#include <glm/glm.hpp>

class CommandGrid_ColorBack : Command {
public:
	CommandGrid_ColorBack(CommandType type, int start, int end, const CommandParameter& parameter);
	glm::vec3 color;
};