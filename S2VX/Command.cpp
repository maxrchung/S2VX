#include "Command.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	Command::Command(const int pStart, const int pEnd, const EasingType pEasing)
		: start{ pStart }, end{ pEnd }, easing{ pEasing } {
		if (start > end) {
			auto message = std::stringstream();
			message << "Command start time must be <= end time. Given:" << std::endl
				<< "Start: " << std::to_string(start) << std::endl
				<< "End: " << std::to_string(end) << std::endl;
			throw ScriptError(message.str());
		}
		int easingValue = static_cast<int>(pEasing);
		int easingCount = static_cast<int>(EasingType::Count);
		int easingLast = static_cast<int>(EasingType::BounceEaseInOut);
		if (easingValue < 0 || easingValue >= easingCount) {
			throw ScriptError("Command easing must be >= 0 and <= " + std::to_string(easingLast) + ". Given: " + std::to_string(easingValue));
		}
	}
}