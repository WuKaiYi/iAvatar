using UnityEngine;

public class CreateObject : MonoBehaviour 
{
    public GameObject myObject; //公共游戏对象

    public void OnButtonClick()
    {
        GameObject newObject = Instantiate(myObject); //实例化游戏对象
        //将新的游戏对象设置为如下：
        newObject.transform.position = Vector3.zero;
        newObject.transform.rotation = Quaternion.identity;
        newObject.transform.localScale = Vector3.one;
    }
}