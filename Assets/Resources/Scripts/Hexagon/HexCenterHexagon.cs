using UnityEngine;
using System.Collections;

public class HexCenterHexagon : WTEntity {

	public HexCenterHexagon(string name) : base(name) {
		WTSpriteComponent innerHex = new WTSpriteComponent("innerHex", "innerHexagon");
		innerHex.sprite.color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
		AddComponent(innerHex);
		
		WTSpriteComponent outerHex = new WTSpriteComponent("outerHex", "hexagon");
		outerHex.sprite.color = new Color(1.0f, 0.8f, 0.2f, 1.0f);
		AddComponent(outerHex);
	}
}
