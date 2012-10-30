using UnityEngine;
using System.Collections;

public class WTMain : MonoBehaviour {

	public static FStage currentScene;
	public static WTMain instance;
	
	public enum SceneType {
		None,
		FalldownTest
	}
	
	void Start () {
		if (instance == null) instance = this;
		
		FutileParams fp = new FutileParams(false, false, true, true);
		fp.AddResolutionLevel(640f, 1.0f, 1.0f, "");
		fp.origin = Vector2.zero;
		
		Futile.instance.Init(fp);
		
		Futile.atlasManager.LoadAtlas("Atlases/ExtrudersSheet");
		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadFont("SoftSugar", "SoftSugar.png", "Atlases/SoftSugar");
		
		SwitchToScene(SceneType.FalldownTest);
	}
	
	public void SwitchToScene(SceneType sceneType) {
		if (currentScene != null) Futile.RemoveStage(currentScene);
		
		if (sceneType == SceneType.FalldownTest) currentScene = new WTFalldownTest();
		if (sceneType == SceneType.None) currentScene = null;
		
		if (currentScene != null) Futile.AddStage(currentScene);
	}
}
