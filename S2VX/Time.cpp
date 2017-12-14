#include "Time.hpp"

#include <regex>

namespace S2VX {
	Time::Time(int pMS)
		: ms{ pMS } {
		int minutes = ms / 60000;
		int minusMinutes = ms - minutes * 60000;
		int seconds = minusMinutes / 1000;
		int minusSeconds = minusMinutes - seconds * 1000;
		char buffer[50];
		sprintf(buffer, "%02d:%02d:%03d", minutes, seconds, minusSeconds);
		format = buffer;
	}

	Time::Time(const std::string& pFormat)
		: format{ pFormat } {
		std::smatch smatch;
		std::regex regex{ R"(\d{2,})" };
		std::regex_search(pFormat, smatch, regex);
		int minutes = std::stoi(smatch[0]);

		auto search = smatch.suffix().str();
		std::regex_search(search, smatch, regex);
		int seconds = std::stoi(smatch[0]);

		search = smatch.suffix().str();
		std::regex_search(search, smatch, regex);
		int pMS = std::stoi(smatch[0]);
		ms = minutes * 60000 + seconds * 1000 + pMS;
	}

	Time::Time() {
		Time(0);
	}

	bool Time::operator<(const Time& rhs) {
		return ms < rhs.ms;
	}

	bool Time::operator<=(const Time& rhs) {
		return ms <= rhs.ms;

	}

	bool Time::operator==(const Time& rhs) {
		return ms == rhs.ms;

	}

	bool Time::operator>(const Time& rhs) {
		return ms > rhs.ms;
	}

	bool Time::operator>=(const Time& rhs) {
		return ms >= rhs.ms;
	}
}