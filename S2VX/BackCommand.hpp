#pragma once
#include "Command.hpp"
namespace S2VX {
	class Back;
	class BackCommand : public Command {
	public:
		explicit BackCommand(Back* const pBack, const int start, const int end, const EasingType easing);
		virtual ~BackCommand() {};
		virtual void update(const float easing) = 0;
	protected:
		Back* const back;
	};
}