#include "Grid.hpp"
#include <GLFW/glfw3.h>

Grid::Grid(std::vector<std::unique_ptr<Command>>& commands) 
	: Element{ commands } {}

void Grid::update(int time) {

}

void Grid::draw() {
	glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}