#include "SpriteComparison.hpp"
#include "Sprite.hpp"
namespace S2VX {
	bool SpriteComparison::operator() (const Sprite& lhs, const Sprite& rhs) const {
		return lhs.getStart() < rhs.getStart();
	}
}