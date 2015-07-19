using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;

namespace UnitySlippyMap
{

public class TextureAtlas
{
    public class TextureInfo
    {
        private Rect        rect;
        public Rect         Rect { get { return rect; } }
        private Texture2D   texture;
		public Texture2D 	Texture { get { return texture; } }

        public TextureInfo(Rect rect, Texture2D texture)
        {
            this.rect = rect;
            this.texture = texture;
        }
    }

    #region Private members & properties

    private Texture2D                   texture;

    private MaxRectsBinPack             pack;
    private Dictionary<int, Rect>       rects;
	private bool						isDirty = false;
	public bool							IsDirty { get { return isDirty; } }

    #endregion

    #region Private methods
    #endregion

    #region Public methods

    public TextureAtlas(int size, string name)
    {
        texture = new Texture2D(size, size);
        if (name != null)
            texture.name = name;
        else
            texture.name = Guid.NewGuid().ToString();
        pack = new MaxRectsBinPack(size, size, false);
        rects = new Dictionary<int, Rect>();
    }

  
    public void Defragment()
    {
    }

    public float Occupancy()
    {
        return pack.Occupancy();
    }
	
	public void Apply()
	{
		isDirty = false;
		Stopwatch watch = new Stopwatch();
		watch.Start();
		texture.Apply();
		watch.Stop();
		
		TimeSpan ts = watch.Elapsed;
        UnityEngine.Debug.Log(String.Format("DEBUG: applied in: {0:00}:{1:00}:{2:00}.{3:00}", 
                    ts.Hours, ts.Minutes, ts.Seconds, 
                    ts.Milliseconds/10));
	}

 
    public int AddTexture(Texture2D texture)
    {
		Rect rect = pack.Insert(texture.width, texture.height, MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestAreaFit);
        if (rect == new Rect())
            return -1;

        int newIndex = 0;
        while (rects.ContainsKey(newIndex))
        {
            ++newIndex;
        }

        rects.Add(newIndex, rect);

        new Job(pixelsWorker(texture, rect), this, true);
		
		

        return newIndex;
    }
	
	private IEnumerator pixelsWorker(Texture2D texture, Rect rect)
	{
		int x = Mathf.RoundToInt(rect.x);
		int y = Mathf.RoundToInt(rect.y);
		int width = Mathf.RoundToInt(rect.width);
		int height = Mathf.RoundToInt(rect.height);
		
		
		Color[] pixels = texture.GetPixels();
		yield return new WaitForFixedUpdate();
		this.texture.SetPixels(x, y, width, height, pixels);
		yield return new WaitForFixedUpdate();
		this.texture.Apply();

		

		this.isDirty = true;
		
	}


    public void RemoveTexture(int id)
    {
        pack.Remove(rects[id]);
        rects.Remove(id);
    }

    public TextureInfo GetTextureInfo(int id)
    {
        return new TextureInfo(rects[id], texture);
    }

    #endregion
}

}