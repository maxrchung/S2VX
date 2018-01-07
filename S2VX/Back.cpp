#include "Back.hpp"
#include "BackCommands.hpp"
#include "Easing.hpp"
#include <glad/glad.h>
namespace S2VX {
	Back::Back(const std::vector<Command*>& commands)
		: Element{ commands } {}
	void Back::draw(const Camera& camera) {
		glClearColor(color.r, color.g, color.b, color.a);
		glClear(GL_COLOR_BUFFER_BIT);
	}
	void Back::update(int time) {
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::BackColor: {
					auto derived = static_cast<BackColorCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					color = glm::mix(derived->startColor, derived->endColor, easing);
					break;
				}
			}
		}
	}
}