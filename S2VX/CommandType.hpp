#pragma once

namespace S2VX {
	enum class CommandType {
		GridColorBack,
		CameraMove,
		CameraRotate,
		CameraZoom,
		SpriteBind,
		SpriteCreate,
		SpriteDestroy,
		SpriteMove,
		Count
	};
}