using UnityEngine;
using System.Collections;

public class WTFrendenGame : FStage {
	public WTFrendenGame() : base("") {
		FSprite sprite = new FSprite("Futile_White");
		sprite.x = Futile.screen.halfWidth;
		sprite.y = Futile.screen.halfHeight;
		sprite.color = Color.red;
		AddChild(sprite);
	}
}
