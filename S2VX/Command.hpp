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
		// Camera for example should not have X and Y scaling, so for single scale validation, use this
		void validateScale(float scale);
		void validateScale(const glm::vec2& scale);
		// I feel a bit bad having this accessible here since it's only used in Sprite commands,
		// but because it's not like a data member that's copied across everything, I think I can live
		// with this maybe maybe not. To be honest I don't know if inheritance is the best solution for
		// this Command architecture. There seems to be cracks appearing here and there, but whatever,
		// probably my best solution for now.
		void validateSpriteID(int spriteID);
	};
	class CommandUniquePointerComparison {
	public:
		// Sort by start time then enum
		bool operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs);
	};
}