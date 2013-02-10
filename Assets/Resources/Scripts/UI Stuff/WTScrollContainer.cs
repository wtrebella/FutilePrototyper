using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface WTScrollContainerInterface {
	void ScrollContainerDidScroll(WTScrollContainer scrollContainer);
	void ScrollContainerWillBeginDragging(WTScrollContainer scrollContainer);
	void ScrollContainerWillEndDragging(WTScrollContainer scrollContainer, bool velocity, Vector2 targetContentOffset);
	void ScrollContainerDidEndDragging(WTScrollContainer scrollContainer, bool willDecelerate);
	void ScrollContainerShouldScrollToTop(WTScrollContainer scrollContainer);
	void ScrollContainerDidScrollToTop(WTScrollContainer scrollContainer);
	void ScrollContainerWillBeginDecelerating(WTScrollContainer scrollContainer);
	void ScrollContainerDidEndDecelerating(WTScrollContainer scrollContainer);
	void ScrollContainerDidEndScrollingAnimation(WTScrollContainer scrollContainer);
}

public class WTScrollContainer : FContainer, FSingleTouchableInterface {
	public WTScrollContainerInterface scrollContainerDelegate;
	
	public ScrollType scrollType;
	
	public Vector2 contentSize = Vector2.zero;
	public WTEdgeInsets contentInset = new WTEdgeInsets(0, 0, 0, 0);
	
	public bool scrollEnabled = true;
	public bool directionLockEnabled = true;
	public bool scrollsToTop = true;
	public bool bounces = true;
	public bool alwaysBounceVertical = true;
	public bool alwaysBounceHorizontal = true;
	public bool delaysContentTouches = true;
	public float decelerationRate = 100f;
	
	private bool dragging_ = false;
	private bool tracking_ = false;
	private bool decelerating_ = false;
	//private Vector2 originalTouchPoint_;
	private Vector2 lastTouchPoint_;
	private Vector2 contentOffset_ = Vector2.zero;
		
	public WTScrollContainer(ScrollType scrollType = ScrollType.Vertical) {
		this.scrollType = scrollType;
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.touchManager.AddSingleTouchTarget(this);
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.touchManager.RemoveSingleTouchTarget(this);
	}
	
	public void SetContentOffset(Vector2 contentOffset, bool animated) {
		// for now, don't do animation
		contentOffset_ = contentOffset;
		if (scrollType == ScrollType.Both || scrollType == ScrollType.Horizontal) this.x = contentOffset_.x;
		if (scrollType == ScrollType.Both || scrollType == ScrollType.Vertical) this.y = contentOffset_.y;
	}
	
	public void ScrollRectToVisible(Rect rect, bool animated) {
		
	}
	
	public bool dragging {
		get {return dragging_;}	
	}
	
	public bool tracking {
		get {return tracking_;}	
	}
	
	public bool decelerating {
		get {return decelerating_;}	
	}
	
	public bool HandleSingleTouchBegan(FTouch touch) {
		// for now, just do dragging, not tracking
		
		//originalTouchPoint_ = touch.position;
		lastTouchPoint_ = touch.position;
		
		return true;
	}
	
	public void HandleSingleTouchMoved(FTouch touch) {
		if (!lastTouchPoint_.Equals(touch.position)) {
			Vector2 deltaPoint = new Vector2(touch.position.x - lastTouchPoint_.x, touch.position.y - lastTouchPoint_.y);
			lastTouchPoint_ = touch.position;
			SetContentOffset(new Vector2(contentOffset_.x + deltaPoint.x, contentOffset_.y + deltaPoint.y), false);
			
			// for now, just clip stuff that's fully out of bounds; later need to figure out how to crop
			
			if (contentSize.Equals(Vector2.zero)) Debug.Log("content size is zero!");
		}
	}
	
	public void HandleSingleTouchEnded(FTouch touch) {
		
	}
	
	public void HandleSingleTouchCanceled(FTouch touch) {
		
	}
	
	/*
Managing the Scroll Indicator
  indicatorStyle  property
  scrollIndicatorInsets  property
  showsHorizontalScrollIndicator  property
  showsVerticalScrollIndicator  property
– flashScrollIndicators
  
Managing the Delegate
  delegate  property
  */
}
