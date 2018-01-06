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
		void BackColor(int start, int end, int easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA);
		void CameraMove(int start, int end, int easing, float startX, float startY, float endX, float endY);
		void CameraRotate(int start, int end, int easing, float startDegrees, float endDegrees);
		void CameraZoom(int start, int end, int easing, float startScale, float endScale);
		// Evaluates chaiscript
		Elements evaluate(const std::string& path);
		void GridFeather(int start, int end, int easing, float startFeather, float endFeather);
		void GridThickness(int start, int end, int easing, float startThickness, float endThickness);
		void SpriteBind(const std::string& path);
		void SpriteMove(int start, int end, int easing, float startX, float startY, float endX, float endY);
	private:
		// Converts sortedCommands to vector form
		std::vector<Command*> sortedToVector(const std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison>& sortedCommands);
		void reset();
		void resetSpriteTime();
		chaiscript::ChaiScript chai;
		int spriteID = -1;
		int spriteStart = std::numeric_limits<int>::max();
		int spriteEnd;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedBackCommands;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedCameraCommands;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedGridCommands;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedSpriteCommands;
		std::unordered_map<int, int> spriteStarts;
		std::unordered_map<int, int> spriteEnds;
	};
}