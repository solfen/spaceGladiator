using UnityEngine;
using System.Collections;

public class TileAnim : MonoBehaviour {
	
	public int columns;
	public int rows;
	private Vector2 tileSize;
	public float interval;
	private Renderer spriteRenderer;
	private float nextUpdate;
	private Vector2 tileOffset;
	private int frameNumber;
	private int currentRow;
	
	
	void Start () {
			
		tileSize = new Vector2(1.0f / columns, 1.0f / rows);
		
		spriteRenderer = gameObject.GetComponentInChildren<Renderer>();
		
		spriteRenderer.material.SetTextureScale("_MainTex", tileSize);
				
		nextUpdate = Time.time;
				
		frameNumber = 0;
		currentRow = 0;
	}
	
	void Update () {
				
		
		if (Time.time >= nextUpdate){
			
			spriteRenderer.material.SetTextureOffset("_MainTex", new Vector2(tileSize.x * frameNumber, tileSize.y * (rows - currentRow)));
			
			frameNumber += 1;
			                                         
			if (frameNumber > columns - 1){
				currentRow += 1;
				frameNumber = 0;
			}
			
			if (currentRow > rows - 1){
				currentRow = 0;				
			}
			
			nextUpdate = Time.time + interval;
			
		}
	}
}
