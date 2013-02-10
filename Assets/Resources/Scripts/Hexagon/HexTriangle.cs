using UnityEngine;
using System.Collections;

public class HexTriangle : WTEntity {
	WTSpriteComponent shadowComponent;
	WTSpriteComponent mainSpriteComponent;
	
	public HexTriangle(string name) : base(name) {
		shadowComponent = new WTSpriteComponent("shadowComponent", "triangle");
		shadowComponent.sprite.color = new Color(0.12f, 0.12f, 0.12f, 1.0f);
		shadowComponent.sprite.scale = 1.2f;
		shadowComponent.sprite.alpha = 0.5f;
		AddComponent(shadowComponent);
		
		mainSpriteComponent = new WTSpriteComponent("mainSpriteComponent", "triangle");
		mainSpriteComponent.sprite.color = new Color(1.0f, 0.2f, 0.8f, 1.0f);
		AddComponent(mainSpriteComponent);
	}
	
	override public void HandleUpdate() {
		base.HandleUpdate();
		
		Vector2 globalPos = LocalToGlobal(new Vector2(mainSpriteComponent.sprite.x, mainSpriteComponent.sprite.y));
		
		Vector2 localShadowPos = GlobalToLocal(new Vector2(globalPos.x + 5f, globalPos.y - 5f));
		
		shadowComponent.sprite.x = localShadowPos.x;
		shadowComponent.sprite.y = localShadowPos.y;
	}
}
