#include "S2VX.hpp"
#include "Display.hpp"
#include "Element.hpp"
#include "Scripting.hpp"
namespace S2VX {
	S2VX::S2VX(const std::string& pScript)
		: script{ pScript } {
	}
	void S2VX::run() {
		Display display;
		Scripting scripting;
		auto elements = scripting.evaluate(script);
		auto now = static_cast<float>(glfwGetTime());
		auto previous = now;
		auto delta = 0.0f;
		auto total = 0.0f;
		// render loop
		while (!display.shouldClose()) {
			// per-frame time logic
			now = static_cast<float>(glfwGetTime());
			delta = now - previous;
			previous = now;
			total += delta;
			// Convert to milliseconds
			const auto milliseconds = static_cast<int>(total * 1000.0f);
			elements.update(milliseconds);
			elements.draw();
			// glfw: swap buffers and poll IO events (keys pressed/released, mouse moved etc.)
			glfwSwapBuffers(display.getWindow());
			glfwPollEvents();
		}
	}
}