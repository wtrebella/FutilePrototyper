using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTEntity : FContainer {
	private FSprite sprite_;
	public WTFallingBlockComponent fallingBlockComponent;
	
	public float xVelocity = 0;
	public float yVelocity = 0;
	
	public float xPrevious = 0;
	public float yPrevious = 0;
	
	public WTEntity(string imageName, WTFallingBlockComponent fallingBlockComponent) {
		if (fallingBlockComponent != null) {
			this.fallingBlockComponent = fallingBlockComponent;
			fallingBlockComponent.owner = this;
		}
		
		sprite_ = new FSprite(imageName);
		AddChild(sprite_);
	}
	
	public FSprite sprite {
		get {return sprite_;}
	}
	
	public Rect GetGlobalSpriteRect() {
		Vector2 globalOrigin = sprite_.LocalToGlobal(new Vector2(sprite_.x, sprite_.y));
		return new Rect(globalOrigin.x, globalOrigin.y, sprite_.width, sprite_.height);
	}
	
	public Rect GetLocalSpriteRect(FNode localNode) {
		Rect globalRect = GetGlobalSpriteRect();
		Vector2 globalOrigin = new Vector2(globalRect.x, globalRect.y);
		Vector2 localOrigin = localNode.GlobalToLocal(globalOrigin);
		return new Rect(localOrigin.x, localOrigin.y, sprite_.width, sprite_.height);
	}
}

public class WTFallingBlockComponent {
	public bool isBeingUsed = false;
	public WTEntity owner;
	
	public WTFallingBlockComponent() {
		
	}
}

public class WTEntityConstruct : List<WTEntity> {
	public List<WTEntity> entities;
	
	public WTEntityConstruct(List<WTEntity> entities) {
		if (entities == null) this.entities = new List<WTEntity>();
		else this.entities = entities;
	}
}