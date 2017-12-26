#include "Camera.hpp"

#include <glm/gtc/type_ptr.hpp>

namespace S2VX {
	Camera::Camera(const std::vector<Command*>& pCommands)
		: Element{ pCommands } {
		updateMatrices();
		zoom(scale);
	}

	void Camera::update(const Time& time) {
		updateActives(time);
	}

	void Camera::move(glm::vec3 pPosition) {
		position = pPosition;
		updateMatrices();
	}

	void Camera::zoom(float pScale) {
		// https://stackoverflow.com/questions/6653080/in-opengl-how-can-i-determine-the-bounds-of-the-view-at-a-given-depth
		// Distance is always 1
		// In our camera, fov is halved
		fov = glm::degrees(atan(pScale));
		scale = pScale;
		updateMatrices();
	}

	void Camera::rotateZ(float degrees) {
		roll = glm::radians(degrees);
		up = glm::vec3(cos(glm::radians(degrees + 90)), sin(glm::radians(degrees + 90)), 0.0f);
		updateMatrices();
	}

	void Camera::updateMatrices() {
		view = glm::lookAt(position, position + front, up);
		// Near plane cannot be 0
		projection = glm::perspective(glm::radians(fov), 1.0f, 0.1f, 2.0f);
	}
}