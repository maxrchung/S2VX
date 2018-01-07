#pragma once
#include "Command.hpp"
#include <set>
#include <vector>
namespace S2VX {
	class Camera;
	class Element {
	public:
		// To preserve unique_ptr, elements are moved from pCommands to commands
		Element(const std::vector<Command*>& pCommands);
		virtual ~Element() {};
		virtual void draw(const Camera& camera) = 0;
		virtual void update(int time) = 0;
		// Updates list of active commands
		void updateActives(int time);
	protected:
		// Has to be ordered because some commands must be ran before others, e.g. Sprite creation/commands
		std::set<int> actives;
		// Deciding to use raw pointers because ownership is handled in Scripting class
		std::vector<Command*> commands;
	private:
		// Used to track current command in updateActives()
		int nextCommand = 0;
	};
}