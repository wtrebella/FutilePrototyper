using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexSliceEntity : WTEntity {
	WTSpriteComponent backgroundSliceComponent;
	
	List<WTSpriteComponent> obstacleComponents = new List<WTSpriteComponent>();
	
	public HexSliceEntity(string name, Color backgroundColor) : base(name) {
		HexBackgroundSlice hbs = new HexBackgroundSlice(60f, 700f);
		hbs.rotation = -90f;
		hbs.color = backgroundColor;
		backgroundSliceComponent = new WTSpriteComponent("backgroundSliceComponent", hbs);
		AddComponent(backgroundSliceComponent);
		
		HexCrossBar hcb = new HexCrossBar(50f);
		hcb.y = 700f;
		hcb.distanceFromBackgroundSliceOrigin = 700f;
		WTSpriteComponent crossBarComponent = new WTSpriteComponent("crossBarComponent", hcb);
		obstacleComponents.Add(crossBarComponent);
		AddComponent(crossBarComponent);
	}		

	public void MoveDownObstacles(float velocity) {
		foreach (WTSpriteComponent obstacleComponent in obstacleComponents) {
			HexCrossBar hcb = (HexCrossBar)obstacleComponent.sprite;
			hcb.y -= velocity * Time.fixedDeltaTime;
			hcb.distanceFromBackgroundSliceOrigin = hcb.y;
			hcb.crossBarHeight = Mathf.Min(hcb.crossBarHeight, hcb.distanceFromBackgroundSliceOrigin);
		}
	}
}
