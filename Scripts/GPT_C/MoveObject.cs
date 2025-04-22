using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public void MoveTo(GameObject target)
    {
        // 首先取得目標位置的Transform物件
        Transform targetTransform = target.transform;

        // 然後將這個gameObject的Transform的parent設定為目標Transform
        target. transform.SetParent(this.transform );

        // 最後重設位置和旋轉值，確保物體正確地貼在新的parent下
        target. transform.localPosition = Vector3.zero;
        target. transform.localRotation = Quaternion.identity;

        this.gameObject.SetActive(true);

        this.GetComponent<Animator>().avatar = target.GetComponent<Animator>().avatar;
    }
}