using UnityEngine;
using System.Collections;

[System.Serializable]
public struct clampPass
{
	public int channel;
	public float minClamp;
	public float maxClamp;
}

public enum edgeRoundStyle
{
	LINEAR_BOX,EXP_BOX,LINEAR_OVAL,EXP_OVAL
}

public struct clampSettings
{
	public edgeRoundStyle roundStyle;
	public int paddingRows;
	//exp value?
}
public class ChunkData 
{

	#region //public variables
	public Texture2D displayTexture
	{
		get 
		{
			//populate here
			return buildDisplayTexture(); //only run when values changed 
		}
	}

	Texture2D buildDisplayTexture()
	{
		Texture2D displayTex = new Texture2D(length,width);
		displayTex.wrapMode = TextureWrapMode.Clamp;
		displayTex.filterMode = FilterMode.Point;
		for(int y =0; y < length; y++)
		{
			for(int x =0; x < width; x++)
			{
				switch(dimentions)
				{
				case 0:
					break;
				case 1:
					displayTex.SetPixel(x,y,new Color(_mapInfo[x,y,0],0,0,0));
					break;
				case 2:
					displayTex.SetPixel(x,y,new Color(_mapInfo[x,y,0],_mapInfo[x,y,1],0,0));
					break;
				case 3:
					displayTex.SetPixel(x,y,new Color(_mapInfo[x,y,0],_mapInfo[x,y,1],_mapInfo[x,y,2],0));
					break;
				default:
					displayTex.SetPixel(x,y,new Color(_mapInfo[x,y,0],_mapInfo[x,y,1],_mapInfo[x,y,2],_mapInfo[x,y,3]));
					break;
				}
			}
		}
		displayTex.Apply();
		return displayTex;
	}
	public int width //x axis
	{
		get 
		{
			return _mapInfo.GetLength(0); 
		}
	}
	public int length//y axis
	{
		get 
		{
			return _mapInfo.GetLength(1); 
		}
	}
	public int dimentions
	{
		get 
		{
			return _mapInfo.GetLength(2); 
		}
	}
	public clampSettings edgeRound
	{
		get
		{
			return _edgeRound;
		}
		set
		{
			_edgeRound = value;
		}
	}


	public float[,,] mapInfo 
	{
		get
		{
			return _mapInfo;
		}
	}
	#endregion
	#region //internal variables
	private clampSettings _edgeRound;
	protected float[,,]  _mapInfo;
	/*{
		get
		{
			return _mapInfo;
		}
		set
		{
			//we need to know the x and y of the value before we can set it then  we can use round edges
			//_mapInfo = value;
		}
	}
	*/
	#endregion
	#region //constructors
	/// <summary>
	/// Initializes a new instance of the <see cref="ChunkData"/> class. with a premade image and size
	/// </summary>
	/// <param name="mapSize">Map size.</param>
	/// <param name="image">Image.</param>
	public ChunkData(float[,,] values)
	{
		initalize(values);

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ChunkData"/> class with an empty image
	/// </summary>
	/// <param name="size">Size.</param>
	public ChunkData(int width,int length)//defaults to 4 dimentions
	{

		initalize(new float [width,length,4]);
	}

	private void initalize(float[,,] values) // havge this here pulery so i dont duplicate logic
	{
		if(values.GetLength(0) < 2 || values.GetLength(1) < 2) //ensures the map is a square
		{
			Debug.LogError("the size you provided for " +this+" is not large enough to build a quad");
		}
		_mapInfo = values;
	}
	#endregion
	#region //functions

	private void roundEdges(ref float original)
	{
		//
		switch(_edgeRound.roundStyle)
		{
		case edgeRoundStyle.LINEAR_BOX:
				
				break;

		default:
				Debug.Log("error"+_edgeRound.roundStyle+" is not implimented yet");
				break;
		}
	}


	public void fillAllWithRandomPerlin(float perlinZoom)
	{
		for(int i=0; i < dimentions; i++)
		{
			fillWithPerlin(	Random.Range(0f,10f), 	Random.Range(0f,10f),	perlinZoom,i);
		}

	}

	/// <summary>
	/// fills the channel specified with perlin noise with a seed
	/// </summary>
	/// <param name="seedX">Seed x.</param>
	/// <param name="seedY">Seed y.</param>
	/// <param name="perlinZoom">Perlin zoom.</param>
	public void fillWithPerlin(float seedX, float seedY,float perlinZoom, int channel)
	{
		for(int y =0; y < length; y++)
		{
			for(int x =0; x < width; x++)
			{
				float cellValue =Mathf.PerlinNoise(seedX + x * perlinZoom,seedY + y* perlinZoom);
				_mapInfo[x,y,channel] = cellValue;  
			}
		}
	}
	/// <summary>
	/// fills the channel specified with perlin noise with no seed
	/// </summary>
	/// <param name="perlinZoom">Perlin zoom.</param>
	public void fillWithPerlin(float perlinZoom,int channel)
	{
		fillWithPerlin(0,0,perlinZoom, channel);
	}
	
	protected void clampValues(int channel, float min, float max)
	{
		float newValue = 0;
		for(int y =0; y < length; y++)
		{
			for(int x =0; x < width; x++)
			{
				
				newValue = _mapInfo[x,y,channel];
				newValue = Mathf.Clamp(newValue,min,max);//clamp it
				_mapInfo[x,y,channel] = newValue; //place the cmaped value back int the array
			}
		}
	}
	

	public void clampChannelValues(clampPass instructions)
	{
		clampValues(instructions.channel,instructions.minClamp,instructions.maxClamp);
	}
	#endregion
	
}
