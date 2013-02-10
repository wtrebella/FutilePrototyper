using UnityEngine;
using System.Collections;

public class WTMain : MonoBehaviour {

	public static FStage currentScene;
	public static WTMain instance;
	
	public enum SceneType {
		None,
		Compartments,
		FrendenGame,
		Hexagon,
		BlahGame
	}
	
	void Start () {
		if (instance == null) instance = this;
		
		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(1024f, 1.0f, 1.0f, "");

		fp.backgroundColor = new Color(0.12f, 0.12f, 0.12f, 1.0f);
		fp.origin = Vector2.zero;
		
		Futile.instance.Init(fp);
		
		Futile.atlasManager.LoadAtlas("Atlases/ExtrudersSheet");
		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadAtlas("Atlases/CompartmentsSheet");
		//Futile.atlasManager.LoadFont("SoftSugar", "SoftSugar", "Atlases/MainSheet", 0, 0);
		
		Go.defaultEaseType = EaseType.SineInOut;
		
		SwitchToScene(SceneType.Hexagon);
	}
	
	public void SwitchToScene(SceneType sceneType) {
		if (currentScene != null) Futile.RemoveStage(currentScene);
		
		if (sceneType == SceneType.Compartments) currentScene = new WTCompartments();
		if (sceneType == SceneType.BlahGame) currentScene = new WTBlahGame();
		if (sceneType == SceneType.FrendenGame) currentScene = new WTFrendenGame();
		if (sceneType == SceneType.Hexagon) currentScene = new WTHexagon();
		if (sceneType == SceneType.None) currentScene = null;
		
		if (currentScene != null) Futile.AddStage(currentScene);
	}
}
