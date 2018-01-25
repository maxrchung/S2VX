#include "SpriteMoveCommand.hpp"
namespace S2VX {
	SpriteMoveCommand::SpriteMoveCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const int pStartX, const int pStartY, const int pEndX, const int pEndY)
		: SpriteCommand{ sprite, start, end, easing },
		startCoordinate{ pStartX, pStartY },
		endCoordinate{ pEndX, pEndY } {}
	void SpriteMoveCommand::update(const int time) {
	}
}