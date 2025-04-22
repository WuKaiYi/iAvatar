using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OpenGallery : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

	public void OpenGalleryOnChick()
	{
	 	GetComponent<DImageDisplayHandler>().SetRawImage(GetComponent<RawImage>(), PickImage(1024));
	}
	private Texture2D PickImage(int maxSize)
	{
		Texture2D texture = new Texture2D (0,0);
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
		{
			
			Debug.Log("Image path: " + path);
			if (path != null)
			{
				// Create Texture from selected image
				 texture = NativeGallery.LoadImageAtPath(path, maxSize);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}
                




				// If a procedural texture is not destroyed manually, 
				// it will only be freed after a scene change
				//Destroy(texture, 5f);
			}
		});
		return texture;
		Debug.Log("Permission result: " + permission);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
