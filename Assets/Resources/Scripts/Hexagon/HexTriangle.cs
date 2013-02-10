using UnityEngine;
using System.Collections;

public class HexTriangle : WTEntity {

	public HexTriangle(string name) : base(name) {
		AddComponent(new WTSpriteComponent("spriteComponent", "triangle"));
	}
}
