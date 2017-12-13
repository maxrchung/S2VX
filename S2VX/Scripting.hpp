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

	void init();
	// Evaluates chaiscript
	std::vector<std::unique_ptr<Element>> evaluate(std::string path);
	void GridColorBack(const Time& start, const Time& end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA);

	chaiscript::ChaiScript chai;
	std::set<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedCommands;
};