#pragma once
#include "Command.hpp"
#include "CommandUniquePointerComparison.hpp"
#include <memory>
#include <set>
#include <vector>
namespace S2VX {
	class Camera;
	class Element {
	public:
		Element();
		virtual ~Element() = default;
		Element(Element&) = default;
		Element& operator=(const Element&) = default;
		Element(Element&&) = default;
		Element& operator=(Element&&) = default;
		virtual void draw() = 0;
		// Updates list of active commands
		// Virtual so that Notes/Sprites can perform special update
		virtual void update(const int time);
		void addCommand(std::unique_ptr<Command>&& command);
		// After initially adding the commands, we sort the vector into correct order
		virtual void sort();
	protected:
		static const CommandUniquePointerComparison commandComparison;
		// Deciding to use raw pointers because ownership is handled in Scripting class
		std::vector<std::unique_ptr<Command>> commands;
		// Used to track current command in updateActives()
		int nextActive;
		// Using a set so that ordering is preserved
		std::set<int> actives;
	};
}