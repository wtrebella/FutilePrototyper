using UnityEngine;
using System.Collections;

public class HexCrossBar : FSprite {
	float crossBarHeight_;
	public float originalHeight;
	float smallerBaseWidth_;
	float largerBaseWidth_;
	float distanceFromBackgroundSliceOrigin_ = 0;
	public float previousDistanceFromBackgroundSliceOrigin = 10000;
	
	public HexCrossBar(float height) : base() {
		this.crossBarHeight = height;
		this.originalHeight = height;
		
		Init(FFacetType.Triangle, Futile.atlasManager.GetElementWithName("Futile_White"),3);
		
		_isAlphaDirty = true;
		UpdateLocalVertices();
	}
	
	public float distanceFromBackgroundSliceOrigin {
		get {return distanceFromBackgroundSliceOrigin_;}
		set {
			previousDistanceFromBackgroundSliceOrigin = distanceFromBackgroundSliceOrigin_;
			distanceFromBackgroundSliceOrigin_ = value;
			if (distanceFromBackgroundSliceOrigin_ < 0) distanceFromBackgroundSliceOrigin_ = 0;
			smallerBaseWidth_ = Mathf.Tan(60f / 2f * Mathf.Deg2Rad) * distanceFromBackgroundSliceOrigin_ * 2f;
			largerBaseWidth_ = smallerBaseWidth_ + Mathf.Tan(60f / 2f * Mathf.Deg2Rad) * crossBarHeight_ * 2f;
			_isMeshDirty = true;
		}
	}
	
	override public void PopulateRenderLayer()
	{
		if(_isOnStage && _firstFacetIndex != -1)
		{		
			float inset = (largerBaseWidth_ - smallerBaseWidth_) / 2f;
			
			_isMeshDirty = false;
			
			int vertexIndex0 = _firstFacetIndex * 3;
			
			Vector3[] triangleVertices = _renderLayer.vertices;
			
			Vector2[] triangleUVVertices = _renderLayer.uvs;
			
			Vector2[] shapeVertices = new Vector2[5];
			
			Vector2[] textureUVVertices = new Vector2[5];
			
			Color[] colors = _renderLayer.colors;
			
			shapeVertices[0] = new Vector2(0, crossBarHeight_);
			shapeVertices[1] = new Vector2(inset, 0);
			shapeVertices[2] = new Vector2(largerBaseWidth_ / 2f, crossBarHeight_);
			shapeVertices[3] = new Vector2(inset + smallerBaseWidth_, 0);
			shapeVertices[4] = new Vector2(largerBaseWidth_, crossBarHeight_);
			
			for (int i = 0; i < 5; i++) {
				shapeVertices[i].x -= largerBaseWidth_ / 2f;
			}
			
			float uvWidth = (_element.uvTopRight.x - _element.uvTopLeft.x);
			
			triangleUVVertices[0] = new Vector2(_element.uvTopLeft.x, _element.uvTopLeft.y);
			triangleUVVertices[1] = new Vector2(_element.uvBottomLeft.x, _element.uvBottomLeft.y);
			triangleUVVertices[2] = new Vector2(_element.uvTopLeft.x + uvWidth / 2f, _element.uvTopLeft.y);
			triangleUVVertices[3] = new Vector2(_element.uvBottomRight.x, _element.uvBottomRight.y);
			triangleUVVertices[4] = new Vector2(_element.uvTopRight.x, _element.uvTopRight.y);
			
			for (int i = 0; i < 9; i++) {
				colors[vertexIndex0 + i] = _alphaColor;
			}
			
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 0], shapeVertices[0],0);
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 1], shapeVertices[1],0);
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 2], shapeVertices[2],0);
			
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 3], shapeVertices[1],0);
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 4], shapeVertices[2],0);
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 5], shapeVertices[3],0);
			
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 6], shapeVertices[2],0);
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 7], shapeVertices[3],0);
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 8], shapeVertices[4],0);
						
			triangleUVVertices[vertexIndex0 +  0] = textureUVVertices[0];
			triangleUVVertices[vertexIndex0 +  1] = textureUVVertices[1];
			triangleUVVertices[vertexIndex0 +  2] = textureUVVertices[2];
			
			triangleUVVertices[vertexIndex0 +  3] = textureUVVertices[1];
			triangleUVVertices[vertexIndex0 +  4] = textureUVVertices[2];
			triangleUVVertices[vertexIndex0 +  5] = textureUVVertices[3];
			
			triangleUVVertices[vertexIndex0 +  6] = textureUVVertices[2];
			triangleUVVertices[vertexIndex0 +  7] = textureUVVertices[3];
			triangleUVVertices[vertexIndex0 +  8] = textureUVVertices[4];
						
			_renderLayer.HandleVertsChange();
		}
	}
	
	public float crossBarHeight {
		get {return crossBarHeight_;}
		set {
			crossBarHeight_ = value;
			if (crossBarHeight_ < 0) crossBarHeight_ = 0;
			_isMeshDirty = true;
		}
	}
}
