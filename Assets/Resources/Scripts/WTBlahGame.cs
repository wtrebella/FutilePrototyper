using UnityEngine;
using System.Collections;

public class WTBlahGame : FStage {
	FSprite nursesPass;
	
	public WTBlahGame() : base("") {
		nursesPass = new FSprite("nursesPass.psd");
		nursesPass.x = Futile.screen.halfWidth;
		nursesPass.y = Futile.screen.halfHeight;
		nursesPass.scale = 0.5f;
		AddChild(nursesPass);
		
		Go.defaultEaseType = EaseType.SineInOut;
		
		Tween tweenUp = new Tween(nursesPass, 0.3f, new TweenConfig()
			.floatProp("scale", 0.6f));
		Tween tweenDown = new Tween(nursesPass, 0.3f, new TweenConfig()
			.floatProp("scale", 0.5f));
		TweenChain chain = new TweenChain();
		chain.setIterations(-1);
		chain.append(tweenUp).append(tweenDown);
		Go.addTween(chain);
		chain.play();
	}
}
