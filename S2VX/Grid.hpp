#pragma once

#include "Element.hpp"
#include <glm/glm.hpp>

class Grid : public Element {
public:
	Grid(std::vector<std::unique_ptr<Command>>& commands);
	void update(const Time& time);
	void draw();

	glm::vec4 backColor;
};