using UnityEngine;
using System.Collections;

public class WTMain : MonoBehaviour {

	public static FStage currentScene;
	public static WTMain instance;
	
	public enum SceneType {
		None,
		FalldownTest, // this doesn't work right at all
		Compartments,
		FrendenGame,
		BlahGame
	}
	
	void Start () {
		if (instance == null) instance = this;
		
		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(480f, 1.0f, 1.0f, "");

		fp.backgroundColor = Color.white;
		fp.origin = Vector2.zero;
		
		Futile.instance.Init(fp);
		
		Futile.atlasManager.LoadAtlas("Atlases/ExtrudersSheet");
		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadAtlas("Atlases/CompartmentsSheet");
		//Futile.atlasManager.LoadFont("SoftSugar", "SoftSugar.png", "Atlases/MainSheet", 0, 0);
		
		Go.defaultEaseType = EaseType.SineInOut;
		
		SwitchToScene(SceneType.FrendenGame);
	}
	
	public void SwitchToScene(SceneType sceneType) {
		if (currentScene != null) Futile.RemoveStage(currentScene);
		
		if (sceneType == SceneType.FalldownTest) currentScene = new WTFalldownTest();
		if (sceneType == SceneType.Compartments) currentScene = new WTCompartments();
		if (sceneType == SceneType.BlahGame) currentScene = new WTBlahGame();
		if (sceneType == SceneType.FrendenGame) currentScene = new WTFrendenGame();
		if (sceneType == SceneType.None) currentScene = null;
		
		if (currentScene != null) Futile.AddStage(currentScene);
	}
}
