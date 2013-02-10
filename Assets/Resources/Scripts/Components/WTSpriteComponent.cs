using UnityEngine;
using System.Collections;

public class WTSpriteComponent : WTAbstractComponent {	
	private FSprite sprite_;
	private TweenChain spritePulsateTween_;
	
	public WTSpriteComponent(string name, string imageName) : base(name) {
		componentType_ = ComponentType.Sprite;
		
		sprite_ = new FSprite(imageName);
	}
	
	public WTSpriteComponent(string name, FSprite sprite) : base(name) {
		componentType_ = ComponentType.Sprite;
		
		sprite_ = sprite;
	}
	
	public bool SpriteContainsGlobalPoint(Vector2 globalPoint) {
		return sprite_.localRect.Contains(sprite_.GlobalToLocal(globalPoint));
	}
	
	public FSprite sprite {
		get {return sprite_;}	
	}
}
