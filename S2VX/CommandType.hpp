#pragma once
namespace S2VX {
	enum class CommandType {
		BackColor,
		CameraMove,
		CameraRotate,
		CameraZoom,
		GridFeather,
		GridThickness,
		// Note that for Sprite, the commands must be arranged in a certain order
		// The enum order determines the order of the command in the case of overlapping start times
		// In such a scenario Bind must happens first, then Create, ... other Sprite commands ..., 
		// and then finish with Delete
		SpriteBind,
		SpriteCreate,
		SpriteMove,
		SpriteDelete,
		Count
	};
}