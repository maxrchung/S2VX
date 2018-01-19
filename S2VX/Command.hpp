#pragma once
#include "CommandType.hpp"
#include <memory>
#include <glm/glm.hpp>
namespace S2VX {
	// Plain old object that holds command info
	struct Command {
	public:
		Command(CommandType pCommandType, int pStart, int pEnd);
		virtual ~Command() {};
		CommandType commandType;
		int start;
		int end;
	protected:
		void validateColor(const glm::vec4& color);
		void validateFade(float fade);
		void validateScale(const glm::vec2& scale);
	};
	class CommandUniquePointerComparison {
	public:
		// Sort by start time then enum
		bool operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs);
	};
}