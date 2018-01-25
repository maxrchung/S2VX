#pragma once
#include "EasingType.hpp"
namespace S2VX {
	// Base for all commands
	class Command {
	public:
		explicit Command(const int pStart, const int pEnd, const EasingType pEasing);
		virtual ~Command() {};
		const EasingType getEasing() const { return easing; }
		const int getEnd() const { return end; }
		const int getStart() const { return start; }
		virtual void update(const int time) = 0;
	private:
		const EasingType easing;
		const int end;
		const int start;
	};
}