using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTFalldownTest : FStage, FSingleTouchableInterface {
	private List<WTEntity> fallingBlocks = new List<WTEntity>();
	private KeyCode lastKeyPressed = KeyCode.None;
	private FSprite background;
	
	public float upwardVelocity = 100f;
	public int verticalScreensMoved = 0;
	public float totalDistanceMoved = 0;
	public WTEntity guy = new WTEntity("whiteSquare.png");
	
	public float previousStageMovement = 0;
	
	public enum Direction {
		None,
		Right,
		Left
	}
	
	#region Init and Setup
	
	public WTFalldownTest() : base("") {
		background = SquareMaker.Square(Futile.screen.width, Futile.screen.height * 2.5f);
		background.x = Futile.screen.halfWidth;
		background.anchorY = 0;
		background.y = -Futile.screen.height * 0.25f;
		AddChild(background);
		
		guy.x = Futile.screen.halfWidth;
		guy.y = 500f;
		guy.yVelocity = 0;
		guy.sprite.color = new Color(0.2f, 0.4f, 0.8f, 1.0f);
		AddChild(guy);
		
		InitFallingBlocks();
	}
		
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	private void InitFallingBlocks() {
		for (int i = 0; i < 3; i++) {
			WTEntity block = new WTEntity("whiteSquare.png");
			block.x = Random.Range(block.sprite.textureRect.width / 2f * block.sprite.scale, Futile.screen.width - block.sprite.textureRect.width / 2f * block.sprite.scale);
			block.y = Random.Range(Futile.screen.height + block.sprite.textureRect.height / 2f * block.sprite.scale, Futile.screen.height * 2f + block.sprite.textureRect.height / 2f * block.sprite.scale);
			block.sprite.color = Color.red;
			fallingBlocks.Add(block);
			AddChild(block);
		}
	}
	
	#endregion
	
	#region Update Handlers
	
	public void HandleUpdate() {
		UpdateFallingBlockMovement();
		UpdateGuyYMovement();
		UpdateGuyXMovement();
		RecycleFallingBlocks();
	}
	
	private Direction GetDirectionForKeyInput() {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) lastKeyPressed = KeyCode.LeftArrow;
		else if (Input.GetKeyDown(KeyCode.RightArrow)) lastKeyPressed = KeyCode.RightArrow;
		else if (lastKeyPressed == KeyCode.LeftArrow && Input.GetKeyUp(KeyCode.LeftArrow)) {
			if (Input.GetKey(KeyCode.RightArrow)) lastKeyPressed = KeyCode.RightArrow;
			else lastKeyPressed = KeyCode.None;
		}
		else if (lastKeyPressed == KeyCode.RightArrow && Input.GetKeyUp(KeyCode.RightArrow)) {
			if (Input.GetKey(KeyCode.LeftArrow)) lastKeyPressed = KeyCode.LeftArrow;
			else lastKeyPressed = KeyCode.None;
		}
		
		if (lastKeyPressed == KeyCode.LeftArrow) return Direction.Left;
		else if (lastKeyPressed == KeyCode.RightArrow) return Direction.Right;
		else return Direction.None;
	}
	
	private void UpdateFallingBlockMovement() {
		float yDelta = -upwardVelocity * Time.deltaTime;
		float xDelta = 0;
		
		foreach (WTEntity block in fallingBlocks) {
			block.yPrevious = block.y;
			block.y += yDelta;
			block.xPrevious = block.x;
			block.x += xDelta;
		}
	}
	
	private void UpdateGuyYMovement() {
		if (Input.GetKey(KeyCode.UpArrow)) guy.yVelocity = 1000f;
		else if (Input.GetKey(KeyCode.DownArrow)) guy.yVelocity = -1000f;
		else guy.yVelocity = 0;
		
		
		float yDelta = guy.yVelocity * Time.deltaTime;
		float newY = guy.y + yDelta;
		float yNewGuyMin = newY - guy.sprite.height / 2f;
		float yCurrentGuyMin = guy.y - guy.sprite.height / 2f;
		float yNewGuyMax = yNewGuyMin + guy.sprite.height;
		float yCurrentGuyMax = yCurrentGuyMin + guy.sprite.height;

		Rect guyRect = guy.GetLocalSpriteRect(this);
		
		foreach (WTEntity block in fallingBlocks) {
			Rect blockRect = block.GetLocalSpriteRect(this);
			
			float yNewBlockMin = block.y - block.sprite.height / 2f;
			float yPreviousBlockMin = block.yPrevious - block.sprite.height / 2f;
			float yNewBlockMax = yNewBlockMin + block.sprite.height;
			float yPreviousBlockMax = yPreviousBlockMin + block.sprite.height;
			
			if (!(guyRect.xMin <= blockRect.xMax && blockRect.xMin <= guyRect.xMax)) continue;
			if (yCurrentGuyMax <= yPreviousBlockMin && yNewGuyMax >= yNewBlockMin) {
				newY = yNewBlockMin - guy.sprite.height / 2f;
				break;
			}
			if (yCurrentGuyMin >= yPreviousBlockMax && yNewGuyMin <= yNewBlockMax) {
				newY = yNewBlockMax + guy.sprite.height / 2f;
				Debug.Log(newY - yNewBlockMax);
				break;
			}
		}
		
		guy.yPrevious = guy.y;
		guy.y = newY;
	}
	
	private void UpdateGuyXMovement() {
		Direction dir = GetDirectionForKeyInput();
		
		if (dir == Direction.Right) guy.xVelocity = WTConfig.MAX_PLAYER_X_VELOCITY;
		else if (dir == Direction.Left) guy.xVelocity = -WTConfig.MAX_PLAYER_X_VELOCITY;
		else guy.xVelocity = 0;

		float xDelta = guy.xVelocity * Time.deltaTime;
		float newX = guy.x + xDelta;
		float xNewGuyMin = newX - guy.sprite.width / 2f;
		float xCurrentGuyMin = guy.x - guy.sprite.width / 2f;
		float xNewGuyMax = xNewGuyMin + guy.sprite.width;
		float xCurrentGuyMax = xCurrentGuyMin + guy.sprite.width;
		
		Rect guyRect = guy.GetLocalSpriteRect(this);
		
		foreach (WTEntity block in fallingBlocks) {
			Rect blockRect = block.GetLocalSpriteRect(this);
			
			float xNewBlockMin = block.x - block.sprite.width / 2f;
			float xPreviousBlockMin = block.xPrevious - block.sprite.width / 2f;
			float xNewBlockMax = xNewBlockMin + block.sprite.width;
			float xPreviousBlockMax = xPreviousBlockMin + block.sprite.width;
		
			if (!(guyRect.yMin <= blockRect.yMax && blockRect.yMin <= guyRect.yMax)) continue;
			if (xCurrentGuyMax <= xPreviousBlockMin && xNewGuyMax >= xNewBlockMin) {
				newX = xNewBlockMin - guy.sprite.width / 2f;
				break;
			}
			if (xCurrentGuyMin >= xPreviousBlockMax && xNewGuyMin <= xNewBlockMax) {
				newX = xNewBlockMax + guy.sprite.width / 2f;
				break;
			}
		}
		
		if (newX > Futile.screen.width - guy.sprite.width / 2f) newX = Futile.screen.width - guy.sprite.width / 2f;
		else if (newX < guy.sprite.width / 2f) newX = guy.sprite.width / 2f;
		
		guy.xPrevious = guy.x;
		guy.x = newX;
	}
	
	private void RecycleFallingBlocks() {			
		float localScreenMinY = GlobalToLocal(Vector2.zero).y;
		float localScreenMaxY = GlobalToLocal(new Vector2(0, Futile.screen.height)).y;
		
		foreach (WTEntity block in fallingBlocks) {
			float blockMinY = localScreenMinY - (block.sprite.textureRect.height / 2f * block.sprite.scale);
			float blockMaxY = localScreenMaxY + (block.sprite.textureRect.height / 2f * block.sprite.scale);
			
			if (block.y < blockMinY) {
				block.y = block.yPrevious = blockMaxY;
				block.x = Random.Range(block.sprite.textureRect.width / 2f * block.sprite.scale, Futile.screen.width - block.sprite.textureRect.width / 2f * block.sprite.scale);
				block.xPrevious = block.x;
			}
		}	
	}
	
	#endregion
		
	#region Touches
	
	public bool HandleSingleTouchBegan(FTouch touch) {
		return true;
	}
	
	public void HandleSingleTouchMoved(FTouch touch) {
		
	}
	
	public void HandleSingleTouchEnded(FTouch touch) {
		
	}
	
	public void HandleSingleTouchCanceled(FTouch touch) {
		
	}
		
	#endregion
}
