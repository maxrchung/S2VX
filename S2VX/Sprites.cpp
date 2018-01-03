#include "Sprites.hpp"
#include "SpriteCommands.hpp"
#include "Easing.hpp"
#include <iostream>
namespace S2VX {
	Sprites::Sprites(const std::vector<Command*>& commands)
		: Element{ commands } {
		// TODO: Assign default block to texture
	}
	void Sprites::draw(const Camera& camera) {
		for (auto& active : activeSprites) {
			active.second.draw(camera);
		}
	}
	void Sprites::update(const Time& time) {
		updateActives(time);
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time.ms - command->start.ms) / (command->end.ms - command->start.ms);
			switch (command->commandType) {
				case CommandType::SpriteBind: {
					auto derived = static_cast<SpriteBindCommand*>(command);
					auto path = derived->path;
					paths[derived->spriteID] = path;
					Texture texture;
					if (textures.find(path) == textures.end()) {
						texture = textures[path] = Texture(path);
					}
					else {
						texture = textures[path];
					}
					break;
				}
				case CommandType::SpriteCreate: {
					auto derived = static_cast<SpriteCreateCommand*>(command);
					auto path = paths[derived->spriteID];
					auto texture = textures[path];
					activeSprites[derived->spriteID] = Sprite(texture);
					break;
				}
				case CommandType::SpriteDelete: {
					auto derived = static_cast<SpriteDeleteCommand*>(command);
					activeSprites.erase(derived->spriteID);
					break;
				}
				case CommandType::SpriteMove: {
					auto derived = static_cast<SpriteMoveCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto pos = glm::mix(derived->startCoordinate, derived->endCoordinate, easing);
					auto& sprite = activeSprites[derived->spriteID];
					sprite.move(pos);
					break;
				}
			}
		}
	}
}