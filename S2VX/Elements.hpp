#pragma once
#include "Camera.hpp"
#include "Grid.hpp"
#include "Sprites.hpp"
#include "Time.hpp"
#include <memory>
namespace S2VX {
	// Structure for handling all elements together
	// Responsible for updating/drawing in proper order
	class Elements {
	public:
		Elements(std::unique_ptr<Camera> pCamera, std::unique_ptr<Grid> pGrid, std::unique_ptr<Sprites> pSprite);
		void draw();
		void update(const Time& time);
		std::unique_ptr<Camera> camera;
		std::unique_ptr<Grid> grid;
		std::unique_ptr<Sprites> sprite;
	};
}