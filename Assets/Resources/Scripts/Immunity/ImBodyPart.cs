using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BodyPartType {
	None,
	Node,
	Bone,
	Vein,
	Artery,
	Muscle,
	Organ,
	Limb
}

public class ImBodyPart {
	protected bool isSelected_ = false;
	protected FContainer spriteContainer_;
	protected FSprite mainSprite_;
	protected float defaultSpriteContainerRotation_ = 0.0f;
	protected float defaultSpriteContainerScale_ = 0.6f;
	protected Color defaultSpriteColor_ = Color.white;
	
	private BodyPartType bodyPartType_;
	private ImOrganComponent organComponent_;
	private ImNodeComponent nodeComponent_;
	private TweenChain spriteContainerPulsateTween_;
	
	private ImBodyPart(OrganType organType, NodeType nodeType, float containerRotation, float containerScale, Color spriteColor) {
		defaultSpriteContainerRotation_ = containerRotation;
		defaultSpriteContainerScale_ = containerScale;
		defaultSpriteColor_ = spriteColor;
		
		spriteContainer_ = new FContainer();
		spriteContainer_.rotation = defaultSpriteContainerRotation_;
		spriteContainer_.scale = defaultSpriteContainerScale_;
		
		bodyPartType_ = BodyPartType.None;
		
		if (organType != OrganType.None) {
			bodyPartType_ = BodyPartType.Organ;
			organComponent_ = new ImOrganComponent(organType, this);
		}
		
		if (nodeType != NodeType.None) {
			bodyPartType_ = BodyPartType.Node;
			nodeComponent_ = new ImNodeComponent(nodeType, this);
		}
		
		InitSprites();
	}
	
	public ImBodyPart(OrganType organType, float containerRotation, float containerScale, Color spriteColor) : this(organType, NodeType.None, containerRotation, containerScale, spriteColor) {
		
	}
	
	public ImBodyPart(NodeType nodeType, float containerRotation, float containerScale, Color spriteColor) : this(OrganType.None, nodeType, containerRotation, containerScale, spriteColor) {
		
	}
	
	public bool SpriteContainsGlobalPoint(Vector2 globalPoint) {
		return mainSprite_.localRect.Contains(mainSprite_.GlobalToLocal(globalPoint));
	}
	
	public ImAbstractBodyPartComponent BodyPartComponent() {
		ImAbstractBodyPartComponent component = null;
		
		if (bodyPartType_ == BodyPartType.Organ) component = organComponent_;
		else if (bodyPartType_ == BodyPartType.Node) component = nodeComponent_;
		
		return component;
	}
	
	public void InitSprites() {
		mainSprite_ = new FSprite(BodyPartComponent().spriteImageName);
		mainSprite_.color = defaultSpriteColor_;
		spriteContainer_.AddChild(mainSprite_);
		
		spriteContainerPulsateTween_ = new TweenChain();
		spriteContainerPulsateTween_.setIterations(-1);
		spriteContainerPulsateTween_.append(new Tween(spriteContainer_, 0.2f, new TweenConfig().floatProp("scale", defaultSpriteContainerScale_ + 0.03f)));
		spriteContainerPulsateTween_.append(new Tween(spriteContainer_, 0.2f, new TweenConfig().floatProp("scale", defaultSpriteContainerScale_)));
		Go.addTween(spriteContainerPulsateTween_);
	}
	
	public void PlaceSpriteContainerAt(float x, float y) {
		spriteContainer_.x = x;
		spriteContainer_.y = y;
	}
	
	#region Getters/Setters
	
	public FSprite mainSprite {
		get {return mainSprite_;}	
	}
	
	public FContainer spriteContainer {
		get {return spriteContainer_;}	
	}
	
	public BodyPartType bodyPartType {
		get {return bodyPartType_;}	
	}
	
	public ImOrganComponent organComponent {
		get {return organComponent_;}	
	}
	
	public ImNodeComponent nodeComponent {
		get {return nodeComponent_;}
	}
		
	public bool isSelected {
		get {
			return isSelected_;
		}
		set {
			isSelected_ = value;
			if (isSelected_) {
				spriteContainerPulsateTween_.play();
			}
			else {
				spriteContainerPulsateTween_.pause();
				spriteContainer_.scale = defaultSpriteContainerScale_;
			}
		}
	}
	
	#endregion
}
