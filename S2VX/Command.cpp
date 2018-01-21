#include "Command.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	Command::Command(const CommandType pCommandType, const int pStart, const int pEnd)
		: commandType{ pCommandType }, start{ pStart }, end{ pEnd } {
		if (start > end) {
			auto message = std::stringstream();
			message << "Command start time must be before end time. Given:" << std::endl
					<< "Start: " << std::to_string(start) << std::endl
					<< "End: " << std::to_string(end) << std::endl;
			throw ScriptError(message.str());
		}
	}
	void Command::validateColor(const glm::vec3& color) const {
		if (color.r < 0.0f || color.r > 1.0f || 
			color.g < 0.0f || color.g > 1.0f ||
			color.b < 0.0f || color.b > 1.0f) {
			auto message = std::stringstream();
			message << "Command color must be between 0 and 1. Given: ("
					<< std::to_string(color.r) << ',' 
					<< std::to_string(color.g) << ',' 
					<< std::to_string(color.b) << ')';
			throw ScriptError(message.str());
		}
	}
	void Command::validateFade(const float fade) const {
		if (fade < 0.0f || fade > 1.0f) {
			throw ScriptError("Command fade must be between 0 and 1. Given: " + std::to_string(fade));
		}
	}
	void Command::validateFeather(const float feather) const {
		if (feather < 0.0f) {
			throw ScriptError("Grid line feather must be greater than or equal to 0. Given: " + std::to_string(feather));
		}
	}
	void Command::validateScale(const glm::vec2& scale) const {
		if (scale.x < 0.0f || scale.y < 0.0f) {
			auto message = std::stringstream();
			message << "Command scale must be between 0 and 1. Given: ("
				<< std::to_string(scale.x) << ')';
			throw ScriptError(message.str());
		}
	}
	void Command::validateThickness(const float thickness) const {
		if (thickness < 0.0f) {
			throw ScriptError("Grid line thickness must be greater than equal to 0. Given: " + std::to_string(thickness));
		}
	}
	bool CommandUniquePointerComparison::operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) const {
		if (lhs->start != rhs->start) {
			return lhs->start < rhs->start;
		}
		else {
			return static_cast<int>(lhs->commandType) < static_cast<int>(rhs->commandType);
		}
	}
}