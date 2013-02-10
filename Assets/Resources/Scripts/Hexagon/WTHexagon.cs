using UnityEngine;
using System.Collections;

public class WTHexagon : FStage {
	HexTriangle triangle;
	float currentTriangleAngle;
	
	public WTHexagon() : base("") {
		HexBackground background = new HexBackground();
		background.x = Futile.screen.halfWidth;
		background.y = Futile.screen.halfHeight;
		AddChild(background);
		
		HexCenterHexagon ch = new HexCenterHexagon("centerHexagon");
		ch.x = Futile.screen.halfWidth;
		ch.y = Futile.screen.halfHeight;
		ch.SpriteComponents()[0].sprite.color = new Color(1.0f, 0.8f, 0.2f, 1.0f);
		AddChild(ch);
		
		triangle = new HexTriangle("triangle");
		triangle.SpriteComponents()[0].sprite.color = new Color(1.0f, 0.2f, 0.8f, 1.0f);
		AddChild(triangle);
		
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
		
		triangle.x = radius * Mathf.Cos(angle * Mathf.Deg2Rad) + Futile.screen.halfWidth;
		triangle.y = radius * Mathf.Sin(angle * Mathf.Deg2Rad) + Futile.screen.halfHeight;
	}
}
