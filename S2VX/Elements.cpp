#include "Elements.hpp"
namespace S2VX {
	Elements::Elements(std::unique_ptr<Back>& pBack, std::unique_ptr<Camera>& pCamera, std::unique_ptr<Grid>& pGrid, std::unique_ptr<Notes>& pNotes, std::unique_ptr<Sprites>& pSprites)
		: back{ std::move(pBack) }, camera{ std::move(pCamera) }, grid{ std::move(pGrid) }, notes{ std::move(pNotes) }, sprites{ std::move(pSprites) },
		all{ camera.get(), back.get(), sprites.get(), grid.get(), notes.get() } {}
	void Elements::draw() {
		back->draw(*camera.get());
		sprites->draw(*camera.get());
		grid->draw(*camera.get());
		notes->draw(*camera.get());
	}
	void Elements::update(int time) {
		for (auto element : all) {
			element->updateActives(time);
			element->update(time);
		}
	}
}