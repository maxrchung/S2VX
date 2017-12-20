#include "Grid.hpp"

#include "Camera.hpp"
#include "CommandsGrid.hpp"
#include "Easing.hpp"
#include <GLFW/glfw3.h>

namespace S2VX {
	Grid::Grid(const std::vector<Command*>& commands)
		: Element{ commands } {

		glGenVertexArrays(1, &VAO);
		glGenBuffers(1, &VBO);

		glBindVertexArray(VAO);
		glBindBuffer(GL_ARRAY_BUFFER, VBO);
		glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		shader = Shader(R"(c:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Line.VertexShader)", R"(c:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Line.FragmentShader)");
	}

	void Grid::update(const Time& time) {
		updateActives(time);

		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time.ms - command->start.ms) / (command->end.ms - command->start.ms);
			switch (command->commandType) {
				case CommandType::CommandGridColorBack: {
					// ? lol
					auto derived = static_cast<CommandGridColorBack*>(command);
					auto easing = interpolation = Easing(derived->easing, interpolation);
					backColor = glm::mix(derived->startColor, derived->endColor, easing);
					break;
				}
			}
		}
	}

	void Grid::draw(Camera* camera) {
		glClearColor(backColor.r, backColor.g, backColor.b, backColor.a);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		shader.use();
		glBindVertexArray(VAO);
		glDrawArrays(GL_LINES, 0, 4);
	}
}