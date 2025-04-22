using UnityEngine;
using UnityEngine.AI;

public class PathRenderer : MonoBehaviour
{
    public Transform targetPoint;
    public Transform playerPoint;
    public LineRenderer lineRenderer;



    public  NavMeshAgent agent;

    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        lineRenderer.positionCount = 0;
    }

    public void SetTarget(Transform transform)
    {
       
        targetPoint = transform;
    }
    public void SetNull()
    {
        targetPoint = null ;
    }

    void Update()
    {
        if (agent == null || targetPoint == null || playerPoint == null)
        {
            lineRenderer.enabled = false;
            agent.enabled = false;
            return;
        }
        agent.enabled=true;
        lineRenderer.enabled = true;
        //this.transform.position = playerPoint.position;//改為等於playerPoint 前方一米
        this.transform.position = playerPoint.position + playerPoint.forward * 1f;



        lineRenderer.material.mainTextureOffset = new Vector2(Time.time, 0f);
        agent.SetDestination(targetPoint.position);

        if (agent.hasPath)
        {
            lineRenderer.positionCount = agent.path.corners.Length;
            lineRenderer.SetPositions(agent.path.corners);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}