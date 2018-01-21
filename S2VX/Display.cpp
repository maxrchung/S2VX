#include "Display.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	Display::Display() {
		// glfw: initialize and configure
		glfwInit();
		glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
		glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
		glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
		// It is not recommended to use GLFW_FLOATING, but this is an easy way to display window over task bar
		//glfwWindowHint(GLFW_FLOATING, GL_TRUE);
		glfwWindowHint(GLFW_DECORATED, GL_FALSE);
		const auto monitor = glfwGetPrimaryMonitor();
		const auto mode = glfwGetVideoMode(monitor);
		const auto monitorWidth = mode->width;
		const auto monitorHeight = mode->height;
		auto windowWidth = monitorHeight;
		// Set screenWidth to be square and the shortest side
		if (monitorWidth < monitorHeight) {
			windowWidth = monitorWidth;
		}
		// glfw window creation
		window = glfwCreateWindow(windowWidth, windowWidth, "S2VX", NULL, NULL);
		if (window == NULL) {
			throw ScriptError("Failed to create GLFW window.");
			glfwTerminate();
		}
		glfwMakeContextCurrent(window);
		// GLFW_DECORATED sets window position to top left corner, need to reset window to middle of screen
		const auto midX = monitorWidth / 2.0f - windowWidth / 2.0f;
		glfwSetWindowPos(window, static_cast<int>(midX), 0);
		// glad: load all OpenGL function pointers
		if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress)) {
			throw ScriptError("Failed to initialize GLAD.");
		}
		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	}
	Display::~Display() {
		// Hopefully this doesn't kill all windows lmaO
		// glfw: terminate, clearing all previously allocated GLFW resources.
		glfwTerminate();
	}
	bool Display::shouldClose() const {
		return glfwWindowShouldClose(window);
	}
}