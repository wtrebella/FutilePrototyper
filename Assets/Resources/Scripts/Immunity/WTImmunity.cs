//#define DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTImmunity : FStage, FSingleTouchableInterface {
	ImOrganLayer organLayer;
	ImNodeLayer nodeLayer;
	ImBodyPart currentOrgan;

	public WTImmunity() : base("") {	
		FSprite sprite = new FSprite("body.png");
		sprite.scale = 0.6f;
		sprite.x = Futile.screen.halfWidth;
		sprite.y = Futile.screen.halfHeight;
		AddChild(sprite);
		
		organLayer = new ImOrganLayer();
		organLayer.x = Futile.screen.halfWidth;
		organLayer.y = Futile.screen.halfHeight;
		AddChild(organLayer);
		
		nodeLayer = new ImNodeLayer();
		nodeLayer.x = Futile.screen.halfWidth;
		nodeLayer.y = Futile.screen.halfHeight;
		AddChild(nodeLayer);
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
		bool touchedOrgan = false;
		foreach (ImBodyPart organ in organLayer.organs) {
			if (organ.SpriteContainsGlobalPoint(touch.position)) {
				touchedOrgan = true;
				if (currentOrgan != null) {
					if (currentOrgan == organ) break;
					currentOrgan.isSelected = false;
					currentOrgan = null;
				}
				currentOrgan = organ;
				currentOrgan.isSelected = true;
				return true;
			}
		}
		if (!touchedOrgan && currentOrgan != null) {
			currentOrgan.isSelected = false;
			currentOrgan = null;
		}
		
		return false;
	}
	
	public void HandleSingleTouchMoved(FTouch touch) {

	}
	
	public void HandleSingleTouchEnded(FTouch touch) {

	}
	
	public void HandleSingleTouchCanceled(FTouch touch) {
		
	}
}
