#pragma once
#include "Command.hpp"
#include <memory>
#include <unordered_set>
#include <vector>
namespace S2VX {
	class Camera;
	class Element {
	public:
		// To preserve unique_ptr, elements are moved from pCommands to commands
		Element(const std::vector<Command*>& pCommands);
		virtual ~Element() {};
		// Updates list of active commands
		virtual void draw(Camera* camera) = 0;
		virtual void update(const Time& time) = 0;
		void updateActives(const Time& time);
		int next = 0;
		// Deciding to use raw pointers because ownership is handled in Scripting class
		std::vector<Command*> commands;
		std::unordered_set<int> actives;
	};
}