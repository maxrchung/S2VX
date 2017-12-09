#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <memory>

class Display {
public:
	Display();								// default constructor
	~Display();								// d'tor

	bool shouldClose();

	GLFWwindow* window = nullptr;
	int windowWidth = -1;

private:
	// Window should not be copied/assigned/etc.
	// C++11:
	Display(const Display&) {};				// copy c'tor
	Display& operator=(const Display&) {};	// copy-assign
	Display(Display&&) {};					// move c'tor
	Display& operator=(Display&&) {};		// move-assign
};