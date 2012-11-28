using UnityEngine;
using System.Collections;

public class WTMain : MonoBehaviour {

	public static FStage currentScene;
	public static WTMain instance;
	
	public enum SceneType {
		None,
		FalldownTest, // this doesn't work right at all
		Compartments,
		BlahGame
	}
	
	void Start () {
		if (instance == null) instance = this;
		
		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(480f, 1.0f, 1.0f, "-res1");
		fp.AddResolutionLevel(1136f, 2.0f, 2.0f, "-res2");
		fp.AddResolutionLevel(2048f, 4.0f, 4.0f, "-res4");
		fp.backgroundColor = Color.white;
		fp.origin = Vector2.zero;
		
		Futile.instance.Init(fp);
		
		Futile.atlasManager.LoadAtlas("Atlases/ExtrudersSheet");
		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadAtlas("Atlases/CompartmentsSheet");
		Futile.atlasManager.LoadFont("SoftSugar", "SoftSugar.png", "Atlases/SoftSugar");
		
		Go.defaultEaseType = EaseType.SineInOut;
		
		SwitchToScene(SceneType.Compartments);
	}
	
	public void SwitchToScene(SceneType sceneType) {
		if (currentScene != null) Futile.RemoveStage(currentScene);
		
		if (sceneType == SceneType.FalldownTest) currentScene = new WTFalldownTest();
		if (sceneType == SceneType.Compartments) currentScene = new WTCompartments();
		if (sceneType == SceneType.BlahGame) currentScene = new WTBlahGame();
		if (sceneType == SceneType.None) currentScene = null;
		
		if (currentScene != null) Futile.AddStage(currentScene);
	}
}
