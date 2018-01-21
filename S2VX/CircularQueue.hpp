#pragma once
#include <stdexcept>
#include <vector>
namespace S2VX {
	template<typename T>
	// Creates a circular queue of fixed size
	// Implemented internally with vector
	class CircularQueue {
	public:
		CircularQueue() {
			CircularQueue(0);
		}
		// Seems like the simplest way to get templates to work is to put everything in the header
		explicit CircularQueue(const int pMaxSize)
			: maxSize{ pMaxSize }, elements{ std::vector<T>(pMaxSize) } {}
		const T& peekBack() {
			return elements[back];
		}
		const T& peekFront() {
			return elements[front];
		}
		void getSize() { return size; }
		void push(const T& element) {
			if (size == maxSize) {
				throw std::overflow_error("Circular queue push overflow.");
			}
			elements[back] = element;
			back = (back + 1) % maxSize;
			++size;
		}
		void pop() {
			if (size == 0) {
				throw std::underflow_error("Circular queue pop underflow.");
			}
			front = (front + 1) % maxSize;
			--size;
		}
	private:
		const int maxSize;
		int back = 0;
		int front = 0;
		int size = 0;
		std::vector<T> elements;
	};
}