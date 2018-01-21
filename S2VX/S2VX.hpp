#pragma once
#include <string>
namespace S2VX {
	class S2VX {
	public:
		explicit S2VX(const std::string& pScript);
		~S2VX() {};
		void run();
	private:
		// Don't make copies of this
		S2VX(const S2VX&) {};
		S2VX(S2VX&&) {};
		S2VX& operator=(const S2VX&) {};
		S2VX& operator=(S2VX&&) {};
		std::string script;
	};
}