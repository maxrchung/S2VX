#pragma once
#include "Back.hpp"
#include "Camera.hpp"
#include "Grid.hpp"
#include "Notes.hpp"
#include "Sprites.hpp"
#include <memory>
namespace S2VX {
	// Structure for handling all elements together
	// Responsible for updating/drawing in proper order
	class Elements {
	public:
		explicit Elements(const std::vector<Command*>& backCommands,
						  const std::vector<Command*>& cameraCommands,
						  const std::vector<Command*>& gridCommands, Shader* const lineShader,
						  const std::vector<Note*>& pNotes,
						  const std::vector<Sprite*>& pSprites);
		void draw();
		void update(const int time);
	private:
		std::unique_ptr<Back> back;
		std::unique_ptr<Camera> camera;
		std::unique_ptr<Grid> grid;
		std::unique_ptr<Notes> notes;
		std::unique_ptr<Sprites> sprites;
		// Holds everything
		std::vector<Element*> all;
	};
}