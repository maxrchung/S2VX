#include "Time.hpp"
#include <regex>
namespace S2VX {
	Time::Time() {
		Time(0);
	}
	Time::Time(const int pMS)
		: ms{ pMS } {
		const int minutes = ms / 60000;
		const int minusMinutes = ms - minutes * 60000;
		const int seconds = minusMinutes / 1000;
		const int minusSeconds = minusMinutes - seconds * 1000;
		char buffer[50];
		sprintf(buffer, "%02d:%02d:%03d", minutes, seconds, minusSeconds);
		format = buffer;
	}
	Time::Time(const std::string& pFormat)
		: format{ pFormat } {
		const std::regex regex{ R"(\d+)" };
		std::smatch smatch;
		std::regex_search(pFormat, smatch, regex);
		const int minutes = std::stoi(smatch[0]);

		auto search = smatch.suffix().str();
		std::regex_search(search, smatch, regex);
		const int seconds = std::stoi(smatch[0]);

		search = smatch.suffix().str();
		std::regex_search(search, smatch, regex);
		const int pMS = std::stoi(smatch[0]);
		ms = minutes * 60000 + seconds * 1000 + pMS;
	}
	bool Time::operator<(const Time& rhs) const {
		return ms < rhs.ms;
	}
	bool Time::operator<=(const Time& rhs) const {
		return ms <= rhs.ms;

	}
	bool Time::operator==(const Time& rhs) const {
		return ms == rhs.ms;

	}
	bool Time::operator>(const Time& rhs) const {
		return ms > rhs.ms;
	}
	bool Time::operator>=(const Time& rhs) const {
		return ms >= rhs.ms;
	}
}