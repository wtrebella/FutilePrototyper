using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTHexagon : FStage {
	HexTriangle triangle;
	float currentTriangleAngle;
	FContainer stageLayer = new FContainer();
	List<FNode> gameObjects = new List<FNode>();
	List<HexSliceEntity> sliceEntities = new List<HexSliceEntity>();
	bool isGameOver = false;
	
	public WTHexagon() : base("") {		
		stageLayer.x = Futile.screen.halfWidth;
		stageLayer.y = Futile.screen.halfHeight;
		AddChild(stageLayer);
		
		Color color1 = new Color(0.3f, 0.3f, 0.3f, 1.0f);
		Color color2 = new Color(0.33f, 0.33f, 0.33f, 1.0f);
		bool isColor1 = true;
		
		for (int i = 0; i < 6; i++) {
			Color actualColor;
			if (isColor1) actualColor = color1;
			else actualColor = color2;
			
			HexSliceEntity sliceEntity = new HexSliceEntity("sliceEntity", actualColor);
			sliceEntity.rotation = i * 60;
			sliceEntities.Add(sliceEntity);
			stageLayer.AddChild(sliceEntity);
			isColor1 = !isColor1;
		}
		
		HexCenterHexagon ch = new HexCenterHexagon("centerHexagon");
		stageLayer.AddChild(ch);
		
		triangle = new HexTriangle("triangle");
		stageLayer.AddChild(triangle);
		
		gameObjects.Add(triangle);
		
		PlaceTriangleAtAngle(45);
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}

	float timer1 = 0.8f;
	float timer2 = 0;
	int lastGroup = 1;
	float stageRotationVelocity = 100f;
	
	public void HandleUpdate () {
		if (isGameOver) {
			if (Input.GetKeyDown(KeyCode.Space)) WTMain.instance.SwitchToScene(WTMain.SceneType.Hexagon);
			return;
		}
		
		stageLayer.rotation += stageRotationVelocity * Time.fixedDeltaTime;
		if (Random.Range(0, 200) == 0) stageRotationVelocity *= -1;
		
		foreach (HexSliceEntity hse in sliceEntities) hse.MoveDownObstacles(300f);
		
		timer1 += Time.fixedDeltaTime;
		timer2 += Time.fixedDeltaTime;
				
		if (lastGroup == 1 && timer1 >= 1.0f * Random.Range(0.45f, 1.5f)) {
			timer1 = 0;
			timer2 = 0;
			lastGroup = 0;
			sliceEntities[0].AddNewCrossbar(Random.Range(25f, 75f));
			sliceEntities[2].AddNewCrossbar(Random.Range(25f, 75f));
			sliceEntities[4].AddNewCrossbar(Random.Range(25f, 75f));
		}
		
		if (lastGroup == 0 && timer2 >= 1.0f * Random.Range(0.45f, 1.5f)) {
			timer1 = 0;
			timer2 = 0;
			lastGroup = 1;
			sliceEntities[1].AddNewCrossbar(Random.Range(25f, 75f));
			sliceEntities[3].AddNewCrossbar(Random.Range(25f, 75f));
			sliceEntities[5].AddNewCrossbar(Random.Range(25f, 75f));
		}
		
		float rotationSpeed = 350f;
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			PlaceTriangleAtAngle(currentTriangleAngle - rotationSpeed * Time.fixedDeltaTime);
		}
		
		else if (Input.GetKey(KeyCode.RightArrow)) {
			PlaceTriangleAtAngle(currentTriangleAngle + rotationSpeed * Time.fixedDeltaTime);
		}
				
		HandleCollisions();
	}
	
	void HandleCollisions() {
		HexSliceEntity slice = sliceEntities[CurrentSliceEntityOfTriangle()];
		
		foreach (WTSpriteComponent obstacle in slice.obstacleComponents) {
			HexCrossBar hcb = (HexCrossBar)obstacle.sprite;
			if (hcb.distanceFromBackgroundSliceOrigin <= 58f && hcb.previousDistanceFromBackgroundSliceOrigin > 58f) {
				isGameOver = true;
				Go.to(triangle, 0.5f, new TweenConfig().setIterations(-1).setEaseType(EaseType.Linear).floatProp("rotation", 360, true));
				return;
			}
		}
	}
	
	int CurrentSliceEntityOfTriangle() {
		float rot = (int)currentTriangleAngle % 360;
		if (rot < 0) rot = rot + 360;
		return (int)(rot / 60);
	}
	
	public void PlaceTriangleAtAngle(float angle) {
		currentTriangleAngle = angle;
		
		triangle.rotation = angle;

		angle *= -1;
		
		float radius = 50f;
		
		triangle.x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
		triangle.y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
	}
}
