#pragma once

#include "Texture.hpp"

namespace S2VX {
	class Sprite {
	public:
		Sprite(Texture* pTexture);
		Texture* texture;
	};
}