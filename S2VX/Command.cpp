#include "Command.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	Command::Command(CommandType pCommandType, int pStart, int pEnd)
		: commandType{ pCommandType }, start{ pStart }, end{ pEnd } {
		if (start > end) {
			auto message = std::stringstream();
			message << "Command start time must be before end time. Given:" << std::endl
					<< "Start: " << std::to_string(start) << std::endl
					<< "End: " << std::to_string(end) << std::endl;
			throw ScriptError(message.str());
		}
	}
	void Command::validateColor(const glm::vec4& color) {
		if (color.r < 0.0f || color.r > 1.0f || 
			color.g < 0.0f || color.g > 1.0f ||
			color.b < 0.0f || color.b > 1.0f || 
			color.a < 0.0f || color.a > 1.0f) {
			auto message = std::stringstream();
			message << "Command color must be between 0 and 1. Given: ("
					<< std::to_string(color.r) << ',' 
					<< std::to_string(color.g) << ',' 
					<< std::to_string(color.b) << ','
					<< std::to_string(color.a) << ')';
			throw ScriptError(message.str());
		}
	}
	void Command::validateFade(float fade) {
		if (fade < 0.0f || fade > 1.0f) {
			throw ScriptError("Command fade must be between 0 and 1. Given: " + std::to_string(fade));
		}
	}
	void Command::validateScale(const glm::vec2& scale) {
		if (scale.x < 0.0f || scale.y < 0.0f) {
			auto message = std::stringstream();
			message << "Command color must be between 0 and 1. Given: ("
				<< std::to_string(scale.x) << ')';
			throw ScriptError(message.str());
		}
	}
	bool CommandUniquePointerComparison::operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) {
		if (lhs->start != rhs->start) {
			return lhs->start < rhs->start;
		}
		else {
			return static_cast<int>(lhs->commandType) < static_cast<int>(rhs->commandType);
		}
	}
}