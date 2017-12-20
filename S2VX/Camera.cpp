#include "Camera.hpp"

#include <glm/gtc/type_ptr.hpp>

namespace S2VX {
	Camera::Camera(const std::vector<Command*>& pCommands)
		: Element{ pCommands } {
		updateView();
	}

	void Camera::update(const Time& time) {
		updateActives(time);
	}

	void Camera::move(glm::vec3 pPosition) {
		position = pPosition;
		view = glm::lookAt(position, position + front, up);
	}

	void Camera::zoom(float pScale) {
		// https://stackoverflow.com/questions/6653080/in-opengl-how-can-i-determine-the-bounds-of-the-view-at-a-given-depth
		// Distance is always 1
		fov = atan(pScale) * 2;
		scale = pScale;
	}

	void Camera::rotateZ(float degrees) {
		roll = glm::radians(degrees);
		up = glm::vec3(cos(glm::radians(degrees + 90)), sin(glm::radians(degrees + 90)), 0.0f);
		view = glm::lookAt(position, position + front, up);
	}

	void Camera::updateView() {
		view = glm::lookAt(position, position + front, up);
	}
}