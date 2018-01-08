#pragma once
#include "Element.hpp"
namespace S2VX {
	class Notes : public Element {
	public:
		Notes(const std::vector<Command*>& commands);
		~Notes();
		void draw(const Camera& camera);
		void update(int time);
	};
}
