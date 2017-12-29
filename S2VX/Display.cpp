#include "Display.hpp"

#include <iostream>

namespace S2VX {
	Display::Display() {
		// glfw: initialize and configure
		// ------------------------------
		glfwInit();
		glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
		glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
		glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

		// It is not recommended to use GLFW_FLOATING, but this is an easy way to display window over task bar
		//glfwWindowHint(GLFW_FLOATING, GL_TRUE);
		glfwWindowHint(GLFW_DECORATED, GL_FALSE);

		auto monitor = glfwGetPrimaryMonitor();
		auto mode = glfwGetVideoMode(monitor);
		auto monitorWidth = mode->width;
		auto monitorHeight = mode->height;

		// Set screenWidth to be square and the shortest side
		if (monitorHeight < monitorWidth) {
			windowWidth = monitorHeight;
		}
		else {
			windowWidth = monitorWidth;
		}

		// glfw window creation
		window = glfwCreateWindow(windowWidth, windowWidth, "S2VX", NULL, NULL);
		if (window == NULL) {
			std::cout << "Failed to create GLFW window" << std::endl;
			glfwTerminate();
		}
		glfwMakeContextCurrent(window);

		// GLFW_DECORATED sets window position to top left corner, need to reset window to middle of screen
		auto midX = monitorWidth / 2.0f - windowWidth / 2.0f;
		glfwSetWindowPos(window, midX, 0);

		// tell GLFW to capture our mouse
		//glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

		// glad: load all OpenGL function pointers
		// ---------------------------------------
		if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress)) {
			std::cout << "Failed to initialize GLAD" << std::endl;
		}

		// configure global opengl state
		// -----------------------------
		glEnable(GL_DEPTH_TEST);
	}

	Display::~Display() {
		// Hopefully this doesn't kill all windows lmaO
		// glfw: terminate, clearing all previously allocated GLFW resources.
		// ------------------------------------------------------------------
		glfwTerminate();
	}

	bool Display::shouldClose() {
		return glfwWindowShouldClose(window);
	}
}