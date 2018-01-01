#include "Grid.hpp"

#include "Camera.hpp"
#include "GridCommands.hpp"
#include "Easing.hpp"
#include <GLFW/glfw3.h>
#include <iostream>
namespace S2VX {
	Grid::Grid(const std::vector<Command*>& commands)
		: Element{ commands } {
		glGenVertexArrays(1, &linesVertexArray);
		glGenBuffers(1, &linesVertexBuffer);
		shader = Shader(R"(c:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Line.VertexShader)", R"(c:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Line.FragmentShader)");
	}
	void Grid::draw(const Camera& camera) {
		glClearColor(backColor.r, backColor.g, backColor.b, backColor.a);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		vertices.clear();
		auto scale = camera.getScale();
		auto posX = static_cast<int>(camera.getPosition().x);
		auto posY = static_cast<int>(camera.getPosition().y);
		auto corner = 0.5f + roundf(scale);
		for (int i = 0; i < camera.getScale() * 2 + 2; ++i) {
			// Left X
			vertices.push_back(posX - corner);
			// Left Y
			auto horizontalY = posY + corner - i;
			vertices.push_back(horizontalY);

			// Right X
			vertices.push_back(posX + corner);
			// Right Y
			vertices.push_back(horizontalY);

			// Top X
			auto verticalX = posX - corner + i;
			vertices.push_back(verticalX);
			// Top Y
			vertices.push_back(posY + corner);

			// Bot X
			vertices.push_back(verticalX);
			// Bot Y
			vertices.push_back(posY - corner);
		}
		glBindBuffer(GL_ARRAY_BUFFER, linesVertexBuffer);
		// Use sizeof and size to get total size
		// Use & to get pointer to first element
		glBufferData(GL_ARRAY_BUFFER, sizeof(vertices[0]) * vertices.size(), &vertices[0], GL_STATIC_DRAW);
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		shader.setMat4("view", camera.getView());
		shader.setMat4("projection", camera.getProjection());
		shader.use();
		glBindVertexArray(linesVertexArray);
		glDrawArrays(GL_LINES, 0, vertices.size());
	}
	void Grid::update(const Time& time) {
		updateActives(time);
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time.ms - command->start.ms) / (command->end.ms - command->start.ms);
			switch (command->commandType) {
				case CommandType::GridColorBack: {
					// ? lol
					auto derived = static_cast<GridColorBackCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					backColor = glm::mix(derived->startColor, derived->endColor, easing);
					break;
				}
			}
		}
	}
}