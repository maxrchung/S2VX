#pragma once
#include "Camera.hpp"
#include "NoteConfiguration.hpp"
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
		std::unique_ptr<Shader> squareShader = std::make_unique<Shader>("Square.VertexShader", "Line.FragmentShader");
		static constexpr unsigned int squarePointsLength = 96;
		static constexpr std::array<float, squarePointsLength> squarePoints = {
			// Position			// Normal
			 0.5f,	 0.5f,		 1.0f,	 0.0f,	// Right triangle top-right		0
			 0.5f,	-0.5f,		 1.0f,	 0.0f,	// Right triangle top-right		4
			 0.5f,	 0.5f,		-1.0f,	 0.0f,	// Right triangle top-right		8

			 0.5f,	-0.5f,		 1.0f,	 0.0f,	// Right triangle bottom-left	12
			 0.5f,	-0.5f,		-1.0f,	 0.0f,	// Right triangle bottom-left	16
			 0.5f,	 0.5f,		-1.0f,	 0.0f,	// Right triangle bottom-left	20

			 0.5f,	-0.5f,		 0.0f,	 1.0f,	// Bottom triangle top-right	24
			 0.5f,	-0.5f,		 0.0f,	-1.0f,	// Bottom triangle top-right	28
			-0.5f,	-0.5f,		 0.0f,	 1.0f,	// Bottom triangle top-right	32

			 0.5f,	-0.5f,		 0.0f,	-1.0f,	// Bottom triangle bottom-left	36
			-0.5f,	-0.5f,		 0.0f,	-1.0f,	// Bottom triangle bottom-left	40
			-0.5f,	-0.5f,		 0.0f,	 1.0f,	// Bottom triangle bottom-left	44

			-0.5f,	 0.5f,		 1.0f,	 0.0f,	// Left triangle top-right		48
			-0.5f,	-0.5f,		 1.0f,	 0.0f,	// Left triangle top-right		52
			-0.5f,	 0.5f,		-1.0f,	 0.0f,	// Left triangle top-right		56

			-0.5f,	-0.5f,		 1.0f,	 0.0f,	// Left triangle bottom-left	60
			-0.5f,	-0.5f,		-1.0f,	 0.0f,	// Left triangle bottom-left	64
			-0.5f,	 0.5f,		-1.0f,	 0.0f,	// Left triangle bottom-left	68

			 0.5f,	 0.5f,		 0.0f,	 1.0f,	// Top triangle top-right		72
			 0.5f,	 0.5f,		 0.0f,	-1.0f,	// Top triangle top-right		76
			-0.5f,	 0.5f,		 0.0f,	 1.0f,	// Top triangle top-right		80

			 0.5f,	 0.5f,		 0.0f,	-1.0f,	// Top triangle bottom-left		84
			-0.5f,	 0.5f,		 0.0f,	-1.0f,	// Top triangle bottom-left		88
			-0.5f,	 0.5f,		 0.0f,	 1.0f,	// Top triangle bottom-left		92
		};
		std::array<float, squarePointsLength> adjustedPoints;
		unsigned int squareVertexArray;
		unsigned int squareVertexBuffer;
		void adjustPoints(int startIndex, int end, float lineWidth, float feather);
	};
}