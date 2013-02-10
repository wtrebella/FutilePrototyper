using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WTEntity : FContainer {	
	public string name;
	
	private event Action<WTAbstractComponent> SignalComponentAdded;
	private event Action<WTAbstractComponent> SignalComponentRemoved;
	private Dictionary<string, WTAbstractComponent> components_;
	
	protected bool isSelected_ = false;
	
	public WTEntity(string name = "an entity") {
		this.name = name;
		components_ = new Dictionary<string, WTAbstractComponent>();
		
		this.SignalComponentAdded += HandleComponentAdded;
		this.SignalComponentRemoved += HandleComponentRemoved;
	}
	
	virtual public void HandleUpdate() {
		foreach (WTAbstractComponent component in components_.Values) component.HandleUpdate();
	}
	
			
	public void AddComponent(WTAbstractComponent component) {
		components_.Add(component.name, component);
		if (SignalComponentAdded != null) SignalComponentAdded(component);
	}
	
	public void RemoveComponent(WTAbstractComponent component) {
		components_.Remove(component.name);
		if (SignalComponentRemoved != null) SignalComponentRemoved(component);
	}
	
	public void HandleComponentAdded(WTAbstractComponent component) {
		component.owner = this;
		
		if (component.componentType == ComponentType.Sprite) AddChild((component as WTSpriteComponent).sprite);
		if (component.componentType == ComponentType.RadialWipeSprite) AddChild((component as WTRadialWipeSpriteComponent).sprite);
		if (component.componentType == ComponentType.SliceSprite) AddChild((component as WTSliceSpriteComponent).sprite);
		if (component.componentType == ComponentType.ScrollContainer) AddChild((component as WTScrollContainerComponent).scrollContainer);
		if (component.componentType == ComponentType.ScrollBar) AddChild((component as WTScrollBarComponent).scrollBar);
		if (component.componentType == ComponentType.Label) AddChild((component as WTLabelComponent).label);
	}
	
	public void HandleComponentRemoved(WTAbstractComponent component) {
		component.owner = null;
		
		if (component.componentType == ComponentType.Sprite) RemoveChild((component as WTSpriteComponent).sprite);
		if (component.componentType == ComponentType.RadialWipeSprite) RemoveChild((component as WTRadialWipeSpriteComponent).sprite);
		if (component.componentType == ComponentType.SliceSprite) RemoveChild((component as WTSliceSpriteComponent).sprite);
		if (component.componentType == ComponentType.ScrollContainer) RemoveChild((component as WTScrollContainerComponent).scrollContainer);
		if (component.componentType == ComponentType.ScrollBar) RemoveChild((component as WTScrollBarComponent).scrollBar);
		if (component.componentType == ComponentType.Label) RemoveChild((component as WTLabelComponent).label);
	}
	
	public List<WTAbstractComponent> ComponentsForType(ComponentType type) {
		List<WTAbstractComponent> comps = new List<WTAbstractComponent>();
		foreach (string key in components_.Keys) {
			if (components_[key].componentType == type) comps.Add(components_[key]);
		}
		return comps;
	}
	
	public WTAbstractComponent ComponentForName(string name) {
		if (components_.ContainsKey(name)) return components_[name];
		else {
			Debug.Log("no component with that name");
			return null;
		}
	}
	
	public List<WTSpriteComponent> SpriteComponents() {
		if (ComponentsForType(ComponentType.Sprite).Count == 0) return null;
		
		List<WTSpriteComponent> scs = new List<WTSpriteComponent>();
		foreach (WTAbstractComponent comp in ComponentsForType(ComponentType.Sprite)) scs.Add((WTSpriteComponent)comp);
		return scs;
	}
	
	public List<WTTouchComponent> TouchComponents() {
		if (ComponentsForType(ComponentType.Touch).Count == 0) return null;
		
		List<WTTouchComponent> tcs = new List<WTTouchComponent>();
		foreach (WTAbstractComponent comp in ComponentsForType(ComponentType.Touch)) tcs.Add((WTTouchComponent)comp);
		return tcs;
	}
	
	public List<WTLabelComponent> LabelComponents() {
		if (ComponentsForType(ComponentType.Label).Count == 0) return null;
		
		List<WTLabelComponent> sls = new List<WTLabelComponent>();
		foreach (WTAbstractComponent comp in ComponentsForType(ComponentType.Label)) sls.Add((WTLabelComponent)comp);
		return sls;
	}
	
	public List<WTRadialWipeSpriteComponent> RadialWipeSpriteComponents() {
		if (ComponentsForType(ComponentType.RadialWipeSprite).Count == 0) return null;
		
		List<WTRadialWipeSpriteComponent> rwscs = new List<WTRadialWipeSpriteComponent>();
		foreach (WTAbstractComponent comp in ComponentsForType(ComponentType.RadialWipeSprite)) rwscs.Add((WTRadialWipeSpriteComponent)comp);
		return rwscs;
	}
	
	public List<WTSliceSpriteComponent> SliceSpriteComponents() {
		if (ComponentsForType(ComponentType.SliceSprite).Count == 0) return null;
		
		List<WTSliceSpriteComponent> sscs = new List<WTSliceSpriteComponent>();
		foreach (WTAbstractComponent comp in ComponentsForType(ComponentType.SliceSprite)) sscs.Add((WTSliceSpriteComponent)comp);
		return sscs;
	}
	
	public WTScrollContainerComponent ScrollContainerComponent() {
		if (ComponentsForType(ComponentType.ScrollContainer).Count == 0) return null;
		
		if (ComponentsForType(ComponentType.ScrollContainer).Count > 1) Debug.Log("there's more than one scroll component attached to this object; should there be?");
		return (WTScrollContainerComponent)ComponentsForType(ComponentType.ScrollContainer)[0];	
	}
	
	public WTScrollBarComponent ScrollBarComponent() {
		if (ComponentsForType(ComponentType.ScrollBar).Count == 0) return null;
		
		if (ComponentsForType(ComponentType.ScrollBar).Count > 1) Debug.Log("there's more than one scroll bar component attached to this object; should there be?");
		return (WTScrollBarComponent)ComponentsForType(ComponentType.ScrollBar)[0];	
	}
}
