using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImOrganLayer : FContainer {
	public List<ImBodyPart> organs;
	
	public ImOrganLayer() {
		organs = new List<ImBodyPart>();
		
		ImBodyPart heart = new ImBodyPart(OrganType.Heart, -20f, 0.6f, Color.red);
		ImBodyPart brain = new ImBodyPart(OrganType.Brain, 0f, 0.6f, Color.magenta);
		ImBodyPart liver = new ImBodyPart(OrganType.Liver, -20f, 0.6f, Color.cyan);
		ImBodyPart lungLeft = new ImBodyPart(OrganType.LungLeft, 20f, 0.6f, Color.blue);
		ImBodyPart lungRight = new ImBodyPart(OrganType.LungRight, -20f, 0.6f, Color.blue);
		ImBodyPart stomach = new ImBodyPart(OrganType.Stomach, -30f, 0.6f, Color.yellow);
		ImBodyPart intestines = new ImBodyPart(OrganType.Intestines, 0f, 0.6f, Color.green);
		
		heart.PlaceSpriteContainerAt(2f, 86f);
		brain.PlaceSpriteContainerAt(-6f, 181f);
		liver.PlaceSpriteContainerAt(-8f, 60f);
		lungLeft.PlaceSpriteContainerAt(-21f, 91f);
		lungRight.PlaceSpriteContainerAt(20f, 94f);
		stomach.PlaceSpriteContainerAt(9f, 44f);
		intestines.PlaceSpriteContainerAt(-4f, 7f);

		organs.Add(heart);
		organs.Add(brain);
		organs.Add(liver);
		organs.Add(lungLeft);
		organs.Add(lungRight);
		organs.Add(stomach);
		organs.Add(intestines);
		
		foreach (ImBodyPart organ in organs) {
			AddChild(organ.spriteContainer);
		}
	}
}
