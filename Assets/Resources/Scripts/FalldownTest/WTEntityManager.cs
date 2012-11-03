using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTEntityManager {
	public List<WTEntity> fallingBlocks = new List<WTEntity>();
	
	public WTEntityManager() {		
		InitFallingBlocks();
	}
	
	private void InitFallingBlocks() {
		for (int i = 0; i < 100; i++) {
			WTEntity block = new WTEntity("square.psd", new WTFallingBlockComponent());
			block.sprite.width = block.sprite.height = WTConfig.TILE_SIZE;
			block.x = Futile.screen.halfWidth;
			block.y = Futile.screen.height + block.sprite.height / 2f;
			block.xPreviousGlobal = Futile.screen.halfWidth;
			block.yPreviousGlobal = Futile.screen.height + block.sprite.height / 2f;
			fallingBlocks.Add(block);
		}
	}
	
	public int GetNumberOfFallingBlocksBeingUsed() {
		int num = 0;
		
		foreach (WTEntity block in fallingBlocks) {
			if (block.fallingBlockComponent.isBeingUsed) num++;	
		}
		
		return num;
	}
	
	public WTEntity GetUnusedFallingBlock() {
		foreach (WTEntity block in fallingBlocks) {
			if (!block.fallingBlockComponent.isBeingUsed) {
				block.fallingBlockComponent.isBeingUsed = true;
				return block;
			}
		}
		
		Debug.Log("No unused blocks");
		return null;
	}
	
	public void RecycleConstruct(WTEntityConstruct construct) {
		foreach (WTEntity entity in construct.entities) RecycleEntity(entity);	
	}
	
	public void RecycleEntity(WTEntity entity) {
		entity.fallingBlockComponent.isBeingUsed = false;
		entity.x = Futile.screen.halfWidth;
		entity.y = Futile.screen.height + entity.sprite.height / 2f;
		entity.xPreviousGlobal = Futile.screen.halfWidth;
		entity.yPreviousGlobal = Futile.screen.height + entity.sprite.height / 2f;
	}
}
