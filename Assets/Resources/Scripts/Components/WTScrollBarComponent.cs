using UnityEngine;
using System.Collections;

public class WTScrollBarComponent : WTAbstractComponent {
	public WTScrollBar scrollBar;
	
	public WTScrollBarComponent(string name) : base(name) {
		componentType_ = ComponentType.ScrollBar;
		
		scrollBar = new WTScrollBar();
	}
}
