#pragma once
#include <string>
namespace S2VX {
	class Time {
	public:
		Time();
		Time(int pMS);
		Time(const std::string& pFormat);
		int ms;
		bool operator<(const Time& rhs);
		bool operator<=(const Time& rhs);
		bool operator==(const Time& rhs);
		bool operator>(const Time& rhs);
		bool operator>=(const Time& rhs);
		std::string format;
	};
}