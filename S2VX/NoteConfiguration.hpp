#pragma once
#include <glm/glm.hpp>
namespace S2VX {
	class NoteConfiguration {
	public:
		NoteConfiguration();
		const glm::vec3& getColor() const { return color; }
		const glm::vec2& getPosition() const { return position; }
		float getDistance() const { return distance; }
		float getFade() const { return fade; }
		float getFeather() const { return feather; }
		float getThickness() const { return thickness; }
		int getApproach() const { return approach; }
		int getEnd() const { return end; }
		int getFadeIn() const { return fadeIn; }
		int getFadeOut() const { return fadeOut; }
		int getStart() const { return start; }
		void setApproach(const int pApproach);
		void setColor(const glm::vec3& pColor);
		void setDistance(const float pDistance);
		// Setting the end will also set the start based on approach
		void setEnd(const int pEnd);
		void setFade(const float pFade);
		void setFadeIn(const int pFadeIn);
		void setFadeOut(const int pFadeOut);
		void setFeather(const float pFeather);
		void setPosition(const glm::vec2& pPosition) { position = pPosition; }
		void setThickness(const float pThickness);
	private:
		void validateTime(const int time);
		float distance;
		float fade;
		float feather;
		float thickness;
		glm::vec2 position;
		glm::vec3 color;
		int approach;
		int end;
		int fadeIn;
		int fadeOut;
		int start;
	};
}