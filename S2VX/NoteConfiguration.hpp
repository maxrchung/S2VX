#pragma once
#include <glm/glm.hpp>
namespace S2VX {
	class NoteConfiguration {
	public:
		float getDistance() { return distance; }
		float getFeather() { return feather; }
		float getWidth() { return width; }
		glm::vec2 getPosition() { return position; }
		int getApproach() { return approach; }
		int getEnd() { return end; }
		int getFadeIn() { return fadeIn; }
		int getFadeOut() { return fadeOut; }
		int getStart() { return start; }
		void setApproach(int pApproach) { approach = pApproach; }
		void setDistance(float pDistance) { distance = pDistance; }
		void setFadeIn(int pFadeIn) { fadeIn = pFadeIn; }
		void setFadeOut(int pFadeOut) { fadeOut = pFadeOut; }
		void setFeather(float pFeather) { feather = pFeather; }
		void setPosition(glm::vec2 pPosition) { position = pPosition; }
		// Setting the end will also set the start based on approach
		void setEnd(int pEnd);
		void setWidth(float pWidth) { width = pWidth; }
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