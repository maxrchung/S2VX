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
		Elements(std::unique_ptr<Back>& pBack, std::unique_ptr<Camera>& pCamera, std::unique_ptr<Grid>& pGrid, std::unique_ptr<Notes>& pNotes, std::unique_ptr<Sprites>& pSprites);
		void draw();
		void update(int time);
		std::unique_ptr<Back> back;
		std::unique_ptr<Camera> camera;
		std::unique_ptr<Grid> grid;
		std::unique_ptr<Notes> notes;
		std::unique_ptr<Sprites> sprites;
		// Holds everything
		std::vector<Element*> all;
	};
}