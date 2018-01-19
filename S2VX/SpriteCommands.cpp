#include "SpriteCommands.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	SpriteBindCommand::SpriteBindCommand(const std::string& pPath)
		: Command{ CommandType::SpriteBind, 0, 0 },
		path{ pPath } {}
	SpriteFadeCommand::SpriteFadeCommand(int start, int end, EasingType pEasing, float pStartFade, float pEndFade)
		: Command{ CommandType::SpriteFade, start, end },
		easing{ pEasing },
		startFade{ pStartFade },
		endFade{ pEndFade } {
		validateFade(startFade);
		validateFade(endFade);
	}
	SpriteMoveXCommand::SpriteMoveXCommand(int start, int end, EasingType pEasing, int pStartX, int pEndX)
		: Command{ CommandType::SpriteMoveX, start, end },
		easing{ pEasing },
		startX{ pStartX },
		endX{ pEndX } {}
	SpriteMoveYCommand::SpriteMoveYCommand(int start, int end, EasingType pEasing, int pStartY, int pEndY)
		: Command{ CommandType::SpriteMoveX, start, end },
		easing{ pEasing },
		startY{ pStartY },
		endY{ pEndY } {}
	SpriteRotateCommand::SpriteRotateCommand(int start, int end, EasingType pEasing, float pStartRotation, float pEndRotation)
		: Command{ CommandType::SpriteRotate, start, end },
		easing{ pEasing },
		startRotation{ pStartRotation },
		endRotation{ pEndRotation } {}
	SpriteScaleCommand::SpriteScaleCommand(int start, int end, EasingType pEasing, float pStartScaleX, float pStartScaleY, float pEndScaleX, float pEndScaleY)
		: Command{ CommandType::SpriteScale, start, end },
		easing{ pEasing },
		startScale{ glm::vec2{pStartScaleX, pStartScaleY} },
		endScale{ glm::vec2{pEndScaleX, pEndScaleY} } {
		validateScale(startScale);
		validateScale(endScale);
	}
}