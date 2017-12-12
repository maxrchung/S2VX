#pragma once

#include "Command.hpp"
#include <glm/glm.hpp>

class CommandGrid_ColorBack : public Command {
public:
	CommandGrid_ColorBack(int start, int end, float startR, float startG, float startA, float startB, float endR, float endG, float endB, float endA);
	glm::vec4 startColor;
	glm::vec4 endColor;
};