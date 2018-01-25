#include "SpriteCommand.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	SpriteCommand::SpriteCommand(Sprite* const pSprite, const int start, const int end, const EasingType easing)
		: Command{ start, end, easing },
		sprite{ pSprite } {}

}