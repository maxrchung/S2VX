#pragma once
#include <string>
namespace S2VX {
	class Time {
	public:
		Time();
		explicit Time(const int pMS);
		explicit Time(const std::string& pFormat);
		bool operator<(const Time& rhs) const;
		bool operator<=(const Time& rhs) const;
		bool operator==(const Time& rhs) const;
		bool operator>(const Time& rhs) const;
		bool operator>=(const Time& rhs) const;
		const std::string& getFormat() { return format; }
		int getMS() const { return ms; }
	private:
		int ms;
		std::string format;
	};
}