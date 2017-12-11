#pragma once

#include "Element.hpp"

class Grid : Element {
public:
	Grid(std::vector<std::unique_ptr<Command>>& commands);
	void update(int time);
	void draw();
};