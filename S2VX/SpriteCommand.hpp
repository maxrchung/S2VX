#pragma once
#include "Command.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Sprite;
	class SpriteCommand : public Command {
	public:
		explicit SpriteCommand(Sprite& pSprite, const int start, const int end, const EasingType easing);
		virtual void update(const float easing) = 0;
	protected:
		Sprite& sprite;
	};
}