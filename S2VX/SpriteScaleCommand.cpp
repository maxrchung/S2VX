#include "SpriteScaleCommand.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	SpriteScaleCommand::SpriteScaleCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const float pStartScaleX, const float pStartScaleY, const float pEndScaleX, const float pEndScaleY)
		: SpriteCommand{ sprite, start, end, easing },
		startScale{ glm::vec2{ pStartScaleX, pStartScaleY } },
		endScale{ glm::vec2{ pEndScaleX, pEndScaleY } } {
		validateSpriteScale(startScale);
		validateSpriteScale(endScale);
	}
	void SpriteScaleCommand::validateSpriteScale(const glm::vec2& scale) const {
		if (scale.x < 0.0f || scale.y < 0.0f) {
			auto message = std::stringstream();
			message << "Sprite scale must be >= 0 and <= 1. Given: ("
				<< std::to_string(scale.x) << ')';
			throw ScriptError(message.str());
		}
	}
	void SpriteScaleCommand::update(const int time) {
	}
}