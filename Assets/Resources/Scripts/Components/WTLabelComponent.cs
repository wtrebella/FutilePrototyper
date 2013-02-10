using UnityEngine;
using System.Collections;

public class WTLabelComponent : WTAbstractComponent {		
	private FLabel label_;
	
	public WTLabelComponent(string name, string fontName, string labelString, Color labelColor, float labelScale) : base(name) {
		componentType_ = ComponentType.Label;
		
		label_ = new FLabel(fontName, labelString);
		label_.scale = labelScale;
		label_.color = labelColor;
	}

	public FLabel label {
		get {return label_;}	
	}
}
