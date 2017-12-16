#include "Grid.hpp"

#include "CommandsGrid.hpp"
#include "Easing.hpp"
#include <GLFW/glfw3.h>

namespace S2VX {
	Grid::Grid(const std::vector<Command*>& commands)
		: Element{ commands } {
	}

	void Grid::update(const Time& time) {
		updateActives(time);

		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time.ms - command->start.ms) / (command->end.ms - command->start.ms);
			auto easing = interpolation = Easing(command->easing, interpolation);
			switch (command->commandType) {
				case CommandType::CommandGridColorBack: {
					// ? lol
					auto derived = static_cast<CommandGridColorBack*>(command);
					backColor = glm::mix(derived->startColor, derived->endColor, easing);
					break;
				}
			}
		}
	}

	void Grid::draw() {
		glClearColor(backColor.r, backColor.g, backColor.b, backColor.a);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	}
}