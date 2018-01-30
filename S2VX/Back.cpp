#include "Back.hpp"
#include <glad/glad.h>
namespace S2VX {
	void Back::draw(const Camera& camera) {
		glClearColor(color.r, color.g, color.b, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT);
	}
}