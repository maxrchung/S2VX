#pragma once

#include "CommandType.hpp"
#include "EasingType.hpp"
#include "ElementType.hpp"
#include "Time.hpp"
#include <memory>
#include <string>
#include <vector>

namespace S2VX {
	// Base class that all commands inherit from
	class Command {
	public:
		Command(CommandType pCommandType, ElementType pElementType, EasingType pEasingType, const Time& pStart, const Time& pEnd);
		virtual ~Command() {};

		CommandType commandType;
		ElementType elementType;
		// Used for most 
		EasingType easingType;

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