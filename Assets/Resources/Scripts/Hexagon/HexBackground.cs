using UnityEngine;
using System.Collections;

public class HexBackground : FContainer {
	HexBackgroundSlice[] backgroundSlices;
	Color color1 = new Color(0.3f, 0.3f, 0.3f, 1.0f);
	Color color2 = new Color(0.4f, 0.4f, 0.4f, 1.0f);
	
	public HexBackground() {
		backgroundSlices = new HexBackgroundSlice[6];
		
		bool isColor1 = true;
		
		for (int i = 0; i < 6; i++) {
			backgroundSlices[i] = new HexBackgroundSlice(60, 600);
			backgroundSlices[i].rotation = 60 * i;
			if (isColor1) backgroundSlices[i].color = color1;
			else backgroundSlices[i].color = color2;
			
			isColor1 = !isColor1;
			
			AddChild(backgroundSlices[i]);
		}
	}
}
