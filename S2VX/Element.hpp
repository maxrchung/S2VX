#pragma once
#include <set>
#include <vector>
namespace S2VX {
	class Camera;
	class Command;
	class Element {
	public:
		// Default constructor, e.g. for Notes and Sprites
		Element() {};
		// To preserve unique_ptr, elements are moved from pCommands to commands
		explicit Element(const std::vector<Command*>& pCommands);
		virtual ~Element() {};
		virtual void draw(const Camera& camera) = 0;
		// Updates list of active commands
		// Virtual so that Notes/Sprites can perform special update
		virtual void update(const int time) = 0;
	protected:
		// Deciding to use raw pointers because ownership is handled in Scripting class
		const std::vector<Command*> commands;
		// Used to track current command in updateActives()
		int nextActive = 0;
		// Using a set so that ordering is preserved
		std::set<int> actives;
	};
}