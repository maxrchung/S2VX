#include "Notes.hpp"
namespace S2VX {
	Notes::Notes(const std::vector<Command*>& commands)
		: Element{ commands } {}
	void Notes::draw(const Camera& camera) {
	}
	void Notes::update(int time) {
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::Note: {
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
				case CommandType::SpriteMove: {
					auto derived = static_cast<SpriteMoveCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto pos = glm::mix(derived->startCoordinate, derived->endCoordinate, easing);
					auto& sprite = activeSprites[derived->spriteID];
					sprite->setPosition(pos);
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