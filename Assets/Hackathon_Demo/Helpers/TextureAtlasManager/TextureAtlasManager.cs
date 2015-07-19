using UnityEngine;

using System.Collections.Generic;

namespace UnitySlippyMap
{


public class TextureAtlasManager : MonoBehaviour
{
    #region Singleton implementation

    private static TextureAtlasManager instance = null;
    public static TextureAtlasManager Instance
    {
        get
        {
            if (null == (object)instance)
            {
                instance = FindObjectOfType(typeof(TextureAtlasManager)) as TextureAtlasManager;
                if (null == (object)instance)
                {
                    var go = new GameObject("[TextureAtlasManager]");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    instance = go.AddComponent<TextureAtlasManager>();
                    instance.EnsureAtlasManager();
                }
            }

            return instance;
        }
    }

    private void EnsureAtlasManager()
    {
        atlases = new Dictionary<int, TextureAtlas>();
        textureAtlasMap = new Dictionary<int, KeyValuePair<int, int>>();
    }

    private TextureAtlasManager()
    {
    }

    private void OnApplicationQuit()
    {
        DestroyImmediate(this.gameObject);
    }

    #endregion

    #region Private members & properties

    private Dictionary<int, TextureAtlas>           atlases;
    private Dictionary<int, KeyValuePair<int, int>> textureAtlasMap;
    private int                                     atlasSize = 512;
	
	private float									lastTimeTextureWasApplied = 0.0f;
	private float									applyDelay = 1.0f;

    #endregion

    #region MonoBehaviour implementation

    private void Start()
    {
    }

    private void Update()
    {
		
		if (lastTimeTextureWasApplied == 0.0f
			|| (lastTimeTextureWasApplied - Time.time) > applyDelay)
		{
			foreach (KeyValuePair<int, TextureAtlas> entry in atlases)
			{
				if (entry.Value.IsDirty)
				{
					entry.Value.Apply();
					lastTimeTextureWasApplied = Time.time;
					break ;
				}
			}
		}
		
    }

    private void OnDestroy()
    {
        instance = null;
    }

    #endregion

    #region Private methods

 
    private int CreateAtlas()
    {
        TextureAtlas atlas = new TextureAtlas(atlasSize,"512");

        int newIndex = 0;
        while (atlases.ContainsKey(newIndex))
        {
            ++newIndex;
        }

        atlases.Add(newIndex, atlas);

        return newIndex;
    }

    #endregion

    #region Public methods

    public int AddTexture(Texture2D texture)
    {
        
        List<KeyValuePair<int, TextureAtlas>> list = new List<KeyValuePair<int, TextureAtlas>>();
        foreach (KeyValuePair<int, TextureAtlas> entry in atlases)
        {
            list.Add(entry);
        }
        list.Sort((firstPair, nextPair) =>
        {
            return Mathf.RoundToInt(firstPair.Value.Occupancy() - nextPair.Value.Occupancy());
        });

        
        int atlasId = -1;
        int textureId = -1;
        foreach (KeyValuePair<int, TextureAtlas> entry in list)
        {
            textureId = entry.Value.AddTexture(texture);
            if (textureId != -1)
            {
                atlasId = entry.Key;
                break;
            }
        }

        if (textureId == -1)
        {
            atlasId = CreateAtlas();
            textureId = atlases[atlasId].AddTexture(texture);
        }

        
        int newId = 0;
        while (textureAtlasMap.ContainsKey(newId))
        {
            ++newId;
        }
        textureAtlasMap.Add(newId, new KeyValuePair<int, int>(atlasId, textureId));

        Debug.Log("DEBUG: new texture: " + textureId + " in atlas: " + atlasId);
         
        return newId;
    }

    public void RemoveTexture(int textureId)
    {
        Debug.Log("DEBUG: TextureAtlasManager.RemoveTexture: textureId: " + textureId);
		if (textureAtlasMap.ContainsKey(textureId) == false)
			return ;
        KeyValuePair<int, int> entry = textureAtlasMap[textureId];
        atlases[entry.Key].RemoveTexture(entry.Value);
        textureAtlasMap.Remove(textureId);
    }

    public TextureAtlas.TextureInfo GetTextureInfo(int textureId)
    {
        KeyValuePair<int, int> entry = textureAtlasMap[textureId];
        return atlases[entry.Key].GetTextureInfo(entry.Value);
    }

    #endregion
}


}