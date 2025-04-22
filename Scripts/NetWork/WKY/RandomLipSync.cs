using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class RandomLipSync : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
  
    }

    public void init()
    {
        meshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        StartCoroutine(rand());
    }

    float time =0;
    IEnumerator rand()
    {
        float max = meshRenderer.sharedMesh.GetBlendShapeFrameWeight(0, meshRenderer.sharedMesh.GetBlendShapeFrameCount(0) - 1);
        while (time > Time.time)
        {
            float t = Random.Range(0.1f, 0.2f);

            DOTween.To(() => weight, x => weight = x, Random.Range(max * 0.4f, max), t);

            UI.SetActive(true);
            yield return new WaitForSeconds(t);
        }
        DOTween.To(() => weight, x => weight = x, 0, 0.1f);
        UI.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(rand());
    }
    bool isSpeak = false;
    public void Speaking()
    {
        time = Time.time + 0.5f;
    }

    public void RandoLip()
    { 
    
    }
    public float weight = 0;
    // Update is called once per frame
    void Update()
    {if (meshRenderer == null)
            return;

        meshRenderer.SetBlendShapeWeight(0, weight);

        UI.transform.LookAt(Camera.main.transform);

    }
}
