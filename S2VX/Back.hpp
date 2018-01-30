#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Back : public Element {
	public:
		Back();
		void draw(const Camera& camera);
		void setColor(const glm::vec3& pColor) { color = pColor; }
	private:
		glm::vec3 color;
	};
}