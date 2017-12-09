#pragma once

#include "Command.hpp"
#include "Time.hpp"
#include <memory>
#include <vector>

class Element {
public:
	Element(std::vector<std::unique_ptr<Command>> pCommands);

	virtual void update(int time) = 0;
	virtual void draw() = 0;
	
	std::vector<std::unique_ptr<Command>> commands;
};