#pragma once
#include <glm/glm.hpp>
namespace S2VX {
	class NoteConfiguration {
	public:
		const glm::vec2& getPosition() const { return position; }
		float getDistance() const { return distance; }
		float getFeather() const { return feather; }
		float getWidth() const { return width; }
		int getApproach() const { return approach; }
		int getEnd() const { return end; }
		int getFadeIn() const { return fadeIn; }
		int getFadeOut() const { return fadeOut; }
		int getStart() const { return start; }
		void setApproach(const int pApproach) { approach = pApproach; }
		void setDistance(const float pDistance) { distance = pDistance; }
		void setFadeIn(const int pFadeIn) { fadeIn = pFadeIn; }
		void setFadeOut(const int pFadeOut) { fadeOut = pFadeOut; }
		void setFeather(const float pFeather) { feather = pFeather; }
		void setPosition(const glm::vec2& pPosition) { position = pPosition; }
		// Setting the end will also set the start based on approach
		void setEnd(const int pEnd);
		void setWidth(const float pWidth) { width = pWidth; }
	private:
		float distance = 3.0f;
		float feather = 0.01f;
		float width = 0.1f;
		glm::vec2 position;
		int approach = 2000;
		int end;
		int fadeIn = 500;
		int fadeOut = 100;
		int start;
	};
}