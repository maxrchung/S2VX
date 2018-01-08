#include "Sprites.hpp"
#include "SpriteCommands.hpp"
#include "Easing.hpp"
namespace S2VX {
	Sprites::Sprites(const std::vector<Command*>& commands)
		: Element{ commands } {}
	void Sprites::draw(const Camera& camera) {
		for (auto& active : activeSprites) {
			active.second->draw(camera);
		}
	}
	void Sprites::update(int time) {
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::SpriteBind: {
					auto derived = static_cast<SpriteBindCommand*>(command);
					auto path = derived->path;
					paths[derived->spriteID] = path;
					if (textures.find(path) == textures.end()) {
						textures[path] = std::make_unique<Texture>(path);
					}
					break;
				}
				case CommandType::SpriteCreate: {
					auto derived = static_cast<SpriteCreateCommand*>(command);
					auto path = paths[derived->spriteID];
					auto texture = textures[path].get();
					activeSprites[derived->spriteID] = std::make_unique<Sprite>(texture, imageShader.get());
					break; // Christ remember to break          eEK
				}
				case CommandType::SpriteDelete: {
					auto derived = static_cast<SpriteDeleteCommand*>(command);
					activeSprites.erase(derived->spriteID);
					break;
				}
				case CommandType::SpriteFade: {
					auto derived = static_cast<SpriteFadeCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto fade = glm::mix(derived->startFade, derived->endFade, easing);
					auto& sprite = activeSprites[derived->spriteID];
					sprite->setFade(fade);
					break;
				}
				case CommandType::SpriteMoveX: {
					auto derived = static_cast<SpriteMoveXCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					// Need to cast to float or else glm::mix will return an int
					auto posX = glm::mix(static_cast<float>(derived->startX), static_cast<float>(derived->endX), easing);
					auto& sprite = activeSprites[derived->spriteID];
					sprite->setPositionX(posX);
					break;
				}
				case CommandType::SpriteMoveY: {
					auto derived = static_cast<SpriteMoveYCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto posY = glm::mix(static_cast<float>(derived->startY), static_cast<float>(derived->endY), easing);
					auto& sprite = activeSprites[derived->spriteID];
					sprite->setPositionY(posY);
					break;
				}
				case CommandType::SpriteRotate: {
					auto derived = static_cast<SpriteRotateCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto rotation = glm::mix(derived->startRotation, derived->endRotation, easing);
					auto& sprite = activeSprites[derived->spriteID];
					sprite->setRotation(rotation);
					break;
				}
				case CommandType::SpriteScale: {
					auto derived = static_cast<SpriteScaleCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto scale = glm::mix(derived->startScale, derived->endScale, easing);
					auto& sprite = activeSprites[derived->spriteID];
					sprite->setScale(scale);
					break;
				}
			}
		}
	}
}