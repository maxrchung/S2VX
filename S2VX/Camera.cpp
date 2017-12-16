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

	// Camera doesn't need to draw
	void Camera::draw() {}

	void Camera::updateView() {
		glm::vec3 direction{ cos(glm::radians(yaw)) * cos(glm::radians(pitch)),
							 sin(glm::radians(pitch)),
							 sin(glm::radians(yaw)) * cos(glm::radians(pitch)) };
		front = glm::normalize(direction);

		// Recalculate the right and up vectors
		right = glm::normalize(glm::cross(front, upWorld));  // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
		upLocal = glm::normalize(glm::cross(right, front));
		view = glm::lookAt(position, position + front, upLocal);
	}
}