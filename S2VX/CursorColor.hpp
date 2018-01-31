#pragma once
#include "CursorCommand.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Cursor;
	class CursorColor : public CursorCommand {
	public:
		CursorColor(Cursor* cursor, const int start, const int end, EasingType easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		void update(const float easing);
	private:
		void validateCursorColor(const glm::vec3& color);
		const glm::vec3 endColor;
		const glm::vec3 startColor;
	};
}