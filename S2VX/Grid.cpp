#include "Grid.hpp"
#include "Camera.hpp"
#include "GridCommands.hpp"
#include "Easing.hpp"
#include <GLFW/glfw3.h>
namespace S2VX {
	Grid::Grid(const std::vector<Command*>& commands, Shader* const pLineShader)
		: Element{ commands }, lineShader{ pLineShader } {
		glGenVertexArrays(1, &linesVertexArray);
		glGenBuffers(1, &linesVertexBuffer);
	}
	Grid::~Grid() {
		glDeleteVertexArrays(1, &linesVertexArray);
		glDeleteBuffers(1, &linesVertexBuffer);
	}
	void Grid::draw(const Camera& camera) {
		std::vector<float> linePoints;
		const auto scale = camera.getScale();
		const auto posX = static_cast<int>(camera.getPosition().x);
		const auto posY = static_cast<int>(camera.getPosition().y);
		const auto corner = 0.5f + floorf(scale);
		const auto range = static_cast<int>(scale) * 2 + 2;
		for (auto i = 0; i < range; ++i) {
			// Left X
			const auto leftX = posX - corner;
			// Left Y
			const auto horizontalY = posY + corner - i;
			// Right X
			const auto rightX = posX + corner;
			// Top triangle
			linePoints.push_back(rightX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(1.0f);
			linePoints.push_back(rightX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(-1.0f);
			linePoints.push_back(leftX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(1.0f);
			// Bot triangle
			linePoints.push_back(rightX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(-1.0f);
			linePoints.push_back(leftX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(-1.0f);
			linePoints.push_back(leftX); linePoints.push_back(horizontalY); linePoints.push_back(0.0f); linePoints.push_back(1.0f);
			// Top X
			const auto verticalX = posX - corner + i;
			// Top Y
			const auto topY = posY + corner;
			// Bot Y
			const auto botY = posY - corner;
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
		glBufferData(GL_ARRAY_BUFFER, sizeof(float) * linePoints.size(), linePoints.data(), GL_DYNAMIC_DRAW);
		// Position
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		// Normal
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
		lineShader->use();
		lineShader->setFloat("fade", 1.0f);
		lineShader->setFloat("feather", feather);
		lineShader->setFloat("lineWidth", lineWidth);
		lineShader->setMat4("projection", camera.getProjection());
		lineShader->setMat4("view", camera.getView());
		glDrawArrays(GL_TRIANGLES, 0, linePoints.size() / 4);
	}
	void Grid::update(int time) {
		for (const auto active : actives) {
			const auto command = commands[active];
			const auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::GridFeather: {
					const auto derived = static_cast<GridFeatherCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto pFeather = glm::mix(derived->startFeather, derived->endFeather, easing);
					feather = pFeather;
					break;
				}
				case CommandType::GridThickness: {
					const auto derived = static_cast<GridThicknessCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto pLineWidth = glm::mix(derived->startThickness, derived->endThickness, easing);
					lineWidth = pLineWidth;
					break;
				}
			}
		}
	}
}