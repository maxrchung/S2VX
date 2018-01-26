#pragma once
struct GLFWwindow;
namespace S2VX {
	class Display {
	public:
		Display();								// default constructor
		~Display();								// d'tor
		bool shouldClose() const;
		GLFWwindow* const getWindow() const { return window; }
	private:
		// Window should not be copied/assigned/etc.
		// C++11:
		Display(const Display&) {};				// copy c'tor
		Display(Display&&) {};					// move c'tor
		Display& operator=(const Display&) {};	// copy-assign
		Display& operator=(Display&&) {};		// move-assign
		GLFWwindow* window;
	};
}