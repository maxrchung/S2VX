#pragma once
#include "CommandType.hpp"
#include <memory>
#include <string>
#include <vector>
namespace S2VX {
	// Plain old object that holds command info
	struct Command {
		Command(CommandType pCommandType, int pStart, int pEnd)
			: commandType{ pCommandType }, start{ pStart }, end{ pEnd } {}
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