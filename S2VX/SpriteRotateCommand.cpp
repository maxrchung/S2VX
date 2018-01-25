#include "SpriteRotateCommand.hpp"
namespace S2VX {
	SpriteRotateCommand::SpriteRotateCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const float pStartRotation, const float pEndRotation)
		: SpriteCommand{ sprite, start, end, easing },
		startRotation{ pStartRotation },
		endRotation{ pEndRotation } {}
	void SpriteRotateCommand::update(const int time) {
	}
}