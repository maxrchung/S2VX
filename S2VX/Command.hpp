#pragma once
#include "CommandType.hpp"
#include "ElementType.hpp"
#include "Time.hpp"
#include <memory>
#include <string>
#include <vector>
namespace S2VX {
	// Plain old object that holds command info
	struct Command {
		Command(CommandType pCommandType, ElementType pElementType, const Time& pStart, const Time& pEnd)
			: commandType{ pCommandType }, elementType{ pElementType }, start{ pStart }, end{ pEnd } {}
		virtual ~Command() {};
		CommandType commandType;
		ElementType elementType;
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