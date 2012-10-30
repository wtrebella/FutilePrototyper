using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTFalldownTest : FStage, FSingleTouchableInterface {
	private List<WTEntity> fallingBlocks = new List<WTEntity>();
	private KeyCode lastKeyPressed = KeyCode.None;
	private FSprite background;
	
	public int score = 0;
	public FLabel scoreLabel;
	public float totalDistanceTraveled = 0;
	public int totalTilesTraveled = 0;
	public int lastConstructCreationPoint = -1;
	public bool isGameOver = false;
	public float upwardVelocity = 800f;
	public int verticalScreensMoved = 0;
	public WTEntity guy;
	public List<WTEntityConstruct> constructs;
	
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
		
		scoreLabel = new FLabel("SoftSugar", "0");
		scoreLabel.scale = 0.5f;
		scoreLabel.color = Color.black;
		scoreLabel.anchorX = 1;
		scoreLabel.anchorY = 1;
		scoreLabel.x = Futile.screen.width - 5;
		scoreLabel.y = Futile.screen.height - 5;
		AddChild(scoreLabel);
		
		guy = new WTEntity("whiteSquare.png", null);
		guy.sprite.width = guy.sprite.height = WTConfig.TILE_SIZE;
		guy.x = Futile.screen.halfWidth;
		guy.y = 200f;
		guy.yVelocity = 0;
		guy.sprite.color = new Color(0.2f, 0.4f, 0.8f, 1.0f);
		AddChild(guy);
		
		constructs = new List<WTEntityConstruct>();
	
		InitFallingBlocks();
	}
		
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.touchManager.AddSingleTouchTarget(this);
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.touchManager.RemoveSingleTouchTarget(this);
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	private void InitFallingBlocks() {
		for (int i = 0; i < 50; i++) {
			WTEntity block = new WTEntity("whiteSquare.png", new WTFallingBlockComponent());
			block.sprite.width = block.sprite.height = WTConfig.TILE_SIZE;
			block.x = Random.Range(block.sprite.textureRect.width / 2f * block.sprite.scale, Futile.screen.width - block.sprite.textureRect.width / 2f * block.sprite.scale);
			block.y = Random.Range(Futile.screen.height + block.sprite.textureRect.height / 2f * block.sprite.scale, Futile.screen.height * 2f + block.sprite.textureRect.height / 2f * block.sprite.scale);
			block.sprite.color = Color.red;
			fallingBlocks.Add(block);
			AddChild(block);
		}
		
		/*for (int i = 0; i < 6; i++) {
			WTEntity block = GetUnusedFallingBlock();
			if (block == null) continue;
			block.fallingBlockComponent.isBeingUsed = true;
		}*/
	}
	
	#endregion
	
	#region Update Handlers
	
	public void HandleUpdate() {
		if (isGameOver) {
			if (Input.GetKeyDown(KeyCode.Space)) WTMain.instance.SwitchToScene(WTMain.SceneType.FalldownTest);
			return;
		}
		totalDistanceTraveled += Time.deltaTime;
		totalTilesTraveled = (int)(totalDistanceTraveled * 1000f / WTConfig.TILE_SIZE);
		//if (totalTilesTraveled % 50 == 0) upwardVelocity += 100f;
		score = (int)(totalDistanceTraveled * 1000f);
		scoreLabel.text = string.Format("{0}", score);
		
		if (totalTilesTraveled % 20 == 0 && totalTilesTraveled != lastConstructCreationPoint) {
			/*if (totalTilesTraveled % 40 == 0) {
				int[][] constructData = new int [2][];

				constructData[0] = new int[] {1, 1, 1, 1, 1, 1};
				constructData[1] = new int[] {0, 0, 0, 0, 0, 1};
				
				MakeFallingBlockConstruct(constructData);
			}*/
			//else {
				int[][] constructData = new int [2][];

				constructData[0] = new int[] {1, 1, 1, 1, 1};
				constructData[1] = new int[] {1, 0, 0, 0, 1};
				
				MakeFallingBlockConstruct(constructData, Random.Range(0, WTConfig.GRID_WIDTH - 5 + 1));
			//}
		}
		
		UpdateFallingBlockMovement();
		UpdateGuyYMovement();
		UpdateGuyXMovement();
		CheckAndRecycleFinishedConstructs();
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
		/*if (GetNumberOfFallingBlocksBeingUsed() < 6) {
			WTEntity block = GetUnusedFallingBlock();
		
			if (block != null) {
				block.fallingBlockComponent.isBeingUsed = true;
			}
		}*/
		
		float yDelta = -upwardVelocity * Time.deltaTime;
		float xDelta = 0;
		
		foreach (WTEntity block in fallingBlocks) {
			if (!block.fallingBlockComponent.isBeingUsed) continue;
			block.yPrevious = block.y;
			block.y += yDelta;
			block.xPrevious = block.x;
			block.x += xDelta;
		}
	}
	
	private void UpdateGuyYMovement() {
		/*if (Input.GetKey(KeyCode.UpArrow)) guy.yVelocity = 1000f;
		else if (Input.GetKey(KeyCode.DownArrow)) guy.yVelocity = -1000f;
		else guy.yVelocity = 0;*/
		
		float yDelta = guy.yVelocity * Time.deltaTime;
		float newY = guy.y + yDelta;
		float yNewGuyMin = newY - guy.sprite.height / 2f;
		float yCurrentGuyMin = guy.y - guy.sprite.height / 2f;
		float yNewGuyMax = yNewGuyMin + guy.sprite.height;
		float yCurrentGuyMax = yCurrentGuyMin + guy.sprite.height;

		Rect guyRect = guy.GetLocalSpriteRect(this);
		
		foreach (WTEntity block in fallingBlocks) {
			if (!block.fallingBlockComponent.isBeingUsed) continue;
			Rect blockRect = block.GetLocalSpriteRect(this);
			
			float yNewBlockMin = block.y - block.sprite.height / 2f;
			float yPreviousBlockMin = block.yPrevious - block.sprite.height / 2f;
			float yNewBlockMax = yNewBlockMin + block.sprite.height;
			float yPreviousBlockMax = yPreviousBlockMin + block.sprite.height;
			
			if (!(guyRect.xMin <= blockRect.xMax && blockRect.xMin <= guyRect.xMax)) continue;
			if (yCurrentGuyMax <= yPreviousBlockMin && yNewGuyMax >= yNewBlockMin) {
				newY = yNewBlockMin - guy.sprite.height / 2f - 0.0001f;
				break;
			}
			if (yCurrentGuyMin >= yPreviousBlockMax && yNewGuyMin <= yNewBlockMax) {
				newY = yNewBlockMax + guy.sprite.height / 2f + 0.0001f;
				break;
			}
		}
		
		guy.yPrevious = guy.y;
		guy.y = newY;
		
		if (guy.y < -guy.sprite.height / 2f) {
			isGameOver = true;
			FLabel label = new FLabel("SoftSugar", "Game Over!");
			label.x = Futile.screen.halfWidth;
			label.y = Futile.screen.halfHeight;
			label.color = Color.black;
			AddChild(label);
			
			label = new FLabel("SoftSugar", "Hit space to try again");
			label.x = Futile.screen.halfWidth;
			label.y = Futile.screen.halfHeight - 50;
			label.scale = 0.5f;
			label.color = Color.black;
			AddChild(label);
		}
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
			if (!block.fallingBlockComponent.isBeingUsed) continue;
			Rect blockRect = block.GetLocalSpriteRect(this);
			
			float xNewBlockMin = block.x - block.sprite.width / 2f;
			float xPreviousBlockMin = block.xPrevious - block.sprite.width / 2f;
			float xNewBlockMax = xNewBlockMin + block.sprite.width;
			float xPreviousBlockMax = xPreviousBlockMin + block.sprite.width;
		
			if (!(guyRect.yMin <= blockRect.yMax && blockRect.yMin <= guyRect.yMax)) continue;
			if (xCurrentGuyMax <= xPreviousBlockMin && xNewGuyMax >= xNewBlockMin) {
				newX = xNewBlockMin - guy.sprite.width / 2f - 0.0001f;
				break;
			}
			if (xCurrentGuyMin >= xPreviousBlockMax && xNewGuyMin <= xNewBlockMax) {
				newX = xNewBlockMax + guy.sprite.width / 2f + 0.0001f;
				break;
			}
		}
		
		if (newX > Futile.screen.width - guy.sprite.width / 2f) newX = Futile.screen.width - guy.sprite.width / 2f;
		else if (newX < guy.sprite.width / 2f) newX = guy.sprite.width / 2f;
		
		guy.xPrevious = guy.x;
		guy.x = newX;
	}
	
	private void CheckAndRecycleFinishedConstructs() {					
		for (int i = constructs.Count - 1; i >= 0; i--) {
			WTEntityConstruct construct = constructs[i];
			
			float localScreenMinY = GlobalToLocal(Vector2.zero).y;
			float localScreenMaxY = GlobalToLocal(new Vector2(0, Futile.screen.height)).y;
			
			bool allAreOutOfBounds = true;
			
			foreach (WTEntity block in construct) {
				float blockMinY = localScreenMinY - (block.sprite.textureRect.height / 2f * block.sprite.scale);
				
				if (block.y >= blockMinY) {
					allAreOutOfBounds = false;
					break;
				}
			}	
			
			if (allAreOutOfBounds) {
				foreach (WTEntity block in construct) {
					float blockMaxY = localScreenMaxY + (block.sprite.textureRect.height / 2f * block.sprite.scale);
					block.y = block.yPrevious = blockMaxY;
					block.x = Futile.screen.halfWidth;//Random.Range(block.sprite.textureRect.width / 2f * block.sprite.scale, Futile.screen.width - block.sprite.textureRect.width / 2f * block.sprite.scale);
					block.xPrevious = block.x;
					block.fallingBlockComponent.isBeingUsed = false;
				}
				
				constructs.Remove(construct);
			}
		}
	}
	
	#endregion
	
	#region Falling Blocks
	
	private WTEntity GetUnusedFallingBlock() {
		foreach (WTEntity block in fallingBlocks) {
			if (!block.fallingBlockComponent.isBeingUsed) return block;
		}
		
		Debug.Log("No unused blocks");
		return null;
	}
	
	private int GetNumberOfFallingBlocksBeingUsed() {
		int num = 0;
		
		foreach (WTEntity block in fallingBlocks) {
			if (block.fallingBlockComponent.isBeingUsed) num++;	
		}
		
		return num;
	}
	
	private void MakeFallingBlockConstruct(int[][] data, int offset = 0) {
		lastConstructCreationPoint = totalTilesTraveled;
		
		WTEntityConstruct construct = new WTEntityConstruct(null);
		
		if (offset > 0) {
			int[][] newData = new int[data.Length][];
			for (int i = 0; i < data.Length; i++) {
				if (offset + data[i].Length > WTConfig.GRID_WIDTH) {
					Debug.Log("Offset too large!");
					return;
				}
				newData[i] = new int[offset + data[i].Length];
				for (int j = 0; j < offset; j++) {
					newData[i][j] = 0;
				}
				for (int j = offset; j < offset + data[i].Length; j++) {
					newData[i][j] = data[i][j - offset];
				}
			}
			data = newData;
		}
		
		for (int i = data.Length - 1; i >= 0; i--) {
			int[] dataRow = data[i];
			if (dataRow.Length > WTConfig.GRID_WIDTH) {
				Debug.Log("Invalid falling block construct. Wider than grid!");
				return;
			}
						
			float yBase = Futile.screen.height + WTConfig.TILE_SIZE;
			
			for (int j = 0; j < dataRow.Length; j++) {
				int dataItem = dataRow[j];
				
				if (dataItem == 0) continue;
				
				WTEntity block = GetUnusedFallingBlock();
				
				if (block == null) continue;
				
				block.fallingBlockComponent.isBeingUsed = true;
				
				block.x = WTConfig.TILE_SIZE * j + block.sprite.width / 2f;
				block.y = yBase + WTConfig.TILE_SIZE * (data.Length - i - 1) + block.sprite.height / 2f;
				
				construct.Add(block);
			}
		}
		
		if (construct.Count > 0) constructs.Add(construct);
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
