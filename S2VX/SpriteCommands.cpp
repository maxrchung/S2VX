#include "SpriteCommands.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	SpriteBindCommand::SpriteBindCommand(const std::string& pPath)
		: Command{ CommandType::SpriteBind, 0, 0 },
		path{ pPath } {}
	SpriteFadeCommand::SpriteFadeCommand(const int start, const int end, const EasingType pEasing, const float pStartFade, const float pEndFade)
		: Command{ CommandType::SpriteFade, start, end },
		easing{ pEasing },
		startFade{ pStartFade },
		endFade{ pEndFade } {
		validateFade(startFade);
		validateFade(endFade);
	}
	SpriteMoveCommand::SpriteMoveCommand(const int start, const int end, const EasingType pEasing, const int pStartX, const int pStartY, const int pEndX, const int pEndY)
		: Command{ CommandType::SpriteMove, start, end },
		easing{ pEasing },
		startCoordinate{ pStartX, pStartY },
		endCoordinate{ pEndX, pEndY } {}
	SpriteRotateCommand::SpriteRotateCommand(const int start, const int end, const EasingType pEasing, const float pStartRotation, const float pEndRotation)
		: Command{ CommandType::SpriteRotate, start, end },
		easing{ pEasing },
		startRotation{ pStartRotation },
		endRotation{ pEndRotation } {}
	SpriteScaleCommand::SpriteScaleCommand(const int start, const int end, const EasingType pEasing, const float pStartScaleX, const float pStartScaleY, const float pEndScaleX, const float pEndScaleY)
		: Command{ CommandType::SpriteScale, start, end },
		easing{ pEasing },
		startScale{ glm::vec2{pStartScaleX, pStartScaleY} },
		endScale{ glm::vec2{pEndScaleX, pEndScaleY} } {
		validateScale(startScale);
		validateScale(endScale);
	}
}