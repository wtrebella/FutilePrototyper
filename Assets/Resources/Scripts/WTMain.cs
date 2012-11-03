using UnityEngine;
using System.Collections;

public class WTMain : MonoBehaviour {

	public static FStage currentScene;
	public static WTMain instance;
	
	public enum SceneType {
		None,
		FalldownTest, // this doesn't work right at all
		Compartments
	}
	
	void Start () {
		if (instance == null) instance = this;
		
		FutileParams fp = new FutileParams(false, false, true, true);
		fp.AddResolutionLevel(568f, 1.0f, 1.0f, "");
		fp.AddResolutionLevel(1136f, 2.0f, 2.0f, "-hd");
		fp.origin = Vector2.zero;
		
		Futile.instance.Init(fp);
		
		Futile.atlasManager.LoadAtlas("Atlases/ExtrudersSheet");
		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadAtlas("Atlases/CompartmentsSheet");
		//Futile.atlasManager.LoadAtlas("Atlases/CompartmentsSheet-hd");
		Futile.atlasManager.LoadFont("SoftSugar", "SoftSugar.png", "Atlases/SoftSugar");
		
		SwitchToScene(SceneType.FalldownTest);
	}
	
	public void SwitchToScene(SceneType sceneType) {
		if (currentScene != null) Futile.RemoveStage(currentScene);
		
		if (sceneType == SceneType.FalldownTest) currentScene = new WTFalldownTest();
		if (sceneType == SceneType.Compartments) currentScene = new WTCompartments();
		if (sceneType == SceneType.None) currentScene = null;
		
		if (currentScene != null) Futile.AddStage(currentScene);
	}
}
