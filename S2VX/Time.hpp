#pragma once

#include <string>

class Time {
public:
	Time(int pMS);
	Time(const std::string& pFormat);
	Time();
	bool operator<(const Time& rhs);
	bool operator<=(const Time& rhs);
	bool operator==(const Time& rhs);
	bool operator>(const Time& rhs);
	bool operator>=(const Time& rhs);


	int ms;
	std::string format;
};