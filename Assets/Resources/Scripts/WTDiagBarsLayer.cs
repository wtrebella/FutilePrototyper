using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTDiagBarsLayer : FContainer {
	
	List<FSprite> sprites = new List<FSprite>();
	float widthPerBar = 80;
	
	public WTDiagBarsLayer() {
		for (int i = 0; i < 16; i++) {
			FSprite sprite = new FSprite("diagBar.png");
			sprite.scale = 1.5f;
			sprite.anchorX = 0;
			sprite.y = Futile.screen.halfHeight;
			sprite.x = Futile.screen.width - widthPerBar * i;
			if (i % 2 == 0) sprite.color = new Color(0.62f, 0.62f, 0.62f, 1.0f);
			else sprite.color = new Color(0.65f, 0.65f, 0.65f, 1.0f);
			sprites.Add(sprite);
			AddChild(sprite);
		}
	}
	
	public override void HandleAddedToStage () {
		base.HandleAddedToStage ();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	public override void HandleRemovedFromStage () {
		base.HandleRemovedFromStage ();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	public void HandleUpdate() {
		foreach (FSprite sprite in sprites) {
			sprite.x -= 100f * Time.deltaTime;
			
			if (sprite.x + sprite.textureRect.width * sprite.scaleX < 0) {
				sprite.x += widthPerBar * sprites.Count;
			}
		}
	}
}
