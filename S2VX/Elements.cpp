#include "Elements.hpp"

namespace S2VX {
	Elements::Elements(std::unique_ptr<Camera> pCamera, std::unique_ptr<Grid> pGrid, std::unique_ptr<Sprites> pSprite)
		: camera(std::move(pCamera)), grid(std::move(pGrid)), sprite(std::move(pSprite)) {}

	void Elements::update(const Time& time) {
		camera->update(time);
		grid->update(time);
		sprite->update(time);
	}

	void Elements::draw() {
		grid->draw(camera.get());
		sprite->draw(camera.get());
	}
}