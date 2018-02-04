#include "Sprite.hpp"
#include "Camera.hpp"
#include "Command.hpp"
#include "Shader.hpp"
#include "Texture.hpp"
#include <algorithm>
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Sprite::Sprite(Camera& pCamera, const Texture& pTexture, Shader& pShader)
		: camera{ pCamera }, 
		color{ glm::vec3{ 1.0f, 1.0f, 1.0f }},
		fade{ 1.0f },
		shader{ pShader },
		position{ glm::vec2{ 0.0f, 0.0f }},
		rotation{ 0.0f },
		scale{ glm::vec2{ 1.0f, 1.0f} },
		texture{ pTexture } {
		glGenVertexArrays(1, &vertexArray);
		glGenBuffers(1, &vertexBuffer);
		glGenBuffers(1, &elementBuffer);
		glBindVertexArray(vertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
		glBufferData(GL_ARRAY_BUFFER, sizeof(corners), corners, GL_STATIC_DRAW);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, elementBuffer);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(cornerIndices), cornerIndices, GL_STATIC_DRAW);
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
		shader.use();
		shader.setInt("image", 0);
	}
	Sprite::~Sprite() {
		glDeleteVertexArrays(1, &vertexArray);
		glDeleteBuffers(1, &vertexBuffer);
		glDeleteBuffers(1, &elementBuffer);
	}
	Sprite::Sprite(Sprite&& other) 
		: camera{ other.camera },
		color{ other.color },
		elementBuffer{ other.elementBuffer },
		end{ other.end },
		fade{ other.fade },
		position{ other.position },
		rotation{ other.rotation },
		scale{ other.scale },
		shader{ other.shader },
		start{ other.start },
		texture{ other.texture },
		vertexArray{ other.vertexArray },
		vertexBuffer{ other.vertexBuffer } {
		other.elementBuffer = 0;
		other.vertexArray = 0;
		other.vertexBuffer = 0;
	}
	Sprite& Sprite::operator=(Sprite&& other) {
		if (this != &other) {
			color = other.color;
			elementBuffer = other.elementBuffer;
			end = other.end;
			fade = other.fade;
			position = other.position;
			rotation = other.rotation;
			scale = other.scale;
			start = other.start;
			vertexArray = other.vertexArray;
			vertexBuffer = other.vertexBuffer;
			other.elementBuffer = 0;
			other.vertexArray = 0;
			other.vertexBuffer = 0;
		}
		return *this;
	}
	void Sprite::draw() {
		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_2D, texture.getImageTexture());
		shader.use();
		glm::mat4 model;
		model = glm::translate(model, glm::vec3{ position.x, position.y, 0.0f });
		model = glm::scale(model, glm::vec3{ scale, 1.0f });
		model = glm::rotate(model, glm::radians(rotation), glm::vec3{ 0.0f, 0.0f, 1.0f });
		shader.setVec3("color", color);
		shader.setMat4("model", model);
		shader.setMat4("view", camera.getView());
		shader.setMat4("projection", camera.getProjection());
		shader.setFloat("fade", fade);
		glBindVertexArray(vertexArray);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
	}
	void Sprite::sort() {
		// Set start and ends for each sprite
		int pStart = std::numeric_limits<int>::max();
		int pEnd = std::numeric_limits<int>::lowest();
		for (auto& command : commands) {
			if (command->getStart() < pStart) {
				pStart = command->getStart();
			}
			else if (command->getEnd() > pEnd) {
				pEnd = command->getEnd();
			}
		}
		start = pStart;
		end = pEnd;
		std::sort(commands.begin(), commands.end(), comparison);
	}
}