#include "Camera.hpp"
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Camera::Camera() {
		updateMatrices();
		zoom(scale);
		// Near plane cannot be 0
		projection = glm::perspective(glm::radians(fov), 1.0f, 0.1f, 100.0f);
	}
	void Camera::move(const glm::vec3& pPosition) {
		position = pPosition;
		updateMatrices();
	}
	void Camera::rotateZ(const float degrees) {
		roll = glm::radians(degrees);
		up = glm::vec3{ cos(glm::radians(degrees + 90.0f)), sin(glm::radians(degrees + 90.0f)), 0.0f };
		updateMatrices();
	}
	void Camera::updateMatrices() {
		view = glm::lookAt(position, position + front, up);
	}
	void Camera::zoom(const float pScale) {
		// https://stackoverflow.com/questions/6653080/in-opengl-how-can-i-determine-the-bounds-of-the-view-at-a-given-depth
		const auto z = (pScale / 2.0f) / tan(glm::radians(fov / 2.0f));
		scale = pScale;
		position.z = z;
		updateMatrices();
	}
}