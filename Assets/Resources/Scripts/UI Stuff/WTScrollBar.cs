using UnityEngine;
using System.Collections;

public class WTScrollBar : WTEntity {
	private WTSliceSpriteComponent mainSpriteComponent;
	
	private float height_;
	
	public WTScrollBar(string name = "scroll bar!") : base(name) {
		mainSpriteComponent = new WTSliceSpriteComponent("mainSliceSpriteComponent", "scrollBar.psd", 50f/4f, 150f/4f, 52f/4f, 0, 52f/4f, 0);
		mainSpriteComponent.sprite.anchorX = 0;
		mainSpriteComponent.sprite.anchorY = 0;
		this.height = 100f;
		AddComponent(mainSpriteComponent);
	}
	
	public float height {
		get {return height_;}
		set {
			if (height_ != value) {
				height_ = value;
				mainSpriteComponent.sprite.height = height_;
			}
		}
	}
	
	public float width {
		get {return mainSpriteComponent.sprite.width;}
		set {mainSpriteComponent.sprite.width = value;}
	}
}
