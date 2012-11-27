using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTCompartments : FStage, FSingleTouchableInterface {
	public List<FSprite> compartmentSprites = new List<FSprite>();
	
	public WTCompartments() : base("") {
		FSprite background = new FSprite("background.png");
		background.x = Futile.screen.halfWidth;
		background.y = Futile.screen.halfHeight;
		AddChild(background);
				
		for (int i = 0; i < 6; i++) {
			string newString = string.Format("compartment{0}.png", i);
			FSprite newSprite = new FSprite(newString);
			newSprite.color = Color.blue;
			newSprite.x = Futile.screen.halfWidth;
			newSprite.y = Futile.screen.halfHeight;
			compartmentSprites.Add(newSprite);
			AddChild(newSprite);
		}
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.touchManager.AddSingleTouchTarget(this);
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.touchManager.RemoveSingleTouchTarget(this);
	}
	
	public bool HandleSingleTouchBegan(FTouch touch) {
		foreach (FSprite sprite in compartmentSprites) {
			/*if (sprite.textureRect.Contains(sprite.GlobalToLocal(touch.position))) {
				if (sprite.color == Color.blue) sprite.color = Color.red;
				else sprite.color = Color.blue;
			}*/
			Debug.Log(sprite.textureRect);	
			//Debug.Log(sprite.element.atlas.texture.GetPixel((int)touch.position.x, (int)touch.position.y));
		}
		
		return true;
	}
	
	public void HandleSingleTouchMoved(FTouch touch) {
		
	}
	
	public void HandleSingleTouchEnded(FTouch touch) {
		
	}
	
	public void HandleSingleTouchCanceled(FTouch touch) {
		
	}
}
