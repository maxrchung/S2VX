#pragma once
#include "Camera.hpp"
#include "NoteConfiguration.hpp"
#include "RectanglePoints.hpp"
#include "Shader.hpp"
#include <array>
namespace S2VX {
	class Note {
	public:
		explicit Note(const NoteConfiguration& pConfiguration, Shader* const pSquareShader);
		~Note();
		const NoteConfiguration& getConfiguration() { return configuration; }
		void draw(const Camera& camera);
		void update(const int time);
	private:
		const NoteConfiguration configuration;
		float scale;
		float fade;
		Shader* const squareShader;
		std::array<RectanglePoints, 4> lines;
		unsigned int squareVertexArray;
		unsigned int squareVertexBuffer;
	};
	struct NoteUniquePointerComparison {
		// Sort by start time
		bool operator() (const std::unique_ptr<Note>& lhs, const std::unique_ptr<Note>& rhs) const;
	};
}