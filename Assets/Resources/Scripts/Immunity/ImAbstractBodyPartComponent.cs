using UnityEngine;
using System.Collections;

public class ImAbstractBodyPartComponent {
	public string spriteImageName;
	public ImBodyPart owner;
	
	public ImAbstractBodyPartComponent(ImBodyPart owner) {
		this.owner = owner;
	}
	
	virtual public string Description() {
		return "I'm an abstract body part!";
	}
}
