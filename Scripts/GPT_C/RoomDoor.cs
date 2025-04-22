using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class RoomDoor : MonoBehaviour
{
    private NavMeshObstacle Obstacle;

    public bool IsOpen = false;
    [SerializeField]
    private float Speed = 1f;
    [SerializeField]
    private float RotationAmount = 90f;
    [SerializeField]
    private float ForwardDirection = 0;


    private Coroutine AnimationCoroutine;
    [SerializeField]
    private bool reverseForward;

    private void Awake()
    {
        Obstacle = GetComponent<NavMeshObstacle>();
        Obstacle.carveOnlyStationary = false;
        Obstacle.carving = IsOpen;
        Obstacle.enabled = IsOpen;

    }

    public void Open(Vector3 UserPosition)
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            float dot = Vector3.Dot(transform.forward, (UserPosition - transform.position).normalized);
            AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {

        Debug.Log(ForwardAmount >= ForwardDirection);
        if (ForwardAmount >= ForwardDirection)
        {
            this.transform.DOLocalRotate(new Vector3(0, 90* (reverseForward ? -1 : 1), 0), Speed);
        }
        else
        {
            this.transform.DOLocalRotate(new Vector3(0, 270* (reverseForward ? -1 : 1), 0), Speed);
        }

        IsOpen = true;

        yield return new WaitForSeconds(Speed);
        Obstacle.enabled = true;
        Obstacle.carving = true;
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(DoRotationClose());
        }
    }

    private IEnumerator DoRotationClose()
    {
        Obstacle.carving = false;
        Obstacle.enabled = false;



        IsOpen = false;



        this.transform.DOLocalRotate(new Vector3(0, 0, 0), Speed);

        this.transform.DOLocalRotate(new Vector3(0, 0, 0), Speed);

        yield return new WaitForSeconds(Speed);
    }
}