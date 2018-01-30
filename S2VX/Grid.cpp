#include "Grid.hpp"
#include "Camera.hpp"
#include "Shader.hpp"
#include <glad/glad.h>
namespace S2VX {
	Grid::Grid(Shader* const pLineShader)
		: lineShader{ pLineShader } {
		glGenVertexArrays(1, &linesVertexArray);
		glGenBuffers(1, &linesVertexBuffer);
	}
	Grid::~Grid() {
		glDeleteVertexArrays(1, &linesVertexArray);
		glDeleteBuffers(1, &linesVertexBuffer);
	}
	void Grid::draw(const Camera& camera) {
		std::vector<float> linePoints;
		// ceilf is needed to fix issues with lines disappearing at edges
		const auto scale = ceilf(camera.getScale());
		const auto posX = static_cast<int>(camera.getPosition().x);
		const auto posY = static_cast<int>(camera.getPosition().y);
		const auto corner = 0.5f + scale;
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
		lineShader->setVec3("color", color);
		lineShader->setFloat("fade", fade);
		lineShader->setFloat("feather", feather);
		lineShader->setFloat("thickness", thickness);
		lineShader->setMat4("projection", camera.getProjection());
		lineShader->setMat4("view", camera.getView());
		glDrawArrays(GL_TRIANGLES, 0, linePoints.size() / 4);
	}
}