#pragma once

#include <string>

class Time {
public:
	Time() { Time(0); }
	Time(int value);
	Time(std::string format);
	int ms;
	std::string format;
};