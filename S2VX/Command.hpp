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
		void validateColor(const glm::vec3& color) const;
		void validateFade(const float fade) const;
		void validateFeather(const float feather) const;
		void validateScale(const glm::vec2& scale) const;
		void validateThickness(const float thickness) const;
	};
	struct CommandUniquePointerComparison {
		// Sort by start time then enum
		bool operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) const;
	};
}