using UnityEngine;
using System.Collections;

public enum NodeType {
	None,
	NormalNode
}

public class ImNodeComponent : ImAbstractBodyPartComponent {
	private NodeType nodeType_;
	
	public ImNodeComponent(NodeType nodeType, ImBodyPart owner) : base(owner) {
		nodeType_ = nodeType;
		
		switch (nodeType) {
		case NodeType.None:
			break;
		case NodeType.NormalNode:
			spriteImageName = "circle.png";
			break;
		default:
			break;
		}
	}
	
	override public string Description() {
		string description = "";
		
		switch (nodeType_) {
		case NodeType.NormalNode:
			description = "Normal node";
			break;
		}
		
		return description;
	}
	
	public NodeType nodeType {
		get {return nodeType_;}	
	}
}
