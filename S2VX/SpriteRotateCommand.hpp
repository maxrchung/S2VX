#pragma once
#include "SpriteCommand.hpp"
namespace S2VX {
	class SpriteRotateCommand : public SpriteCommand {
	public:
		explicit SpriteRotateCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const float pStartRotation, const float pEndRotation);
		void update(const int time);
	private:
		const float endRotation;
		const float startRotation;
	};
}