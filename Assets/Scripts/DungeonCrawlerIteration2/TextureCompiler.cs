using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class TextureCompiler //this class will create a 2d texture base on the array of charateds passed
{
	public Texture2D mapTexture;
	public World myWorld;


	public TextureCompiler (char[,] dataArrray,World _myWorld)
	{
		myWorld = _myWorld;
		buildTexture (dataArrray);
	}

	protected void buildTexture(char[,] data)
	{
		mapTexture = new Texture2D (data.GetLength(0),data.GetLength(1));
		for (int y =0; y < data.GetLength(1); y++) 
		{
			for (int x =0; x < data.GetLength(0); x++) 
			{
				mapTexture.SetPixel(x,y,findColor(data[x,y]));
			}
		}
		mapTexture.Apply ();
	}

	private Color findColor(char letterAlias)
	{

		for (int i= 0; i <  myWorld.charColors.Length; i++) 
		{
			if(  myWorld.charColors[i].letter == letterAlias)
			{
				return   myWorld.charColors[i].color;
			}
		}
		return Color.black; //if we dont find the leter we asume the desired color is black
	}


}
