using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImNodeLayer : FContainer, FSingleTouchableInterface {
	public List<ImBodyPart> nodes;
	
	FButton tempButton;
	ImBodyPart currentNode;
	
	public ImNodeLayer() {
		tempButton = new FButton("circle.png", "circle.png", null);
		tempButton.x = 50f - Futile.screen.halfWidth;
		tempButton.y = Futile.screen.height - 50f - Futile.screen.halfHeight;
		tempButton.sprite.color = Color.blue;
		tempButton.SignalPress += PressedButton;
		AddChild(tempButton);
		
		nodes = new List<ImBodyPart>();
		
		for (int i = 0; i < 21; i++) {
			ImBodyPart node = new ImBodyPart(NodeType.NormalNode, 0, 0.4f, Color.red);
			nodes.Add(node);
			AddChild(node.spriteContainer);
		}
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.touchManager.AddSingleTouchTarget(this);
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.touchManager.RemoveSingleTouchTarget(this);
	}
	
	public void PressedButton(FButton button) {
		
	}
	
	public bool HandleSingleTouchBegan(FTouch touch) {
		foreach (ImBodyPart node in nodes) {
			if (node.SpriteContainsGlobalPoint(touch.position)) {
				currentNode = node;
				return true;
			}
		}
		
		return false;
	}
	
	public void HandleSingleTouchMoved(FTouch touch) {
		if (currentNode == null) return;
		
		currentNode.spriteContainer.x += touch.deltaPosition.x;
		currentNode.spriteContainer.y += touch.deltaPosition.y;
	}
	
	public void HandleSingleTouchEnded(FTouch touch) {
		if (currentNode == null) return;
		
		currentNode = null;
	}
	
	public void HandleSingleTouchCanceled(FTouch touch) {
		
	}
}
