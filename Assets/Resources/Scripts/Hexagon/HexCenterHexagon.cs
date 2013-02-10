using UnityEngine;
using System.Collections;

public class HexCenterHexagon : WTEntity {

	public HexCenterHexagon(string name) : base(name) {
		AddComponent(new WTSpriteComponent("spriteComponent", "hexagon"));
	}
}
