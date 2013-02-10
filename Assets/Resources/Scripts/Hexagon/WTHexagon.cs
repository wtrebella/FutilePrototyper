using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTHexagon : FStage {
	HexTriangle triangle;
	float currentTriangleAngle;
	FContainer stageLayer = new FContainer();
	FContainer playerLayer = new FContainer();
	List<FNode> gameObjects = new List<FNode>();
	HexSliceEntity sliceEntity;
	
	public WTHexagon() : base("") {		
		stageLayer.x = Futile.screen.halfWidth;
		stageLayer.y = Futile.screen.halfHeight;
		AddChild(stageLayer);
		
		playerLayer.x = Futile.screen.halfWidth;
		playerLayer.y = Futile.screen.halfHeight;
		AddChild(playerLayer);
		
		//HexBackground background = new HexBackground();
		//stageLayer.AddChild(background);
		
		sliceEntity = new HexSliceEntity("sliceEntity", new Color(0.3f, 0.3f, 0.3f, 1.0f));
		stageLayer.AddChild(sliceEntity);
		
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
		
		sliceEntity.MoveDownObstacles(100f);
		
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
