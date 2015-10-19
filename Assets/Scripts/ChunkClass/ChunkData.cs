using UnityEngine;
using System.Collections;
public enum MAPDATACHANNEL
{
	R,G,B,A
};

[System.Serializable]
public struct clampPass
{
	public MAPDATACHANNEL channel;
	public float minClamp;
	public float maxClamp;
}
public class ChunkData 
{

	#region //public variables
	public Texture2D mapCells
	{
		get 
		{
			mapData.Apply();
			return mapData; 
		}
		set
		{
			if(value.width != mapData.width ||value.height != mapData.height )
			{
				Debug.Log("error" +this+"is not the same dimensions as the vaue you provided");
			}
			mapData = value;
		}
	}
	public int mapSize
	{
		get 
		{
			return size; 
		}
	}
	#endregion

	#region //internal variables
	protected Texture2D mapData;
	protected int size;
	#endregion

	#region //constructors
	/// <summary>
	/// Initializes a new instance of the <see cref="ChunkData"/> class. with a premade image and size
	/// </summary>
	/// <param name="mapSize">Map size.</param>
	/// <param name="image">Image.</param>
	public ChunkData(int mapSize, Texture2D image)
	{
		initalize(mapSize,image);

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ChunkData"/> class with an empty image
	/// </summary>
	/// <param name="size">Size.</param>
	public ChunkData(int mapSize)
	{
		initalize(mapSize,new Texture2D(mapSize,mapSize));
	}

	private void initalize(int mapSize, Texture2D image) // havge this here pulery so i dont duplicate logic
	{
		if(image.height != mapSize || image.width != mapSize)
		{
			Debug.LogError("the size you provided for " +this+" does not match the size of the image you provided");
		}
		size = mapSize;
		mapData = image;
		mapData.filterMode = FilterMode.Point;
		mapData.wrapMode = TextureWrapMode.Clamp;
	}
	#endregion
	public void fillAllWithRandomPerlin(float perlinZoom)
	{
		fillWithPerlin(	Random.Range(0f,10f), 	Random.Range(0f,10f),	perlinZoom, MAPDATACHANNEL.R);
		fillWithPerlin(	Random.Range(0f,10f), 	Random.Range(0f,10f),	perlinZoom, MAPDATACHANNEL.G);
		fillWithPerlin(	Random.Range(0f,10f), 	Random.Range(0f,10f),	perlinZoom, MAPDATACHANNEL.B);
		fillWithPerlin(	Random.Range(0f,10f),	Random.Range(0f,10f),	perlinZoom, MAPDATACHANNEL.G);
	}

	/// <summary>
	/// fills the channel specified with perlin noise with a seed
	/// </summary>
	/// <param name="seedX">Seed x.</param>
	/// <param name="seedY">Seed y.</param>
	/// <param name="perlinZoom">Perlin zoom.</param>
	public void fillWithPerlin(float seedX, float seedY,float perlinZoom, MAPDATACHANNEL channel)
	{
		for(int y =0; y < size; y++)
		{
			for(int x =0; x < size; x++)
			{
				float cellValue =Mathf.PerlinNoise(seedX + x * perlinZoom,seedY + y* perlinZoom);
				accesWithMdc(channel,x,y,cellValue);  
			}
		}
		mapData.Apply();
	}
	/// <summary>
	/// fills the channel specified with perlin noise with no seed
	/// </summary>
	/// <param name="perlinZoom">Perlin zoom.</param>
	public void fillWithPerlin(float perlinZoom,MAPDATACHANNEL channel)
	{
		fillWithPerlin(0,0,perlinZoom, channel);
	}

	private void accesWithMdc(MAPDATACHANNEL channel,int x, int y,float value)
	{
		mapData.SetPixel(x,y,setChannel(channel,mapData.GetPixel(x,y),value));
	}

	Color setChannel(MAPDATACHANNEL channel,Color pixel,float newValue)
	{
		switch(channel)
		{
		case MAPDATACHANNEL.R:
			return 	new Color(newValue,pixel.g,pixel.b,pixel.a);
			break;
		case MAPDATACHANNEL.G:
			return 	new Color(pixel.r,newValue,pixel.b,pixel.a);
			break;
		case MAPDATACHANNEL.B:
			return 	new Color(pixel.r,pixel.g,newValue,pixel.a);
			break;		
		case MAPDATACHANNEL.A:
			return 	 new Color(pixel.r,pixel.g,pixel.b,newValue);
			break;

		}
		Debug.LogError("daraChannel error in" + this);
		return Color.black;
	}

	protected float getChannel(MAPDATACHANNEL channel,Color yourColor)
	{
		switch(channel)
		{
		case MAPDATACHANNEL.R:
			return yourColor.r;
			break;
		case MAPDATACHANNEL.G:
			return yourColor.g;
			break;
		case MAPDATACHANNEL.B:
			return yourColor.b;
			break;		
		case MAPDATACHANNEL.A:
			return yourColor.a;
			break;
			
		}
		Debug.LogError("daraChannel error in" + this);
		return 0f;
	}


	protected void clampValues(MAPDATACHANNEL channel, float min, float max)
	{
		Color[] mapColor = mapData.GetPixels();


		float newValue =0;
		for(int i =0; i < mapColor.Length; i++)
		{
			newValue = getChannel(channel,mapColor[i]);//get the value
			newValue = Mathf.Clamp(newValue,min,max);//clamp it
			mapColor[i] = setChannel(channel,mapColor[i],newValue);//place it back in the array
		}

		mapData.SetPixels(mapColor);
	}

	public void clampChannelValues(clampPass instructions)
	{
		clampValues(instructions.channel,instructions.minClamp,instructions.maxClamp);
	}

}
