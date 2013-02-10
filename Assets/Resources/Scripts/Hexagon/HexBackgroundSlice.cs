using UnityEngine;
using System.Collections;

public class HexBackgroundSlice : FSprite {
	float baseWidth_;
	float height_;
	
	public HexBackgroundSlice(float angle, float height) : base() {
		height_ = height;
		baseWidth_ = 2 * (height_ * Mathf.Tan(angle / 2f * Mathf.Deg2Rad));
		
		Init(FFacetType.Triangle, Futile.atlasManager.GetElementWithName("Futile_White"),1);
		
		_isAlphaDirty = true;
		UpdateLocalVertices();
	}
	
	override public void PopulateRenderLayer()
	{
		if(_isOnStage && _firstFacetIndex != -1)
		{		
			_isMeshDirty = false;
			
			int vertexIndex0 = _firstFacetIndex * 3;
			
			Vector3[] triangleVertices = _renderLayer.vertices;
			
			Vector2[] triangleUVVertices = _renderLayer.uvs;
			
			Vector2[] shapeVertices = new Vector2[3];
			
			Vector2[] textureUVVertices = new Vector2[3];
			
			Color[] colors = _renderLayer.colors;
			
			shapeVertices[0] = new Vector2(0, 0);
			shapeVertices[1] = new Vector2(height_, -baseWidth_ / 2f);
			shapeVertices[2] = new Vector2(height_, baseWidth_ / 2f);
			
			float uvWidth = (_element.uvTopRight.x - _element.uvTopLeft.x);
			
			triangleUVVertices[0] = new Vector2(_element.uvBottomLeft.x, _element.uvBottomRight.y);
			triangleUVVertices[1] = new Vector2(_element.uvTopRight.x, _element.uvTopRight.y);
			triangleUVVertices[2] = new Vector2(_element.uvTopLeft.x, _element.uvTopLeft.y);
			
			for (int i = 0; i < 3; i++) {
				colors[vertexIndex0 + i] = _alphaColor;
			}
			
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 0], shapeVertices[0],0);
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 1], shapeVertices[1],0);
			_concatenatedMatrix.ApplyVector3FromLocalVector2(ref triangleVertices[vertexIndex0 + 2], shapeVertices[2],0);
						
			triangleUVVertices[vertexIndex0 +  0] = textureUVVertices[0];
			triangleUVVertices[vertexIndex0 +  1] = textureUVVertices[1];
			triangleUVVertices[vertexIndex0 +  2] = textureUVVertices[2];
						
			_renderLayer.HandleVertsChange();
		}
	}
}
