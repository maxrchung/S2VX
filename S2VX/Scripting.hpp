#pragma once

#include "Command.hpp"
#include <memory>
#include <set>

class Scripting {
public:
	// Initializing scripting
	Scripting();

	// Evaluates chaiscript
	std::multiset<Command> evaluate(std::string path);
};
