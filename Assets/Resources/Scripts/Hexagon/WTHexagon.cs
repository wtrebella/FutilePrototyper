using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTHexagon : FStage {
	HexTriangle triangle;
	float currentTriangleAngle;
	FContainer stageLayer = new FContainer();
	FContainer playerLayer = new FContainer();
	List<FNode> gameObjects = new List<FNode>();
	List<HexSliceEntity> sliceEntities = new List<HexSliceEntity>();
	
	public WTHexagon() : base("") {		
		stageLayer.x = Futile.screen.halfWidth;
		stageLayer.y = Futile.screen.halfHeight;
		AddChild(stageLayer);
		
		playerLayer.x = Futile.screen.halfWidth;
		playerLayer.y = Futile.screen.halfHeight;
		AddChild(playerLayer);
		
		//HexBackground background = new HexBackground();
		//stageLayer.AddChild(background);
		
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
		playerLayer.AddChild(triangle);
		
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

	public void HandleUpdate () {
		//stageLayer.rotation += 100f * Time.fixedDeltaTime;
		
		foreach (HexSliceEntity hse in sliceEntities) hse.MoveDownObstacles(100f);
		
		float rotationSpeed = 350f;
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			PlaceTriangleAtAngle(currentTriangleAngle - rotationSpeed * Time.fixedDeltaTime);
		}
		
		else if (Input.GetKey(KeyCode.RightArrow)) {
			PlaceTriangleAtAngle(currentTriangleAngle + rotationSpeed * Time.fixedDeltaTime);
		}
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
