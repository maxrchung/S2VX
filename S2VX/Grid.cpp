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
	}
	void Grid::draw(const Camera& camera) {
		glClearColor(backColor.r, backColor.g, backColor.b, backColor.a);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		linePoints.clear();
		auto scale = camera.getScale();
		auto posX = static_cast<int>(camera.getPosition().x);
		auto posY = static_cast<int>(camera.getPosition().y);
		auto corner = 0.5f + roundf(scale);
		for (auto i = 0; i < scale * 2 + 2; ++i) {
			// Left X
			linePoints.push_back(posX - corner);
			// Left Y
			auto horizontalY = posY + corner - i;
			linePoints.push_back(horizontalY);

			// Right X
			linePoints.push_back(posX + corner);
			// Right Y
			linePoints.push_back(horizontalY);

			// Top X
			auto verticalX = posX - corner + i;
			linePoints.push_back(verticalX);
			// Top Y
			linePoints.push_back(posY + corner);

			// Bot X
			linePoints.push_back(verticalX);
			// Bot Y
			linePoints.push_back(posY - corner);
		}
		glBindVertexArray(linesVertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, linesVertexBuffer);
		// Use sizeof and size to get total size
		// Use & to get pointer to first element
		glBufferData(GL_ARRAY_BUFFER, sizeof(linePoints[0]) * linePoints.size(), &linePoints[0], GL_DYNAMIC_DRAW);
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		shader.setMat4("view", camera.getView());
		shader.setMat4("projection", camera.getProjection());
		shader.use();
		glDrawArrays(GL_LINES, 0, linePoints.size());
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