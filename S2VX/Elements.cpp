#include "Elements.hpp"

namespace S2VX {
	Elements::Elements(std::unique_ptr<Camera> pCamera, std::unique_ptr<Grid> pGrid)
		: camera(std::move(pCamera)), grid(std::move(pGrid)) {}

	void Elements::update(const Time& time) {
		camera->update(time);
		grid->update(time);
	}

	void Elements::draw() {
		grid->draw(camera.get());
	}
}