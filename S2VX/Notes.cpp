#include "Notes.hpp"
#include "NoteCommands.hpp"
namespace S2VX {
	Notes::Notes(const std::vector<Command*>& commands)
		: Element{ commands } {}
	void Notes::draw(const Camera& camera) {
	}
	void Notes::update(int time) {
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::Note: {
					auto derived = static_cast<NoteCommand*>(command);
					break;
				}
			}
		}
	}
}