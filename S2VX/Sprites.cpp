#include "Sprites.hpp"
#include "SpriteCommands.hpp"
#include "Easing.hpp"
namespace S2VX {
	Sprites::Sprites(const std::vector<Command*>& commands)
		: Element{ commands } {
		// TODO: Assign default block to texture
	}
	void Sprites::draw(Camera* camera) {
		for (auto& active : activeSprites) {
			active.second.draw();
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
					if (textures.find(path) != textures.end()) {
						textures[path] = Texture(path);
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
				}
				case CommandType::SpriteDelete: {
					auto derived = static_cast<SpriteDeleteCommand*>(command);
					activeSprites.erase(derived->spriteID);
				}
				case CommandType::SpriteMove: {
					auto derived = static_cast<SpriteMoveCommand*>(command);
					auto sprite = activeSprites[derived->spriteID];
					auto easing = Easing(derived->easing, interpolation);
					auto pos = glm::mix(derived->startCoordinate, derived->endCoordinate, easing);
					sprite.move(pos);
					break;
				}
			}
		}
	}

}