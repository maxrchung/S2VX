#include "S2VX.hpp"

#include "Display.hpp"
#include "Element.hpp"
#include "Scripting.hpp"

S2VX::S2VX(const std::string& pScript)
	: script{ pScript } {}

void S2VX::run() {
	Display display;

	Scripting scripting;
	auto elements = scripting.evaluate(script);
	 
	float now = glfwGetTime();
	float previous = now;
	float delta = 0.0f;

	// render loop
	// -----------
	while (!display.shouldClose()) {
		// per-frame time logic
		// --------------------
		now = glfwGetTime();
		delta = now - previous;
		previous = now;

		// Update
		for (auto& element : elements) {
			element->update(delta);
		}

		// Draw
		for (auto& element : elements) {
			element->draw();
		}

		// glfw: swap buffers and poll IO events (keys pressed/released, mouse moved etc.)
		// -------------------------------------------------------------------------------
		glfwSwapBuffers(display.window);
		glfwPollEvents();
	}
}