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
		// Camera doesn't need to draw
		void draw(Camera* camera) {};

		// Reposition camera
		void move(glm::vec3 pPosition);

		// Global roll CCW in degrees
		void rotateZ(float pRoll);

		// Sets the scaling of the camera
		// Parameter is the number of squares to stretch the screen
		void zoom(float pScale);

		glm::vec3 position = glm::vec3(0.0f, 0.0f, 1.0f);
		glm::vec3 front = glm::vec3(0.0f, 0.0f, -1.0f);
		glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f);
		glm::vec3 right = glm::vec3(1.0f, 0.0f, 0.0f);
		glm::mat4 view;
		glm::mat4 projection;

		float scale = 10.0f;
		float fov = 90.0f;

		// z-axis rotation +x to +y
		float roll = 0.0f;

	private:
		// Updates the view/projection matrices
		// Might be costly to call this everytime camera changes - Need to probably monitor this
		void updateMatrices();
	};
}