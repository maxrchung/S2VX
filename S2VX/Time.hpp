#pragma once

#include <string>

class Time {
public:
	Time(int pMS);
	Time(const std::string& pFormat);
	Time();
	bool operator<(const Time& time);

	int ms;
	std::string format;
};