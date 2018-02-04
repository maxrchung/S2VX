#include "Grid.hpp"
#include "Camera.hpp"
#include "Shader.hpp"
#include <glad/glad.h>
namespace S2VX {
	Grid::Grid(Camera& pCamera, Shader& pShader)
		: camera{ pCamera }, 
		color{ glm::vec3{ 1.0f, 1.0f, 1.0f } },
		fade{ 1.0f },
		feather{ 0.01f },
		shader{ pShader },
		thickness{ 0.1f } {
		glGenVertexArrays(1, &vertexArray);
		glGenBuffers(1, &vertexBuffer);
	}
	Grid::Grid(Grid&& other) 
		: camera{ other.camera },
		color{ other.color },
		fade{ other.fade },
		feather{ other.feather },
		shader{ other.shader },
		thickness{ other.thickness },
		vertexArray{ other.vertexArray },
		vertexBuffer{ other.vertexBuffer } {
		other.vertexArray = 0;
		other.vertexBuffer = 0;
	}
	Grid& Grid::operator=(Grid&& other) {
		if (this != &other) {
			color = other.color;
			fade = other.fade;
			feather = other.feather;
			thickness = other.thickness;
			vertexArray = other.vertexArray;
			vertexBuffer = other.vertexBuffer;
			other.vertexArray = 0;
			other.vertexBuffer = 0;
		}
		return *this;
	}
	Grid::~Grid() {
		glDeleteVertexArrays(1, &vertexArray);
		glDeleteBuffers(1, &vertexBuffer);
	}
	void Grid::draw() {
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
		glBindVertexArray(vertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
		glBufferData(GL_ARRAY_BUFFER, sizeof(float) * linePoints.size(), linePoints.data(), GL_DYNAMIC_DRAW);
		// Position
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		// Normal
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
		shader.use();
		shader.setVec3("color", color);
		shader.setFloat("fade", fade);
		shader.setFloat("feather", feather);
		shader.setFloat("thickness", thickness);
		shader.setMat4("projection", camera.getProjection());
		shader.setMat4("view", camera.getView());
		glDrawArrays(GL_TRIANGLES, 0, linePoints.size() / 4);
	}
}