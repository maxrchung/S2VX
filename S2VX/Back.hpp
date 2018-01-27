#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Back : public Element {
	public:
		explicit Back() {};
		explicit Back(const std::vector<Command*>& commands);
		void draw(const Camera& camera);
		void setColor(const glm::vec3& pColor) { color = pColor; }
	private:
		glm::vec3 color = glm::vec3{ 0.0f, 0.0f, 0.0f };
	};
}