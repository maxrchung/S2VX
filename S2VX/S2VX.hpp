#pragma once

#include <string>

class S2VX {
public:
	S2VX(const std::string& pScript);
	~S2VX() {};

	void run();

	std::string script;
private:
	// Don't make copies of this
	S2VX(const S2VX&) {};
	S2VX& operator=(const S2VX&) {};
	S2VX(S2VX&&) {};
	S2VX& operator=(S2VX&&) {};
};