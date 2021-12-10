using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetobject;//角色
    private float screenWidth;

    // Start is called before the first frame update
    void Start()
    {
        float height = Camera.main.orthographicSize * 2.0f;
        screenWidth = height * Camera.main.aspect;//屏幕宽
    }

    // Update is called once per frame
    void Update()
    {
        float targetObjectX = targetobject.transform.position.x;//角色位置
        Vector3 newCameraPostion = transform.position;//相机位置
        if(targetObjectX>= (newCameraPostion.x+ screenWidth / 5))
        {
            newCameraPostion.x = targetObjectX - screenWidth / 5;
            transform.position = newCameraPostion;
        }else if (targetObjectX<= (newCameraPostion.x- screenWidth / 5)){
            newCameraPostion.x = targetObjectX + screenWidth / 5;
            transform.position = newCameraPostion;
        }
    }
}
