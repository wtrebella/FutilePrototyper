using UnityEngine;
using System.Collections;

public class SquareMaker : MonoBehaviour {

	public static FSprite Square(float width, float height) {
		return Square(width, height, Color.white);
	}
	
	public static FSprite Square(float width, float height, Color color) {
		FSprite sprite = new FSprite("whiteSquare.png");
		sprite.width = width;
		sprite.height = height;
		sprite.color = color;
		return sprite;
	}
}
