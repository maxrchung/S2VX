#pragma once

#include "Camera.hpp"
#include "Grid.hpp"
#include "Time.hpp"
#include <memory>

namespace S2VX {
	// Structure for handling all elements together
	// Responsible for updating/drawing in proper order
	class Elements {
	public:
		Elements(std::unique_ptr<Camera> pCamera, std::unique_ptr<Grid> pGrid);
		void update(const Time& time);
		void draw();

		std::unique_ptr<Camera> camera;
		std::unique_ptr<Grid> grid;
	};
}