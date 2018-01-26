#include "SpriteUniquePointerComparison.hpp"
#include "Sprite.hpp"
namespace S2VX {
	bool SpriteUniquePointerComparison::operator() (const std::unique_ptr<Sprite>& lhs, const std::unique_ptr<Sprite>& rhs) const {
		return lhs->getStart() < rhs->getStart();
	}
}