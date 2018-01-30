#include "NoteConfiguration.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	NoteConfiguration::NoteConfiguration()
		: approach{ 2000 },
		color{ glm::vec3{ 1.0f, 1.0f, 1.0f } },
		distance{ 3.0f },
		fade{ 0.8f },
		fadeIn{ 500 },
		fadeOut{ 100 },
		feather{ 0.01f },
		position{ glm::vec2{ 0.0f, 0.0f }},
		thickness{ 0.1f } {}
	void NoteConfiguration::setApproach(const int pApproach) {
		validateTime(pApproach);
		approach = pApproach;
	}
	void NoteConfiguration::setColor(const glm::vec3& pColor) {
		if (pColor.r < 0.0f || pColor.r > 1.0f ||
			pColor.g < 0.0f || pColor.g > 1.0f ||
			pColor.b < 0.0f || pColor.b > 1.0f) {
			auto message = std::stringstream();
			message << "Note color values must be >= 0 and <= 1. Given: ("
				<< std::to_string(color.r) << ','
				<< std::to_string(color.g) << ','
				<< std::to_string(color.b) << ')';
			throw ScriptError(message.str());
		}
		color = pColor;
	}
	void NoteConfiguration::setDistance(const float pDistance) {
		if (pDistance < 0.0f) {
			throw ScriptError("Note distance must be >= 0. Given: " + std::to_string(pDistance));
		}
		distance = pDistance;
	}
	void NoteConfiguration::setEnd(const int pEnd) {
		validateTime(pEnd);
		end = pEnd + fadeOut;
		start = pEnd - approach;
	}
	void NoteConfiguration::setFade(const float pFade) {
		if (pFade < 0.0f || pFade > 1.0f) {
			throw ScriptError("Note fade must be >= 0 and <= 1. Given: " + std::to_string(pFade));
		}
		fade = pFade;
	}
	void NoteConfiguration::setFadeIn(const int pFadeIn) {
		validateTime(pFadeIn);
		if (pFadeIn > approach) {
			auto message = std::stringstream();
			message << "Note fade in time must be <= approach time. Given: " << std::endl
				<< "FadeIn: " << std::to_string(pFadeIn) << std::endl
				<< "Approach: " << std::to_string(approach) << std::endl;
			throw ScriptError(message.str());
		}
		fadeIn = pFadeIn;
	}
	void NoteConfiguration::setFadeOut(const int pFadeOut) {
		validateTime(pFadeOut);
		fadeOut = pFadeOut;
	}
	void NoteConfiguration::setFeather(const float pFeather) {
		if (pFeather < 0.0f) {
			throw ScriptError("Note line feather must be >= 0. Given: " + std::to_string(pFeather));
		}
		feather = pFeather;
	}
	void NoteConfiguration::setThickness(const float pThickness) {
		if (thickness < 0.0f) {
			throw ScriptError("Note line thickness must be >= 0. Given: " + std::to_string(pThickness));
		}
		thickness = pThickness;
	}
	// I don't think notes should be placed at negative time :bigThink:
	void NoteConfiguration::validateTime(const int time) {
		if (time < 0) {
			throw ScriptError("Note time must be >= 0. Given: " + std::to_string(time));
		}
	}
}