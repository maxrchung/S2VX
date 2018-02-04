#include "CursorFeatherCommand.hpp"
#include "Cursor.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	CursorFeatherCommand::CursorFeatherCommand(Cursor& cursor, const int start, const int end, const EasingType easing, const float pStartFeather, const float pEndFeather)
		: CursorCommand{ cursor, start, end, easing },
		endFeather{ pEndFeather },
		startFeather{ pStartFeather } {
		validateCursorFeather(endFeather);
		validateCursorFeather(startFeather);
	}
	void CursorFeatherCommand::update(const float easing) {
		const auto feather = glm::mix(startFeather, endFeather, easing);
		cursor.setFeather(feather);
	}
	void CursorFeatherCommand::validateCursorFeather(const float feather) {
		if (feather < 0.0f) {
			throw ScriptError("Cursor feather must be >= 0. Given: " + std::to_string(feather));
		}
	}
}