#pragma once
#include <memory>
namespace S2VX {
	class Sprite;
	struct SpriteComparison {
		// Sort by start time
		bool operator() (const Sprite& lhs, const Sprite& rhs) const;
	};
}