#pragma once
#include "CommandType.hpp"
#include <memory>
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	// Plain old object that holds command info
	struct Command {
		Command(CommandType pCommandType, int pStart, int pEnd)
			: commandType{ pCommandType }, start{ pStart }, end{ pEnd } {
			if (start > end) {
				auto message = std::stringstream();
				message << "Command start time must be before end time. Given:" << std::endl;
				message << "Start: " << std::to_string(start) << std::endl;
				message << "End: " << std::to_string(end) << std::endl;
				throw ScriptError(message.str());
			}
			else if (start < 0) {
				auto message = std::stringstream();
				message << "Command start time must be >= 0. Given:" << std::endl;
				message << "Start: " << std::to_string(start) << std::endl;
				throw ScriptError(message.str());
			}
			else if (end < 0) {
				auto message = std::stringstream();
				message << "Command end time must be >= 0. Given:" << std::endl;
				message << "End: " << std::to_string(start) << std::endl;
				throw ScriptError(message.str());
			}
		}
		virtual ~Command() {};
		CommandType commandType;
		int start;
		int end;
	};
	class CommandUniquePointerComparison {
	public:
		// Sort by start time then enum
		bool operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) {
			if (lhs->start != rhs->start) {
				return lhs->start < rhs->start;
			}
			else {
				return static_cast<int>(lhs->commandType) < static_cast<int>(rhs->commandType);
			}
		}
	};
}