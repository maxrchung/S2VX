#include "Elements.hpp"
namespace S2VX {
	Elements::Elements(std::unique_ptr<Back>& pBack, std::unique_ptr<Camera>& pCamera, std::unique_ptr<Grid>& pGrid, std::unique_ptr<Sprites>& pSprites)
		: back{ std::move(pBack) }, camera{ std::move(pCamera) }, grid{ std::move(pGrid) }, sprites{ std::move(pSprites) } {}
	void Elements::draw() {
		back->draw(*camera.get());
		sprites->draw(*camera.get());
		grid->draw(*camera.get());
	}
	void Elements::update(const Time& time) {
		camera->update(time);
		back->update(time);
		sprites->update(time);
		grid->update(time);
	}
}