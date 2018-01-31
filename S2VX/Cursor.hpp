#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Cursor : public Element {
	public:
		Cursor() {};
		void draw(const Camera& camera);
		void update(const int time);
		void setFade(const float pFade) { fade = pFade; }
		void setScale(const float pScale) { scale = pScale; }
		void setColor(const glm::vec3& pColor) { color = pColor; }
	private:
		float fade;
		float scale;
		glm::vec3 color;
	};
}
