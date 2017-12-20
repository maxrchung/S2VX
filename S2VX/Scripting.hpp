#pragma once

#include "Command.hpp"
#include "Element.hpp"
#include "Elements.hpp"
#include "EasingType.hpp"
#include <chaiscript/chaiscript.hpp>
#include <memory>
#include <set>

namespace S2VX {
	class Scripting {
	public:
		// Initializing scripting
		Scripting();

		void init();
		// Evaluates chaiscript
		Elements evaluate(const std::string& path);
		void GridColorBack(const std::string& start, const std::string& end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA, int easing);

		chaiscript::ChaiScript chai;
		std::set<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedCommands;
	};
}