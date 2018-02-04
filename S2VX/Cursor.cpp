#include "Cursor.hpp"
#include "Camera.hpp"
#include "Display.hpp"
#include "Shader.hpp"
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
namespace S2VX {
	const RectanglePoints Cursor::cursor = RectanglePoints{ 0.0f, 0.0f, RectangleOrientation::Horizontal };
	Cursor::Cursor(const Camera& pCamera, const Display& pDisplay, Shader& pShader)
		: camera{ pCamera },
		color{ glm::vec3{ 1.0f, 1.0f, 1.0f }},
		fade{ 1.0f },
		feather{ 0.005f },
		scale{ 0.05f },
		shader{ pShader },
		display{ pDisplay } {
		glGenVertexArrays(1, &vertexArray);
		glGenBuffers(1, &vertexBuffer);
		glBindVertexArray(vertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
		glBufferData(GL_ARRAY_BUFFER, sizeof(float) * cursor.getPoints().size(), cursor.getPoints().data(), GL_DYNAMIC_DRAW);
		// Position
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		// Normal
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
	}
	Cursor::~Cursor() {
		glDeleteVertexArrays(1, &vertexArray);
		glDeleteBuffers(1, &vertexBuffer);
	}
	Cursor::Cursor(Cursor&& other) 
		: camera{ other.camera },
		display{ other.display },
		fade{ other.fade },
		feather{ other.feather },
		scale{ other.scale },
		color{ other.color },
		shader{ other.shader },
		vertexArray{ other.vertexArray },
		vertexBuffer{ other.vertexBuffer } {
		other.vertexArray = 0;
		other.vertexBuffer = 0;
	}
	Cursor& Cursor::operator=(Cursor&& other) {
		if (this != &other) {
			fade = other.fade;
			feather = other.feather;
			scale = other.scale;
			color = other.color;
			vertexArray = other.vertexArray;
			vertexBuffer = other.vertexBuffer;
			other.vertexArray = 0;
			other.vertexBuffer = 0;
		}
		return *this;
	}
	void Cursor::draw() {
		shader.use();
		shader.setVec3("color", color);
		shader.setFloat("fade", fade);
		shader.setFloat("feather", feather);
		shader.setFloat("thickness", scale);
		double x;
		double y;
		glfwGetCursorPos(display.getWindow(), &x, &y);
		float normalX = (static_cast<float>(x) / static_cast<float>(display.getWidth()) - 0.5f) * 2.0f;
		float normalY = -((static_cast<float>(y) / static_cast<float>(display.getWidth()) - 0.5f)) * 2.0f;
		glm::mat4 model;
		model = glm::translate(model, glm::vec3{ normalX, normalY, 0.0f });
		model = glm::rotate(model, camera.getRoll(), glm::vec3{ 0.0f, 0.0f, -1.0f });
		shader.setMat4("model", model);
		glBindVertexArray(vertexArray);
		glDrawArrays(GL_TRIANGLES, 0, cursor.getPoints().size() / 4);
	}
}