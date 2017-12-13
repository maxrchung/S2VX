#include "Grid.hpp"

#include "CommandsGrid.hpp"
#include <GLFW/glfw3.h>

Grid::Grid(const std::vector<Command*>& commands) 
	: Element{ commands } {}

void Grid::update(const Time& time) {
	updateActives(time);

	for (auto active : actives) {
		auto command = commands[active];
		auto factor = static_cast<float>(time.ms - command->start.ms) / (command->end.ms - command->start.ms);
		switch (command->commandType) {
			case CommandType::CommandGridColorBack: {
				// ? lol
				auto derived = dynamic_cast<CommandGridColorBack*>(command);
				backColor = glm::mix(derived->startColor, derived->endColor, factor);
				break;
			}
		}
	}
}

void Grid::draw() {
	glClearColor(backColor.r, backColor.g, backColor.b, backColor.a);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}