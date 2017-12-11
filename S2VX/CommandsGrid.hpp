#pragma once

#include "Command.hpp"
#include <glm/glm.hpp>

class Grid_ColorBack : Command {
public:
	Grid_ColorBack(CommandType type, int start, int end, const CommandParameter& parameter);
	glm::vec3 startColor;
	glm::vec3 endColor;
};