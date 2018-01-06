#pragma once
#include "CommandType.hpp"
#include "Time.hpp"
#include <memory>
#include <string>
#include <vector>
namespace S2VX {
	// Plain old object that holds command info
	struct Command {
		Command(CommandType pCommandType, const Time& pStart, const Time& pEnd)
			: commandType{ pCommandType }, start{ pStart }, end{ pEnd } {}
		virtual ~Command() {};
		CommandType commandType;
		Time start;
		Time end;
	};
	class CommandUniquePointerComparison {
	public:
		bool operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) {
			return lhs->start < rhs->start;
		}
	};
}