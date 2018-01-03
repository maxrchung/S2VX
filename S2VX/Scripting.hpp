#pragma once
#include "Command.hpp"
#include "Element.hpp"
#include "Elements.hpp"
#include "EasingType.hpp"
#include "Sprites.hpp"
#include <chaiscript/chaiscript.hpp>
#include <memory>
#include <set>
#include <unordered_map>
namespace S2VX {
	class Scripting {
	public:
		// Initializing scripting
		Scripting();
		void BackColor(const std::string& start, const std::string& end, int easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA);
		void CameraMove(const std::string& start, const std::string& end, int easing, float startX, float startY, float endX, float endY);
		void CameraRotate(const std::string& start, const std::string& end, int easing, float startDegrees, float endDegrees);
		void CameraZoom(const std::string& start, const std::string& end, int easing, float startScale, float endScale);
		// Evaluates chaiscript
		Elements evaluate(const std::string& path);
		void GridSetLineWidth(const std::string& start, const std::string& end, int easing, float startThickness, float endThickness);
		void SpriteBind(const std::string& path);
		void SpriteMove(const std::string& start, const std::string& end, int easing, float startX, float startY, float endX, float endY);
	private:
		void reset();
		void resetSpriteTime();
		chaiscript::ChaiScript chai;
		int spriteID = -1;
		Time spriteStart = Time(std::numeric_limits<int>::max());
		Time spriteEnd;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedCommands;
		std::unordered_map<int, Time> spriteStarts;
		std::unordered_map<int, Time> spriteEnds;
	};
}