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
		void GridColorBack(const std::string& start, const std::string& end, int easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA);
		void CameraMove(const std::string& start, const std::string& end, int easing, float startX, float startY, float endX, float endY);
		void CameraRotate(const std::string& start, const std::string& end, int easing, float startDegrees, float endDegrees);

		chaiscript::ChaiScript chai;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedCommands;
	};
}