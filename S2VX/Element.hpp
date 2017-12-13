#pragma once

#include "Command.hpp"
#include <memory>
#include <unordered_set>
#include <vector>

class Element {
public:
	// To preserve unique_ptr, elements are moved from pCommands to commands
	Element(const std::vector<Command*>& pCommands);
	virtual ~Element() {};

	// Updates list of active commands
	void updateActives(const Time& time);

	virtual void update(const Time& time) = 0;
	virtual void draw() = 0;
	
	// Deciding to use raw pointers because ownership is handled in Scripting class
	std::vector<Command*> commands;
	std::unordered_set<int> actives;
	int next = 0;
};