#pragma once
#include "Command.hpp"
#include "NoteConfiguration.hpp"
namespace S2VX {
	struct NoteBindCommand : Command {
		NoteBindCommand(int time, const NoteConfiguration& pConfiguration);
		NoteConfiguration configuration;
		glm::vec2 coordinate;
	};
}