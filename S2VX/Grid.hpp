#pragma once

#include "Element.hpp"
#include <glm/glm.hpp>

class Grid : public Element {
public:
	Grid(const std::vector<Command*>& commands);
	void update(const Time& time);
	void draw();

	glm::vec4 backColor;
};