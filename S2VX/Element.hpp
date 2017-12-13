#pragma once

#include "Command.hpp"
#include <memory>
#include <unordered_set>
#include <vector>

class Element {
public:
	// To preserve unique_ptr, elements are moved from pCommands to commands
	Element(std::vector<std::unique_ptr<Command>>& pCommands);
	void updateActiveCommands(const Time& time);


	virtual void update(const Time& time) = 0;
	virtual void draw() = 0;
	
	std::vector<std::unique_ptr<Command>> commands;
	std::unordered_set<int> activeCommands;
	int activeIndex = 0;

};