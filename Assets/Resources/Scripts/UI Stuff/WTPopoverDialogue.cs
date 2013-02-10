using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// i think i should make it so the anchor is the bottom left

public class WTPopoverDialogue : WTEntity {
	public WTScrollBar scrollBar;
	public List<WTTableCell> tableCells;
	public bool isShowing = false;
	public WTEntity correspondingEntity;

	private float width_;
	private float height_;
	private float inset_ = 7.5f;
	private WTSpriteComponent triangleSpriteComponent;
	public WTPopoverDialogue(bool withScrollBar, string name = "popover dialogue") : base(name) {
		tableCells = new List<WTTableCell>();
		
		AddComponent(new WTSliceSpriteComponent("sliceSpriteComponent", "uiPopover.psd", 10, 10, inset_, inset_, inset_, inset_));
		
		//triangleSpriteComponent = new ImSpriteComponent("triangleSpriteComponent", "popoverTriangle.psd");
		//AddComponent(triangleSpriteComponent);
		
		if (withScrollBar) {
			AddComponent(new WTScrollBarComponent("scrollBarComponent"));
		}
	}
	
	public void AddTableCell(string leftLabelString, string rightSpriteImageName, TableCellType type) {	
		WTTableCell tableCell = new WTTableCell("tableCell", 8f, 3f, width_ - inset_, Color.white, type);
		tableCells.Add(tableCell);
		tableCell.x = inset_ / 2f - width_ / 2f;
		tableCell.AddLeftLabel("TwCen", leftLabelString, Color.black, 0.2f);
		tableCell.AddRightSprite(rightSpriteImageName, 1f);
		tableCell.rightSpriteComponent.sprite.color = Color.blue;
		AddChild(tableCell);
		
		RefreshHeight();		
		ArrangeCells();
	}
	
	public void AddTableCell(string centerLabelString, TableCellType type) {	
		WTTableCell tableCell = new WTTableCell("tableCell", 8f, 3f, width_ - inset_, Color.white, type);
		tableCells.Add(tableCell);
		tableCell.x = inset_ / 2f - width_ / 2f;
		tableCell.AddCenterLabel("TwCen", centerLabelString, Color.black, 0.2f);
		AddChild(tableCell);
		
		RefreshHeight();		
		ArrangeCells();
	}

	public void Show(WTEntity correspondingEntity) {
		isShowing = true;
		this.isVisible = true;
		this.correspondingEntity = correspondingEntity;
	}
	
	public void Dismiss() {
		isShowing = false;
		this.isVisible = false;
		for (int i = tableCells.Count - 1; i >= 0; i--) {
			WTTableCell cell = tableCells[i];
			RemoveTableCell(cell);
		}
		correspondingEntity = null;
	}
	
	public void RemoveTableCell(WTTableCell tableCell) {
		tableCells.Remove(tableCell);
		RemoveChild(tableCell);
		RefreshHeight();
		ArrangeCells();
	}
	
	private void RefreshHeight() {
		float totalHeight = inset_;
		
		foreach (WTTableCell cell in tableCells) totalHeight += cell.height;
		
		this.height = totalHeight;
	}
	
	private void ArrangeCells() {		
		WTTableCell doneCell = null;
		
		foreach (WTTableCell cell in tableCells) {
			if (cell.tableCellType == TableCellType.Done) doneCell = cell;	
		}
		
		if (doneCell != null) {
			tableCells.Remove(doneCell);
			tableCells.Add(doneCell);
		}
		
		float yPositionSoFar = height_ / 2f - inset_ / 2f;
		
		foreach (WTTableCell tableCell in tableCells) {
			yPositionSoFar -= tableCell.height;
			tableCell.y = yPositionSoFar;
		}
	}
	
	public float width {
		get {return width_;}
		set {
			FSliceSprite sprite = SliceSpriteComponents()[0].sprite;
			if (width_ != value) {
				width_ = value;
				sprite.width = width_;
				if (ScrollBarComponent() != null) ScrollBarComponent().scrollBar.x = width_ / 2f - ScrollBarComponent().scrollBar.width - inset_ / 2f; // why're the insets divided by 2???
			}
		}
	}
	
	public float height {
		get {return height_;}
		set {
			FSliceSprite sprite = SliceSpriteComponents()[0].sprite;
			if (height_ != value) {
				height_ = value;
				sprite.height = height_;
				if (ScrollBarComponent() != null) {
					ScrollBarComponent().scrollBar.height = height_ - inset_;
					ScrollBarComponent().scrollBar.y = -height_ / 2f + inset_ / 2f;
				}
			}
		}
	}
	
	public bool HandleTouchBegan(FTouch touch) {
		foreach (WTTableCell cell in tableCells) {
			if (cell.LocalRectContainsTouch(touch)) {
				return true;
			}
		}
		
		return false;
	}
	
	public bool ContainsTouch(FTouch touch) {
		FSliceSprite sprite = SliceSpriteComponents()[0].sprite;
		Vector2 localPos = GlobalToLocal(touch.position);
		if (sprite.localRect.Contains(localPos)) return true;
		else return false;
	}
	
	public float inset {
		get {return inset_;}	
	}
	
	/*public bool HandleTouchMoved(FTouch touch) {
		FSliceSprite sprite = SliceSpriteComponents()[0].sprite;
		Vector2 localPos = GlobalToLocal(touch.position);
		if (sprite.localRect.Contains(localPos)) {
			Debug.Log("touched popover");
			return true;
		}
		return false;
	}
	
	public bool HandleTouchEnded(FTouch touch) {
		FSliceSprite sprite = SliceSpriteComponents()[0].sprite;
		Vector2 localPos = GlobalToLocal(touch.position);
		if (sprite.localRect.Contains(localPos)) {
			Debug.Log("touched popover");
			return true;
		}
		return false;
	}
	
	public bool HandleTouchCanceled(FTouch touch) {
		FSliceSprite sprite = SliceSpriteComponents()[0].sprite;
		Vector2 localPos = GlobalToLocal(touch.position);
		if (sprite.localRect.Contains(localPos)) {
			Debug.Log("touched popover");
			return true;
		}
		return false;
	}*/
}