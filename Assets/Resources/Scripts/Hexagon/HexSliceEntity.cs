using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexSliceEntity : WTEntity {
	static int crossBarNum = 0;
	WTSpriteComponent backgroundSliceComponent;
	
	List<WTSpriteComponent> obstacleComponents = new List<WTSpriteComponent>();
	
	public HexSliceEntity(string name, Color backgroundColor) : base(name) {
		HexBackgroundSlice hbs = new HexBackgroundSlice(60f, 700f);
		hbs.rotation = -90f;
		hbs.color = backgroundColor;
		backgroundSliceComponent = new WTSpriteComponent("backgroundSliceComponent", hbs);
		AddComponent(backgroundSliceComponent);
	}

	public void MoveDownObstacles(float velocity) {
		List<WTSpriteComponent> obstaclesToRemove = new List<WTSpriteComponent>();
		
		foreach (WTSpriteComponent obstacleComponent in obstacleComponents) {
			HexCrossBar hcb = (HexCrossBar)obstacleComponent.sprite;
			hcb.y -= velocity * Time.fixedDeltaTime;
			hcb.distanceFromBackgroundSliceOrigin = hcb.y;
			if (hcb.y < 0) {
				hcb.y = 0;
				hcb.crossBarHeight -= velocity * Time.fixedDeltaTime;
				if (hcb.crossBarHeight == 0) {
					obstaclesToRemove.Add(obstacleComponent);
				}
			}
		}
		
		foreach (WTSpriteComponent obstacle in obstaclesToRemove) {
			obstacleComponents.Remove(obstacle);
			RemoveComponent(obstacle);		
		}
	}
	
	public void AddNewCrossbar(float height) {
		HexCrossBar hcb = new HexCrossBar(height);
		hcb.y = 700f;
		hcb.distanceFromBackgroundSliceOrigin = 700f;
		WTSpriteComponent crossBarComponent = new WTSpriteComponent(string.Format("crossBarComponent{0}", crossBarNum), hcb);
		crossBarNum++;
		obstacleComponents.Add(crossBarComponent);
		AddComponent(crossBarComponent);
	}
}
