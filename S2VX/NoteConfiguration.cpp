#include "NoteConfiguration.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	void NoteConfiguration::setApproach(const int pApproach) {
		validateTime(pApproach);
		approach = pApproach;
	}
	void NoteConfiguration::setColor(const glm::vec3& pColor) {
		if (pColor.r < 0.0f || pColor.r > 1.0f ||
			pColor.g < 0.0f || pColor.g > 1.0f ||
			pColor.b < 0.0f || pColor.b > 1.0f) {
			auto message = std::stringstream();
			message << "Note color must be between 0 and 1. Given: ("
				<< std::to_string(color.r) << ','
				<< std::to_string(color.g) << ','
				<< std::to_string(color.b) << ')';
			throw ScriptError(message.str());
		}
		color = pColor;
	}
	void NoteConfiguration::setDistance(const float pDistance) {
		if (pDistance < 0.0f) {
			throw ScriptError("Note distance must be greater than or equal to 0. Given: " + std::to_string(pDistance));
		}
		distance = pDistance;
	}
	void NoteConfiguration::setFadeIn(const int pFadeIn) {
		validateTime(pFadeIn);
		if (pFadeIn > approach) {
			auto message = std::stringstream();
			message << "Note fade in time must be less than approach time. Given: " << std::endl
				<< "FadeIn: " << std::to_string(pFadeIn) << std::endl
				<< "Approach: " << std::to_string(approach) << std::endl;
			throw ScriptError(message.str());
		}
		fadeIn = pFadeIn;
	}
	void NoteConfiguration::setEnd(const int pEnd) {
		validateTime(pEnd);
		end = pEnd + fadeOut;
		start = pEnd - approach;
	}
	void NoteConfiguration::setFadeOut(const int pFadeOut) {
		validateTime(pFadeOut);
		fadeOut = pFadeOut;
	}
	void NoteConfiguration::setFeather(const float pFeather) {
		if (pFeather < 0.0f) {
			throw ScriptError("Note line feather must be greater than or equal to 0. Given: " + std::to_string(pFeather));
		}
		feather = pFeather;
	}
	void NoteConfiguration::setThickness(const float pThickness) {
		if (thickness < 0.0f) {
			throw ScriptError("Note line thickness must be greater than or equal to 0. Given: " + std::to_string(pThickness));
		}
		thickness = pThickness;
	}
	// I don't think notes should be placed at negative time :bigThink:
	void NoteConfiguration::validateTime(const int time) {
		if (time < 0) {
			throw ScriptError("Note time must be greater than or equal to 0. Given: " + std::to_string(time));
		}
	}
}