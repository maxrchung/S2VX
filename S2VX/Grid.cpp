#include "Grid.hpp"
#include "Camera.hpp"
#include "GridCommands.hpp"
#include "Easing.hpp"
#include <GLFW/glfw3.h>
namespace S2VX {
	Grid::Grid(const std::vector<Command*>& commands)
		: Element{ commands } {
		glGenVertexArrays(1, &linesVertexArray);
		glGenBuffers(1, &linesVertexBuffer);
	}
	Grid::~Grid() {
		glDeleteVertexArrays(1, &linesVertexArray);
		glDeleteBuffers(1, &linesVertexBuffer);
	}
	void Grid::draw(const Camera& camera) {
		std::vector<float> linePoints;
		auto scale = camera.getScale();
		auto posX = static_cast<int>(camera.getPosition().x);
		auto posY = static_cast<int>(camera.getPosition().y);
		auto corner = 0.5f + floorf(scale);
		auto range = static_cast<int>(scale) * 2 + 2;
		for (auto i = 0; i < range; ++i) {
			// Left X
			auto leftX = posX - corner;
			// Left Y
			auto horizontalY = posY + corner - i;
			// Right X
			auto rightX = posX + corner;
			// Top triangle
			linePoints.push_back(rightX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(1.0f);
			linePoints.push_back(rightX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(-1.0f);
			linePoints.push_back(leftX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(1.0f);
			// Bot triangle
			linePoints.push_back(rightX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(-1);
			linePoints.push_back(leftX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(-1);
			linePoints.push_back(leftX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(1);
			// Top X
			auto verticalX = posX - corner + i;
			// Top Y
			auto topY = posY + corner;
			// Bot Y
			auto botY = posY - corner;
			// Top triangle
			linePoints.push_back(verticalX); linePoints.push_back(topY); linePoints.push_back(1.0f); linePoints.push_back(0.0f);
			linePoints.push_back(verticalX); linePoints.push_back(botY); linePoints.push_back(1.0f); linePoints.push_back(0.0f);
			linePoints.push_back(verticalX); linePoints.push_back(topY); linePoints.push_back(-1.0f); linePoints.push_back(0.0f);
			// Bot triangle
			linePoints.push_back(verticalX); linePoints.push_back(botY); linePoints.push_back(1.0f); linePoints.push_back(0.0f);
			linePoints.push_back(verticalX); linePoints.push_back(botY); linePoints.push_back(-1.0f); linePoints.push_back(0.0f);
			linePoints.push_back(verticalX); linePoints.push_back(topY); linePoints.push_back(-1.0f); linePoints.push_back(0.0f);
		}
		glBindVertexArray(linesVertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, linesVertexBuffer);
		// Use sizeof and size to get total size
		// Use & to get pointer to first element
		glBufferData(GL_ARRAY_BUFFER, sizeof(float) * linePoints.size(), linePoints.data(), GL_STREAM_DRAW);
		// Position
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		// Normal
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
		linesShader->use();
		linesShader->setFloat("lineWidth", lineWidth);
		linesShader->setMat4("view", camera.getView());
		linesShader->setMat4("projection", camera.getProjection());
		glDrawArrays(GL_TRIANGLES, 0, linePoints.size() / 4);
	}
	void Grid::update(const Time& time) {
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time.ms - command->start.ms) / (command->end.ms - command->start.ms);
			switch (command->commandType) {
				case CommandType::GridSetLineWidth: {
					auto derived = static_cast<GridSetLineWidthCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto pLineWidth = glm::mix(derived->startThickness, derived->endThickness, easing);
					lineWidth = pLineWidth;
					break;
				}
			}
		}
	}
}