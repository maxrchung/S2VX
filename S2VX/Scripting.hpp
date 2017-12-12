#pragma once

#include "Command.hpp"
#include "Element.hpp"
#include <chaiscript/chaiscript.hpp>
#include <memory>
#include <set>

class Scripting {
public:
	// Initializing scripting
	Scripting();
	// Evaluates chaiscript
	std::vector<std::unique_ptr<Element>> evaluate(std::string path);
	void Grid_ColorBack(int start, int end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA);

	chaiscript::ChaiScript chai;
	// Instance for chaiscript
	std::unique_ptr<Scripting> instance;

	std::multiset<std::unique_ptr<Command>> sortedCommands;
};
