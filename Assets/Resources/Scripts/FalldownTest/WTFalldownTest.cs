using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTFalldownTest : FStage, FSingleTouchableInterface {
	private KeyCode lastKeyPressed = KeyCode.None;
	private FSprite background;
	private WTEntityManager entityManager;
	
	public int score = 0;
	public FLabel scoreLabel;
	public float totalDistanceTraveled = 0;
	public int totalTilesTraveled = 0;
	public int lastConstructCreationPoint = -1;
	public bool isGameOver = false;
	public float upwardVelocity = 10f;
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
		background = WTSquareMaker.Square(Futile.screen.width, Futile.screen.height * 2.5f);
		background.x = Futile.screen.halfWidth;
		background.anchorY = 0;
		background.y = -Futile.screen.height * 0.25f;
		AddChild(background);
		
		//WTDiagBarsLayer diagBars = new WTDiagBarsLayer();
		//AddChild(diagBars);
		
		guy = new WTEntity("square.psd", null);
		guy.sprite.width = guy.sprite.height = WTConfig.TILE_SIZE / 2f;
		guy.x = Futile.screen.halfWidth;
		guy.y = 400f;
		guy.yVelocity = 0;
		guy.sprite.color = new Color(0.2f, 0.4f, 0.8f, 1.0f);
		AddChild(guy);
		
		constructs = new List<WTEntityConstruct>();
	
		entityManager = new WTEntityManager();
						
		scoreLabel = new FLabel("SoftSugar", "0");
		scoreLabel.scale = 0.5f;
		scoreLabel.color = Color.black;
		scoreLabel.anchorX = 1;
		scoreLabel.anchorY = 1;
		scoreLabel.x = Futile.screen.width - 5;
		scoreLabel.y = Futile.screen.height - 5;
		AddChild(scoreLabel);
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
	
	#endregion
	
	#region Update Handlers
	
	public void HandleUpdate() {		
		if (isGameOver) {
			if (Input.GetKeyDown(KeyCode.Space)) WTMain.instance.SwitchToScene(WTMain.SceneType.FalldownTest);
			return;
		}
		
		totalDistanceTraveled += Time.deltaTime * upwardVelocity;
		totalTilesTraveled = (int)(totalDistanceTraveled / WTConfig.TILE_SIZE);
		if (totalTilesTraveled % 50 == 0) upwardVelocity += 10f;
		score = totalTilesTraveled * 10;
		scoreLabel.text = string.Format("{0}", score);
			
		UpdateConstructs();
		UpdateConstructMovement();
		UpdateGuyYMovement();
		UpdateGuyXMovement();
	}
	
	private void UpdateConstructs() {
		for (int i = constructs.Count - 1; i >= 0; i--) {
			WTEntityConstruct construct = constructs[i];

			float localScreenMinY = GlobalToLocal(Vector2.zero).y;
			float localScreenMinX = GlobalToLocal(Vector2.zero).x;
			float localScreenMaxX = GlobalToLocal(new Vector2(Futile.screen.width, 0)).x;
			
			foreach (WTEntity block in construct.entities) {
				float blockMinX = localScreenMinX - block.sprite.width * 1.5f;
				float blockMaxX = localScreenMaxX + block.sprite.width * 1.5f;
				
				Vector2 globalBlockPos = block.container.GlobalToLocal(new Vector2(block.x, block.y));
				
				if (globalBlockPos.x <= blockMinX) globalBlockPos.x = Futile.screen.width + block.sprite.width / 2f + (globalBlockPos.x - blockMinX);
				else if (globalBlockPos.x >= blockMaxX) globalBlockPos.x = -block.sprite.width / 2f + (globalBlockPos.x - blockMaxX);
			}
			
			bool allAreOutOfBounds = true;
			
			foreach (WTEntity block in construct.entities) {
				float blockMinY = localScreenMinY - (block.sprite.height / 2f * block.sprite.scale);
				
				Vector2 globalBlockPos = block.container.GlobalToLocal(new Vector2(block.x, block.y));

				if (globalBlockPos.y >= blockMinY) {
					allAreOutOfBounds = false;
					break;
				}
			}	
			
			if (allAreOutOfBounds) {
				foreach (WTEntity block in construct.entities) {
					entityManager.RecycleEntity(block);
				}
				
				RemoveChild(construct);
				constructs.Remove(construct);
			}
		}
		
		int spaceBetwenConstructs = Random.Range(5, 20);
		if (totalTilesTraveled > lastConstructCreationPoint + spaceBetwenConstructs) {
			ConstructType type = (ConstructType)Random.Range(1, 4);
			int offset = Random.Range(0, WTConfig.GRID_WIDTH - 4);
			RotationType rotation = (RotationType)Random.Range(0, 4);
			float xVelocity = Random.Range(-100f, 100f);
			float yVelocity = Random.Range(-100f, 100f) - upwardVelocity;
			MakeConstruct(type, rotation, xVelocity, yVelocity, offset);
		}
	}
	
	private void UpdateConstructMovement() {		
		foreach (WTEntityConstruct construct in constructs) {			
			float xDelta = construct.xVelocity * Time.deltaTime;
			float yDelta = construct.yVelocity * Time.deltaTime;
			construct.MoveTo(construct.x + xDelta, construct.y + yDelta);
		}
	}
	
	private void UpdateGuyYMovement() {
		/*if (Input.GetKey(KeyCode.UpArrow)) guy.yVelocity = 1000f;
		else if (Input.GetKey(KeyCode.DownArrow)) guy.yVelocity = -1000f;
		else guy.yVelocity = 0;*/
		
		Vector2 globalGuyPos = guy.container.LocalToGlobal(new Vector2(guy.x, guy.y));
		
		float yDelta = guy.yVelocity * Time.deltaTime;
		float newY = globalGuyPos.y + yDelta;
		float yNewGuyMin = newY - guy.sprite.height / 2f;
		float yCurrentGuyMin = globalGuyPos.y - guy.sprite.height / 2f;
		float yNewGuyMax = yNewGuyMin + guy.sprite.height;
		float yCurrentGuyMax = yCurrentGuyMin + guy.sprite.height;
				
		foreach (WTEntity block in entityManager.fallingBlocks) {
			if (!block.fallingBlockComponent.isBeingUsed) continue;
			
			Vector2 globalBlockPos = block.container.LocalToGlobal(new Vector2(block.x, block.y));
			// have to make the bounds at the end of the screen different too
			
			float yNewBlockMin = globalBlockPos.y - block.sprite.height / 2f;
			float yPreviousBlockMin = block.yPreviousGlobal - block.sprite.height / 2f;
			float yNewBlockMax = yNewBlockMin + block.sprite.height;
			float yPreviousBlockMax = yPreviousBlockMin + block.sprite.height;
						
			float xGuyMin = globalGuyPos.x - guy.sprite.width / 2f;
			float xGuyMax = xGuyMin + guy.sprite.width;
			float xBlockMin = globalBlockPos.x - block.sprite.width / 2f;
			float xBlockMax = xBlockMin + block.sprite.width;
						
			if (!(xGuyMin <= xBlockMax && xGuyMax >= xBlockMin)) continue;

			if (yCurrentGuyMax <= yPreviousBlockMin && yNewGuyMax >= yNewBlockMin) {
				newY = yNewBlockMin - guy.sprite.height / 2f - 0.001f;
				break;
			}
			
			if (yCurrentGuyMin >= yPreviousBlockMax && yNewGuyMin <= yNewBlockMax) {
				newY = yNewBlockMax + guy.sprite.height / 2f + 0.001f;
				break;
			}
		}
		
		Vector2 localGuyPos = guy.container.GlobalToLocal(new Vector2(globalGuyPos.x, newY));
		
		guy.yPreviousGlobal = globalGuyPos.y;
		guy.y = localGuyPos.y;
				
		if (globalGuyPos.y < -guy.sprite.height / 2f) {
			EndGame();
		}
	}
	
	private void UpdateGuyXMovement() {
		Direction dir = GetDirectionForKeyInput();

		if (dir == Direction.Right) guy.xVelocity = WTConfig.MAX_PLAYER_X_VELOCITY;
		else if (dir == Direction.Left) guy.xVelocity = -WTConfig.MAX_PLAYER_X_VELOCITY;
		else guy.xVelocity = 0;
		
		Vector2 globalGuyPos = guy.container.LocalToGlobal(new Vector2(guy.x, guy.y));

		float xDelta = guy.xVelocity * Time.deltaTime;
		float newX = globalGuyPos.x + xDelta;
		float xNewGuyMin = newX - guy.sprite.width / 2f;
		float xCurrentGuyMin = globalGuyPos.x - guy.sprite.width / 2f;
		float xNewGuyMax = xNewGuyMin + guy.sprite.width;
		float xCurrentGuyMax = xCurrentGuyMin + guy.sprite.width;
		
		foreach (WTEntity block in entityManager.fallingBlocks) {
			if (!block.fallingBlockComponent.isBeingUsed) continue;
			
			Vector2 globalBlockPos = block.container.LocalToGlobal(new Vector2(block.x, block.y));
			
			float xNewBlockMin = globalBlockPos.x - block.sprite.width / 2f;
			float xPreviousBlockMin = block.xPreviousGlobal - block.sprite.width / 2f;
			float xNewBlockMax = xNewBlockMin + block.sprite.width;
			float xPreviousBlockMax = xPreviousBlockMin + block.sprite.width;
		
			if (!(globalGuyPos.y - guy.sprite.height / 2f <= globalBlockPos.y + block.sprite.height / 2f && globalBlockPos.y - block.sprite.height / 2f <= globalGuyPos.y + guy.sprite.height / 2f)) continue;
			if (xCurrentGuyMax <= xPreviousBlockMin && xNewGuyMax >= xNewBlockMin) {
				newX = xNewBlockMin - guy.sprite.width / 2f - 0.001f;
				break;
			}
			if (xCurrentGuyMin >= xPreviousBlockMax && xNewGuyMin <= xNewBlockMax) {
				newX = xNewBlockMax + guy.sprite.width / 2f + 0.001f;
				break;
			}
		}
		
		if (newX > Futile.screen.width - guy.sprite.width / 2f) newX = Futile.screen.width - guy.sprite.width / 2f;
		else if (newX < guy.sprite.width / 2f) newX = guy.sprite.width / 2f;
		
		Vector2 localGuyPos = guy.container.GlobalToLocal(new Vector2(newX, globalGuyPos.y));
	
		guy.xPreviousGlobal = globalGuyPos.x;
		guy.x = localGuyPos.x;
	}
	
	#endregion
	
	#region Falling Blocks
	
	private void MakeConstruct(ConstructType type, RotationType rotation, float xVelocity, float yVelocity, int offset = 0) {	
		if (type == ConstructType.DottedLine) {
			offset = 0;
			rotation = RotationType.Rotation0;
		}
		
		int[][] data = WTEntityConstructData.ConstructDataArray(type, rotation);
		
		if (data == null) {
			Debug.Log("data is null");
			return;
		}
		
		lastConstructCreationPoint = totalTilesTraveled;
		
		List<WTEntity> entityArray = new List<WTEntity>();
		
		for (int i = data.Length - 1; i >= 0; i--) {
			int[] dataRow = data[i];
			if (dataRow == null) {
				continue;
			}
			if (dataRow.Length > WTConfig.GRID_WIDTH) {
				Debug.Log("Invalid falling block construct. Wider than grid!");
				return;
			}
						
			float yBase = Futile.screen.height + WTConfig.TILE_SIZE;
			
			for (int j = 0; j < dataRow.Length; j++) {
				int dataItem = dataRow[j];
				
				if (dataItem == 0) continue;
				
				WTEntity block = entityManager.GetUnusedFallingBlock();
				
				if (block == null) continue;
							
				block.x = WTConfig.TILE_SIZE * (j + offset) + block.sprite.width / 2f;
				block.y = yBase + WTConfig.TILE_SIZE * (data.Length - i - 1) + block.sprite.height / 2f;
				
				entityArray.Add(block);
			}
			
			float rand = RXRandom.Float();
			int colors = 5;
			float interval = 1f / (float)colors;
			Color color;
			
			if (rand < interval) {
				color = Color.red;
			}
			else if (rand >= interval && rand < interval * 2f) {
				color = Color.green;
			}
			else if (rand >= interval * 2f && rand < interval * 3f) {
				color = Color.magenta;
			}
			else if (rand >= interval * 3f && rand < interval * 4f) {
				color = Color.yellow;
			}
			else {
				color = Color.blue;
			}
			
			foreach (WTEntity block in entityArray) block.sprite.color = color;		
		}
		
		if (entityArray.Count > 0) {
			WTEntityConstruct construct = new WTEntityConstruct(entityArray);
			AddChild(construct);
			construct.xVelocity = xVelocity;
			construct.yVelocity = yVelocity;
			constructs.Add(construct);
		}
	}
	
	#endregion
	
	#region Game Flow
	
	private void EndGame() {
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
	
	#endregion
		
	#region Touches and Input
	
	public bool HandleSingleTouchBegan(FTouch touch) {
		return true;
	}
	
	public void HandleSingleTouchMoved(FTouch touch) {
		
	}
	
	public void HandleSingleTouchEnded(FTouch touch) {
		
	}
	
	public void HandleSingleTouchCanceled(FTouch touch) {
		
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
	
		
	#endregion
}
