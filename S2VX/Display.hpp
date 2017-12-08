#pragma once

class Display {
public:
	Display();								// default constructor
	~Display() {};							// d'tor

	int screenWidth = -1;

private:
	// Window should not be copied/assigned/etc.
	// C++11:
	Display(const Display&) {};				// copy c'tor
	Display& operator=(const Display&) {};	// copy-assign
	Display(Display&&) {};					// move c'tor
	Display& operator=(Display&&) {};		// move-assign

	void framebuffer_size_callback(GLFWwindow* window, int width, int height);
	void mouse_callback(GLFWwindow* window, double xpos, double ypos);
	void scroll_callback(GLFWwindow* window, double xoffset, double yoffset);
	void processInput(GLFWwindow *window);
};