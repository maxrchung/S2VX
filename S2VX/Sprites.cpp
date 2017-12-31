#include "Sprites.hpp"

#include "SpriteCommands.hpp"

namespace S2VX {
	Sprites::Sprites(const std::vector<Command*>& commands)
		: Element{ commands } {
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

					Texture* texture = nullptr;
					if (textures.find(path) != textures.end()) {
						textures[path] = std::make_unique<Texture>(path);
					}
					else {
						texture = textures[path].get();
					}

					sprites[derived->spriteID] = std::make_unique<Sprite>(texture);
					break;
				}
				case CommandType::SpriteMove: {
					auto derived = static_cast<SpriteMoveCommand*>(command);
					auto sprite = sprites[derived->spriteID].get();

					sprite->Move(derived.)

					break;
				}
			}
		}
	}

	void Sprites::draw(Camera* camera) {
	}
}