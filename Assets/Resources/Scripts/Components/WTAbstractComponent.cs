using UnityEngine;
using System;
using System.Collections;

public class WTAbstractComponent {
	public WTEntity owner;
	
	protected ComponentType componentType_;
	
	private string name_;
		
	public WTAbstractComponent(string name) {
		name_ = name;
	}
	
	public string name {
		get {return name_;}	
	}
	
	public ComponentType componentType {
		get {return componentType_;}	
	}
	
	virtual public void HandleUpdate() {
		
	}
}
