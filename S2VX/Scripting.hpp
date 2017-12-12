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
	void GridColorBack(int start, int end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA);
	void Test();


	chaiscript::ChaiScript chai;
	std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedCommands;
};