using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPrefabObj : MonoBehaviour
{
    public int obj_i = 0;
    public GameObject[] BuildObj;
    public LeanSelectableByFinger LeanSelectableByFinger;

    GameObject obj;

    public string Model_Link;

    public Vector3 localPos = Vector3.zero;
    public Vector3 ObjScale=Vector3.one;
    // Start is called before the first frame update

    public void Delete()
    {
        Destroy(this.gameObject);
    }

    async void Start()
    {
    

        if (Model_Link != "")
        {
            //var gltf = gameObject.AddComponent<GLTFast.GltfAsset>();
            var gltf = new GLTFast.GltfImport();

           // gltf.Url = Model_Link;
            var success = await gltf.Load(Model_Link);
            if (success)
            {
                // Here you can customize the post-loading behavior

                // Get the first material
                var material = gltf.GetMaterial();
                Debug.LogFormat("The first material is called {0}", material.name);

                // Instantiate the glTF's main scene
                await gltf.InstantiateMainSceneAsync(this.transform.GetChild(0));
                // Instantiate the glTF's main scene
                //await gltf.InstantiateMainSceneAsync(new GameObject("Instance 2").transform);

                // Instantiate each of the glTF's scenes
                for (int sceneId = 0; sceneId < gltf.SceneCount; sceneId++)
                {
                   // await gltf.InstantiateSceneAsync(transform, sceneId);
                }
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
        }
        else
        {
            obj = Instantiate(BuildObj[obj_i], this.transform.GetChild(0));
            obj.SetActive(false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.SetActive(true );


            // GetComponent<MeshCollider>().sharedMesh = obj.GetComponentInChildren<MeshFilter>().mesh;
        }



     
    }

    // Update is called once per frame
    void Update()
    {
       // this.transform.GetChild(0).localScale = ObjScale;
      //  this.transform.GetChild(0).localPosition = localPos;
    }

   

}
