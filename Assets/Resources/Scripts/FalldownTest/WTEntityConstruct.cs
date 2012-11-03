using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTEntityConstruct : FContainer {
	public List<WTEntity> entities;
	public float xVelocity;
	public float yVelocity;
	
	public WTEntityConstruct(List<WTEntity> entities) {
		if (entities == null) this.entities = new List<WTEntity>();
		else this.entities = entities;
		
		foreach (WTEntity entity in entities) {
			AddChild(entity);	
		}
	}
	
	public void MoveTo(float newX, float newY) {
		foreach (WTEntity entity in entities) {
			Vector2 globalPos = LocalToGlobal(new Vector2(entity.x, entity.y));
			entity.xPreviousGlobal = globalPos.x;
			entity.yPreviousGlobal = globalPos.y;
		}
		
		this.x = newX;
		this.y = newY;
	}
	
	/*public void SetVelocity(float xVelocity, float yVelocity) {
		foreach (WTEntity entity in entities) {
			entity.xVelocity = xVelocity;
			entity.yVelocity = yVelocity;
		}
	}*/
}

public enum ConstructType {
	None,
	Pyramid,
	Bracket,
	DottedLine
}

public enum RotationType {
	Rotation0,
	Rotation90,
	Rotation180,
	Rotation270
}

public class WTEntityConstructData {
	
	public WTEntityConstructData() {
		
	}
	
	public static int[][] ConstructDataArray(ConstructType type, RotationType rotation = RotationType.Rotation0) {
		int[][] d = null;
		
		if (type == ConstructType.Pyramid) {
			if (rotation == RotationType.Rotation0) {
				d = new int [3][];

				d[0] = new int[] {1, 1, 1, 1, 1};
				d[1] = new int[] {0, 1, 1, 1, 0};
				d[2] = new int[] {0, 0, 1, 0, 0};
			}
			if (rotation == RotationType.Rotation90) {
				d = new int [5][];
		
				d[0] = new int[] {0, 0, 1};
				d[1] = new int[] {0, 1, 1};
				d[2] = new int[] {1, 1, 1};
				d[3] = new int[] {0, 1, 1};
				d[4] = new int[] {0, 0, 1};
			}
			if (rotation == RotationType.Rotation180) {
				d = new int [3][];
				
				d[0] = new int[] {0, 0, 1, 0, 0};
				d[1] = new int[] {0, 1, 1, 1, 0};
				d[2] = new int[] {1, 1, 1, 1, 1};				
			}
			if (rotation == RotationType.Rotation270) {
				d = new int [5][];
						
				d[0] = new int[] {1, 0, 0};
				d[1] = new int[] {1, 1, 0};
				d[2] = new int[] {1, 1, 1};
				d[3] = new int[] {1, 1, 0};
				d[4] = new int[] {1, 0, 0};
			}
		}
		
		if (type == ConstructType.Bracket) {
			if (rotation == RotationType.Rotation0) {
				d = new int [2][];
		
				d[0] = new int[] {1, 1, 1, 1, 1};
				d[1] = new int[] {1, 0, 0, 0, 1};
			}
			if (rotation == RotationType.Rotation90) {
				d = new int [5][];
		
				d[0] = new int[] {1, 1};
				d[1] = new int[] {0, 1};
				d[2] = new int[] {0, 1};
				d[3] = new int[] {0, 1};
				d[4] = new int[] {1, 1};
			}
			if (rotation == RotationType.Rotation180) {
				d = new int [2][];
		
				d[0] = new int[] {1, 0, 0, 0, 1};
				d[1] = new int[] {1, 1, 1, 1, 1};
			}
			if (rotation == RotationType.Rotation270) {
				d = new int [5][];
		
				d[0] = new int[] {1, 1};
				d[1] = new int[] {1, 0};
				d[2] = new int[] {1, 0};
				d[3] = new int[] {1, 0};
				d[4] = new int[] {1, 1};
			}
		}
		
		if (type == ConstructType.DottedLine) {
			if (rotation != RotationType.Rotation0) Debug.Log("Can't rotate dotted line");
			
			d = new int [1][];
	
			d[0] = new int[] {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1};
		}
				
		return d;
	}
}
