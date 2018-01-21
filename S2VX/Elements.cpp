#include "Elements.hpp"
namespace S2VX {
	Elements::Elements(const std::vector<Command*>& backCommands,
					   const std::vector<Command*>& cameraCommands,
					   const std::vector<Command*>& gridCommands, Shader* const lineShader,
					   const std::vector<Note*>& pNotes,
					   const std::vector<Sprite*>& pSprites)
		: back{ std::make_unique<Back>(backCommands) },
		camera{ std::make_unique<Camera>(cameraCommands) },
		grid{ std::make_unique<Grid>(gridCommands, lineShader) },
		notes{ std::make_unique<Notes>(pNotes) },
		sprites{ std::make_unique<Sprites>(pSprites) },
		all{ camera.get(), back.get(), sprites.get(), grid.get(), notes.get() } {}
	void Elements::draw() {
		back->draw(*camera.get());
		sprites->draw(*camera.get());
		grid->draw(*camera.get());
		notes->draw(*camera.get());
	}
	void Elements::update(const int time) {
		for (const auto element : all) {
			element->updateActives(time);
			element->update(time);
		}
	}
}