#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Camera : public Element {
	public:
		explicit Camera() {};
		explicit Camera(const std::vector<Command*>& pCommands);
		const glm::vec3& getPosition() const { return position; }
		const glm::mat4& getProjection() const { return projection; }
		const glm::mat4& getView() const { return view; }
		float getScale() const { return scale; }
		// Camera doesn't need to draw
		void draw(const Camera& camera) {};
		// Reposition camera
		void move(const glm::vec3& pPosition);
		// Global roll CCW in degrees
		void rotateZ(const float pRoll);
		// Sets the scaling of the camera
		// Parameter is the number of squares to stretch the screen
		void zoom(const float pScale);
	private:
		// Updates the view/projection matrices
		// Might be costly to call this everytime camera changes - Need to probably monitor this
		void updateMatrices();
		const glm::vec3 front = glm::vec3{ 0.0f, 0.0f, -1.0f };
		const float fov = 90.0f;
		float scale = 3.0f;
		// z-axis rotation +x to +y
		float roll = 0.0f;
		glm::mat4 projection;
		glm::mat4 view;
		glm::vec3 position = glm::vec3{ 0.0f, 0.0f, 1.0f };
		glm::vec3 right = glm::vec3{ 1.0f, 0.0f, 0.0f };
		glm::vec3 up = glm::vec3{ 0.0f, 1.0f, 0.0f };
	};
}