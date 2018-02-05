#pragma once
#include <memory>
namespace S2VX {
	class Sprite;
	struct SpriteUniquePointerComparison {
		// Sort by start time
		bool operator() (const std::unique_ptr<Sprite>& lhs, const std::unique_ptr<Sprite>& rhs) const;
	};
}