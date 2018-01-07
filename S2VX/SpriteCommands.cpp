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
	SpriteMoveCommand::SpriteMoveCommand(int start, int end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY)
		: Command{ CommandType::SpriteMove, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startCoordinate{ glm::vec2{ startX, startY } },
		endCoordinate{ glm::vec2{ endX, endY } } {
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
		startScale{ glm::vec2{ pStartScaleX, pStartScaleY } },
		endScale{ glm::vec2{ pEndScaleX, pEndScaleY } } {
		validateScale(startScale);
		validateScale(endScale);
		validateSpriteID(spriteID);
	}
}