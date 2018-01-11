#include "Notes.hpp"
#include "NoteCommands.hpp"
namespace S2VX {
	Notes::Notes(const std::vector<Command*>& commands)
		: Element{ commands } {}
	void Notes::draw(const Camera& camera) {
		for (auto& note : activeNotes) {
			note->draw(camera);
		}
	}
	void Notes::update(int time) {
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::NoteBind: {
					auto derived = static_cast<NoteBindCommand*>(command);
					auto configuration = derived->configuration;
					auto note = std::make_unique<Note>(configuration);
					activeNotes.insert(std::move(note));
					break;
				}
			}
		}
		for (auto& note : activeNotes) {
			note->update(time);
		}
	}
}