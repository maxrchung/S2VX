#pragma once

#include "Element.hpp"
#include <glad/glad.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

namespace S2VX {
	class Camera : public Element {
	public:
		Camera(const std::vector<Command*>& pCommands);

		void update(const Time& time);

		void draw();

		// Camera Attributes
		glm::vec3 position = glm::vec3(0.0f, 0.0f, 1.0f);
		glm::vec3 front = glm::vec3(0.0f, 0.0f, -1.0f);
		glm::vec3 upWorld = glm::vec3(0.0f, 1.0f, 0.0f);
		glm::vec3 upLocal;
		glm::vec3 right;
		glm::mat4 view;

		float fov = 45.0f;

		// Eular Angles
		// z-axis rotation CCW x
		float yaw = -90.0f;
		// y-axis rotation CCW z
		float pitch = 0.0f;
		// x-axis rotation CCW z
		float roll = 0.0f;

	private:
		// Calculates the front vector from the Camera's (updated) Eular Angles
		// Updates the view matrix calculated using Eular Angles and the LookAt Matrix
		void updateView();
	};
}