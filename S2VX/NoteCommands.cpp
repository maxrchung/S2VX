#include "NoteCommands.hpp"
namespace S2VX {
	NoteBindCommand::NoteBindCommand(int time, const NoteConfiguration& pConfiguration)
		: Command{ CommandType::NoteBind, time, time },
		configuration{ pConfiguration } {}
}