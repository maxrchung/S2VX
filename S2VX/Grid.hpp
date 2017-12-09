#pragma once

#include "Element.hpp"

class Grid : Element {
public:
	Grid(const std::vector<std::unique_ptr<Command>>& commands);
	void update(int time);
	void draw();
};