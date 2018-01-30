#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Camera : public Element {
	public:
		Camera();
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
		const glm::vec3 front;
		const float fov;
		float scale;
		// z-axis rotation +x to +y
		float roll;
		glm::mat4 projection;
		glm::mat4 view;
		glm::vec3 position;
		glm::vec3 right;
		glm::vec3 up;
	};
}