#include "SpriteCommands.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	SpriteBindCommand::SpriteBindCommand(int pSpriteID, const std::string& pPath)
		: Command{ CommandType::SpriteBind, 0, 0 },
		spriteID{ pSpriteID },
		path{ pPath } {}
	SpriteCreateCommand::SpriteCreateCommand(int start, int pSpriteID)
		: Command{ CommandType::SpriteCreate, start, start },
		spriteID{ pSpriteID } {}
	SpriteDeleteCommand::SpriteDeleteCommand(int end, int pSpriteID)
		: Command{ CommandType::SpriteDelete, end, end },
		spriteID{ pSpriteID } {}
	SpriteFadeCommand::SpriteFadeCommand(int start, int end, EasingType pEasing, int pSpriteID, float pStartFade, float pEndFade)
		: Command{ CommandType::SpriteFade, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startFade{ pStartFade },
		endFade{ pEndFade } {
		validateFade(startFade);
		validateFade(endFade);
		validateSpriteID(spriteID);
	}
	SpriteMoveXCommand::SpriteMoveXCommand(int start, int end, EasingType pEasing, int pSpriteID, int pStartX, int pEndX)
		: Command{ CommandType::SpriteMoveX, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startX{ pStartX },
		endX{ pEndX } {
		validateSpriteID(spriteID);
	}
	SpriteMoveYCommand::SpriteMoveYCommand(int start, int end, EasingType pEasing, int pSpriteID, int pStartY, int pEndY)
		: Command{ CommandType::SpriteMoveX, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startY{ pStartY },
		endY{ pEndY } {
		validateSpriteID(spriteID);
	}
	SpriteRotateCommand::SpriteRotateCommand(int start, int end, EasingType pEasing, int pSpriteID, float pStartRotation, float pEndRotation)
		: Command{ CommandType::SpriteRotate, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startRotation{ pStartRotation },
		endRotation{ pEndRotation } {
		validateSpriteID(spriteID);
	}
	SpriteScaleCommand::SpriteScaleCommand(int start, int end, EasingType pEasing, int pSpriteID, float pStartScaleX, float pStartScaleY, float pEndScaleX, float pEndScaleY)
		: Command{ CommandType::SpriteScale, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startScale{ glm::vec2{pStartScaleX, pStartScaleY} },
		endScale{ glm::vec2{pEndScaleX, pEndScaleY} } {
		validateScale(startScale);
		validateScale(endScale);
		validateSpriteID(spriteID);
	}
}