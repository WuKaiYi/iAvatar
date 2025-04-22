using ES3Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPrefabManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // The prefabs we want to instantiate.
    // Before adding prefabs to this array, you should right-click them and select Easy Save 3 > Enable Easy Save for Prefab.
    // You should also add an Easy Save 3 Manager to your scene by going to Tools > Easy Save 3 > Add Manager to Scene.
    public GameObject[] prefabs;
    // When we instantiate a prefab we'll put it in this list.
    private List<GameObject> prefabInstances = new List<GameObject>();

    public void UploadImage()
    {
        // Pick a prefab from the prefabs array at random.
        var prefab = prefabs[Random.Range(0, prefabs.Length)];
        // Instantiate the prefab at a random location within 5 units of (0,0).
        var prefabInstance = Instantiate(prefab);
        prefabInstance.GetComponent<BuildPrefabObj>().obj_i = 1;
        prefabInstance.GetComponentInChildren<OpenGallery>().OpenGalleryOnChick();

        // Add the prefab instance to the prefabInstances List.
        prefabInstances.Add(prefabInstance);
    }

    public Transform tagransform;
    public LayerMask obstacleLayer;
    public float spawnDistance = 2f;
    void put_obj(GameObject prefab)
    {

    }

    public void Upload(int i)
    {

        /*      // Pick a prefab from the prefabs array at random.
              var prefab = prefabs[Random.Range(0, prefabs.Length)];

              Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
              RaycastHit hit;
              if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, spawnDistance))
              {
                  spawnPosition = hit.point;
              }
              GameObject prefabInstance = Instantiate(prefab, new Vector3(spawnPosition.x, spawnPosition.y,0), Quaternion.identity);*/

        var prefab = prefabs[Random.Range(0, prefabs.Length)];
        var prefabInstance = Instantiate(prefab, tagransform.position + Camera.main.transform.forward * spawnDistance, Quaternion.identity);
        Vector3 lookAtPosition = new Vector3(Camera.main.transform.position.x, prefabInstance.transform.position.y, Camera.main.transform.position.z);
        prefabInstance.transform.LookAt(lookAtPosition);

        // Instantiate the prefab at a random location within 5 units of (0,0).



        prefabInstance.GetComponent<BuildPrefabObj>().obj_i = i;

        prefabInstances.Add(prefabInstance);
    }
    // Instantiates a random prefab at a random position.
    public void CreateRandomPrefab()
    {
        // Pick a prefab from the prefabs array at random.
        var prefab = prefabs[Random.Range(0, prefabs.Length)];
        // Instantiate the prefab at a random location within 5 units of (0,0).
        var prefabInstance = Instantiate(prefab);
        // Add the prefab instance to the prefabInstances List.
        prefabInstances.Add(prefabInstance);
    }
    public void FindES3Prefabs()
    {
        //找到场景中所有带有ES3Prefab组件的物体，并将其加入prefabInstances列表中
        var es3Prefabs = FindObjectsOfType<ES3Prefab>();
        foreach (var es3Prefab in es3Prefabs)
        {
            Debug.Log(es3Prefab.gameObject.name);
            prefabInstances.Add(es3Prefab.gameObject);
        }
        SavePrefabInstances();
    }

    public void SavePrefabInstances()
    {
        // Save our prefabInstances list to file using "prefabInstances" as the unique key to idetify the data.
        ES3.Save("prefabInstances", prefabInstances, "BuildObjs.es3");
    }

    public void LoadPrefabInstances()
    {
        // Load our prefabInstances List using the same unique key as we used to save it.
        // If no save data exists it will return an empty List.
        // If prefab instances with these reference IDs still exist.
        prefabInstances = ES3.Load("prefabInstances", "BuildObjs.es3", new List<GameObject>());
    }
}
