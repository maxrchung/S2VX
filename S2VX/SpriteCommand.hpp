#pragma once
#include "Command.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Sprite;
	class SpriteCommand : Command {
	public:
		explicit SpriteCommand(Sprite* const pSprite, const int start, const int end, const EasingType easing);
		virtual ~SpriteCommand() {};
		virtual void update(const int time) = 0;
	private:
		Sprite* const sprite;
	};
}