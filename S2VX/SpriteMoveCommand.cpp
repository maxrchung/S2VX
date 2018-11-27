#include "SpriteMoveCommand.hpp"
#include "Sprite.hpp"
namespace S2VX {
	SpriteMoveCommand::SpriteMoveCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const float pStartX, const float pStartY, const float pEndX, const float pEndY)
		: SpriteCommand{ sprite, start, end, easing },
		startCoordinate{ pStartX, pStartY },
		endCoordinate{ pEndX, pEndY } {}
	void SpriteMoveCommand::update(const float easing) {
		const auto coordinate = glm::mix(startCoordinate, endCoordinate, easing);
		sprite->setPosition(coordinate);
	}
}