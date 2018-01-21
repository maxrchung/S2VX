#pragma once
#include "CommandType.hpp"
#include <memory>
#include <glm/glm.hpp>
namespace S2VX {
	// Plain old object that holds command info
	struct Command {
	public:
		explicit Command(const CommandType pCommandType, const int pStart, const int pEnd);
		virtual ~Command() {};
		const CommandType commandType;
		const int start;
		const int end;
	protected:
		void validateColor(const glm::vec4& color) const;
		void validateFade(float fade) const;
		void validateScale(const glm::vec2& scale) const;
	};
	struct CommandUniquePointerComparison {
		// Sort by start time then enum
		bool operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) const;
	};
}