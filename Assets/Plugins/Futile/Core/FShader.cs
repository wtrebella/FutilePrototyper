using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FShader
{
	static public FShader defaultShader;
	
	//shader types
	public static FShader Normal;
	public static FShader Additive;
	public static FShader AdditiveColor;
	public static FShader Solid;
	public static FShader SolidColored;
	
	private static int _nextShaderIndex = 0;
	private static List<FShader> _shaders = new List<FShader>();
	
	public int index;
	public string name;
	public Shader shader;
	
	private FShader()
	{
		throw new NotSupportedException("Use FShader.CreateShader() instead");
	}
	
	private FShader (string name, Shader shader, int index) //only to be constructed inside this class using CreateShader()
	{
		this.index = index;
		this.name = name;
		this.shader = shader; 
	}
	
	public static void Init() //called by Futile
	{
		Normal = CreateShader("Normal", Shader.Find("Futile/Normal"));	
		Additive = CreateShader("Additive", Shader.Find("Futile/Additive"));	
		AdditiveColor = CreateShader("AdditiveColor", Shader.Find("Futile/AdditiveColor"));	
		Solid = CreateShader("Solid", Shader.Find("Futile/Solid"));	
		SolidColored = CreateShader("SolidColored", Shader.Find("Futile/SolidColored"));	
		
		defaultShader = Normal;
	}
	
	//create your own FShaders by creating them here
	
	public static FShader CreateShader(string shaderShortName, Shader shader)
	{
		for(int s = 0; s<_shaders.Count; s++)
		{
			if(_shaders[s].name == shaderShortName) return _shaders[s]; //don't add it if we have it already	
		}
		
		FShader newShader = new FShader(shaderShortName, shader, _nextShaderIndex++);
		_shaders.Add (newShader);
		
		return newShader;
	}
	
}


