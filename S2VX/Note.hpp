#pragma once
#include "Camera.hpp"
#include "NoteConfiguration.hpp"
#include "RectanglePoints.hpp"
#include "Shader.hpp"
#include <array>
namespace S2VX {
	class Note {
	public:
		Note(const NoteConfiguration& pConfiguration);
		~Note();
		void draw(const Camera& camera);
		void update(int time);
	private:
		// Interpolated distance
		float activeScale;
		float activeFade;
		NoteConfiguration configuration;
		std::unique_ptr<Shader> squareShader = std::make_unique<Shader>("Rectangle.VertexShader", "Rectangle.FragmentShader");
		std::array<RectanglePoints, 4> lines;
		unsigned int squareVertexArray;
		unsigned int squareVertexBuffer;
	};
}