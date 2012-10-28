using UnityEngine;
using System.Collections;

public class WTEntity : FContainer {
	private FSprite sprite_;
	
	public float xVelocity = 0;
	public float yVelocity = 0;
	
	public float xPrevious = 0;
	public float yPrevious = 0;
	
	public WTEntity(string imageName) {
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
	
	/*public Rect GetGlobalSpriteRectIfItsYOri() {
		Vector2 globalOrigin = sprite_.LocalToGlobal(new Vector2(sprite_.x, sprite_.y));
		Vector2 previousGlobalOrigin = new Vector2(globalOrigin.x - (x - xPrevious), globalOrigin.y - (y - yPrevious));
		return new Rect(previousGlobalOrigin.x, previousGlobalOrigin.y, sprite_.width, sprite_.height);
	}
	
	public Rect GetLocalSpritePreviousRect(FNode localNode) {
		Rect previousGlobalRect = GetGlobalSpritePreviousRect();
		Vector2 previousGlobalOrigin = new Vector2(previousGlobalRect.xMin, previousGlobalRect.yMin);
		Vector2 previousLocalOrigin = localNode.GlobalToLocal(previousGlobalOrigin);
		return new Rect(previousLocalOrigin.x, previousLocalOrigin.y, sprite_.width, sprite_.height);
	}*/
}
