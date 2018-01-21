#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Back : public Element {
	public:
		explicit Back(const std::vector<Command*>& commands);
		void draw(const Camera& camera);
		void update(const int time);
	private:
		glm::vec4 color = glm::vec4(0.0f, 0.0f, 0.0f, 1.0f);
	};
}