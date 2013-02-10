using UnityEngine;
using System.Collections;

public class WTTableCell : WTEntity {
	float horizontalPadding_ = 0;
	float verticalPadding_ = 0;
	float width_;
	float height_ = 0;
		
	public TableCellType tableCellType;
	public WTLabelComponent leftLabelComponent;
	public WTSpriteComponent rightSpriteComponent;
	public WTLabelComponent centerLabelComponent;
	public WTSpriteComponent bottomLineSpriteComponent;
	
	private WTSpriteComponent backgroundSpriteComponent;
	
	public WTTableCell(string name, float horizontalPadding, float verticalPadding, float width, Color backgroundColor, TableCellType type) : base(name) {
		this.tableCellType = type;
		horizontalPadding_ = horizontalPadding;
		verticalPadding_ = verticalPadding;
		width_ = width;
		
		if (backgroundColor.a > 0) {
			backgroundSpriteComponent = new WTSpriteComponent("backgroundSpriteComponent", "Futile_White");
			backgroundSpriteComponent.sprite.color = backgroundColor;
			backgroundSpriteComponent.sprite.width = width_;
			backgroundSpriteComponent.sprite.anchorX = 0;
			backgroundSpriteComponent.sprite.anchorY = 0;
			backgroundSpriteComponent.sprite.y = 1;
			AddComponent(backgroundSpriteComponent);
		}
		
		bottomLineSpriteComponent = new WTSpriteComponent("bottomLineSpriteComponent", "Futile_White");
		bottomLineSpriteComponent.sprite.width = width;
		bottomLineSpriteComponent.sprite.height = 1f;
		bottomLineSpriteComponent.sprite.color = Color.black;
		bottomLineSpriteComponent.sprite.alpha = 0.2f;
		bottomLineSpriteComponent.sprite.anchorX = 0;
		//bottomLineSpriteComponent.sprite.y = 1f;
		AddComponent(bottomLineSpriteComponent);
	}
	
	public void AddLeftLabel(string fontName, string labelString, Color labelColor, float labelScale) {
		labelString = AddLineBreaksToString(labelString, 25);
					
		WTLabelComponent lc = new WTLabelComponent("leftLabelComponent", fontName, labelString, labelColor, labelScale);
		lc.label.anchorX = 0;
		lc.label.x = horizontalPadding_;
		leftLabelComponent = lc;
		AddComponent(lc);
		
		this.height = Mathf.Max(height_, lc.label.textRect.height * lc.label.scaleY + verticalPadding_ * 2);
		
		lc.label.y = height_ / 2f;
	}
	
	public void AddCenterLabel(string fontName, string labelString, Color labelColor, float labelScale) {					
		WTLabelComponent lc = new WTLabelComponent("centerLabelComponent", fontName, labelString, labelColor, labelScale);
		lc.label.x = width_ / 2f;
		centerLabelComponent = lc;
		AddComponent(lc);
		
		this.height = Mathf.Max(height_, lc.label.textRect.height * lc.label.scaleY + verticalPadding_ * 2);
		
		lc.label.y = height_ / 2f;
	}
	
	public bool LocalRectContainsTouch(FTouch touch) {
		Vector2 localPos = backgroundSpriteComponent.sprite.GlobalToLocal(touch.position);
		if (backgroundSpriteComponent.sprite.localRect.Contains(localPos)) return true;
		else return false;
	}
	
	public string AddLineBreaksToString(string stringToBreak, int charsPerRow) {
		int currentIndexOfNewLine = 0;
		char[] labelStringArray = stringToBreak.ToCharArray();
		
		while (labelStringArray.Length > currentIndexOfNewLine + charsPerRow) {
			int indexOfLastSpaceBeforeSplit = -1;
			
			for (int i = currentIndexOfNewLine + charsPerRow; i >= 0; i--) {				
				if (labelStringArray[i] == ' ') {
					indexOfLastSpaceBeforeSplit = i;
					break;
				}
			}
			
			if (indexOfLastSpaceBeforeSplit == -1) {
				Debug.Log("No space found!");
				return stringToBreak;
			}
			
			labelStringArray[indexOfLastSpaceBeforeSplit] = '\n';
			currentIndexOfNewLine = indexOfLastSpaceBeforeSplit + 1;
		}
		
		return new string(labelStringArray);
	}
	
	public void AddRightSprite(string imageName, float spriteScale) {
		WTSpriteComponent sc = new WTSpriteComponent("rightSpriteComponent", imageName);
		sc.sprite.scale = spriteScale;
		sc.sprite.x = width_ - sc.sprite.width / 2f - horizontalPadding_;
		rightSpriteComponent = sc;
		AddComponent(sc);
		
		this.height = Mathf.Max(height_, sc.sprite.localRect.height + verticalPadding_ * 2);
		
		sc.sprite.y = height_ / 2f;
	}
	
	public float width {
		get {return width_;}
	}
	
	public float height {
		get {return height_;}
		set {
			height_ = value;
			backgroundSpriteComponent.sprite.height = height_ - 2f;
		}
	}
	
	public float horizontalPadding {
		get {return horizontalPadding_;}
	}
	
	public float verticalPadding {
		get {return verticalPadding_;}
	}
}