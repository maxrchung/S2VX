#pragma once

#include "Display.hpp"

class S2VX {
public:
	S2VX();

	void run();

	Display display;
	bool running = true;
};