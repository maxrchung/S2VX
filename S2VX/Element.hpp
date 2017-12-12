#pragma once

#include "Command.hpp"
#include "Time.hpp"
#include <memory>
#include <vector>

enum class ElementType {
	Grid
};

class Element {
public:
	// To preserve unique_ptr, elements are moved from pCommands to commands
	Element(std::vector<std::unique_ptr<Command>>& pCommands);

	virtual void update(int time) = 0;
	void updateActiveCommands();
	virtual void draw() = 0;
	
	std::vector<std::unique_ptr<Command>> commands;
};