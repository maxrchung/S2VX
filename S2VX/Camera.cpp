#include "Camera.hpp"
#include "CameraCommands.hpp"
#include "Easing.hpp"
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Camera::Camera(const std::vector<Command*>& pCommands)
		: Element{ pCommands } {
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
		up = glm::vec3{ cos(glm::radians(degrees + 90)), sin(glm::radians(degrees + 90)), 0.0f };
		updateMatrices();
	}
	void Camera::update(int time) {
		for (const auto active : actives) {
			const auto command = commands[active];
			const auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::CameraMove: {
					const auto derived = static_cast<CameraMoveCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto pos = glm::mix(derived->startCoordinate, derived->endCoordinate, easing);
					move(glm::vec3{ pos, position.z });
					break;
				}
				case CommandType::CameraRotate: {
					const auto derived = static_cast<CameraRotateCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto rotation = glm::mix(derived->startRotation, derived->endRotation, easing);
					rotateZ(rotation);
					break;
				}
				case CommandType::CameraZoom: {
					const auto derived = static_cast<CameraZoomCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto scale = glm::mix(derived->startScale, derived->endScale, easing);
					zoom(scale);
					break;
				}
			}
		}
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