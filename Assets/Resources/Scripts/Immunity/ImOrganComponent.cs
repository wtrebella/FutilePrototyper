using UnityEngine;
using System.Collections;

public enum OrganType {
	None,
	Heart,
	Brain,
	Intestines,
	LungLeft,
	LungRight,
	Liver,
	Stomach
}

public class ImOrganComponent : ImAbstractBodyPartComponent {	
	private OrganType organType_;
	
	public ImOrganComponent(OrganType organType, ImBodyPart owner) : base(owner) {
		organType_ = organType;
		
		switch (organType) {
		case OrganType.None:
			break;
		case OrganType.Brain:
			spriteImageName = "brain.png";
			break;
		case OrganType.Heart:
			spriteImageName = "heart.png";
			break;
		case OrganType.Intestines:
			spriteImageName = "intestines.png";
			break;
		case OrganType.Liver:
			spriteImageName = "liver.png";
			break;
		case OrganType.LungLeft:
			spriteImageName = "lungLeft.png";
			break;
		case OrganType.LungRight:
			spriteImageName = "lungRight.png";
			break;
		case OrganType.Stomach:
			spriteImageName = "stomach.png";
			break;
		default:
			break;
		}
	}
	
	public OrganType organType {
		get {return organType_;}	
	}
	
	override public string Description() {
		string description = "";
		
		switch (organType_) {
		case OrganType.Brain:
			description = "Brain";
			break;
		case OrganType.Heart:
			description = "Heart";
			break;
		case OrganType.Intestines:
			description = "Intestines";
			break;
		case OrganType.Liver:
			description = "Liver";
			break;
		case OrganType.LungLeft:
			description = "LungLeft";
			break;
		case OrganType.LungRight:
			description = "LungRight";
			break;
		case OrganType.None:
			description = "None";
			break;
		case OrganType.Stomach:
			description = "Stomach";
			break;
		}
		
		return description;
	}
}
