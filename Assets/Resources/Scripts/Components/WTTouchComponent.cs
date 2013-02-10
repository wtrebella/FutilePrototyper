using UnityEngine;
using System.Collections;

public class WTTouchComponent : WTAbstractComponent {	
	public FTouch initialTouch;
	public Vector2 offset = Vector2.zero;
	
	public WTTouchComponent(string name) : base(name) {
		componentType_ = ComponentType.Touch;
	}
}
