#include "Command.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	Command::Command(CommandType pCommandType, int pStart, int pEnd)
		: commandType{ pCommandType }, start{ pStart }, end{ pEnd } {
		if (start > end) {
			auto message = std::stringstream();
			message << "Command start time must be before end time. Given:" << std::endl;
			message << "Start: " << std::to_string(start) << std::endl;
			message << "End: " << std::to_string(end) << std::endl;
			throw ScriptError(message.str());
		}
		else if (start < 0) {
			throw ScriptError("Command start time must be >= 0. Given:" + std::to_string(start));
		}
		else if (end < 0) {
			throw ScriptError("Command end time must be >= 0. Given:" + std::to_string(end));
		}
	}
}