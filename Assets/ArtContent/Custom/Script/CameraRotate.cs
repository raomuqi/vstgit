using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform target;//相机的目标
    private float xSpeed = 200;//x轴的旋转速度
    private float ySpeed = 150;//x轴的旋转速度
    public float yMin = -45;//y最小角度
    public float yMax = 45;//y最大角度
    public bool needDamping = true; //是否需要的阻尼
    private float damping = 5;//阻尼 
    private float x = 0;
    private float y = 0;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (target)
        {

            //使用鼠标光按钮来控制相机，调整照相机位置
            if (Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = ClampAngle(y, yMin, yMax);

            }

            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 disVector = new Vector3(0.0f, 0.0f, 0);
            Vector3 position = rotation * disVector + target.position;

            if (needDamping)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }

    //限制旋转角度
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    private void Update()
    {
        //视野放大缩小
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (GetComponent<Camera>().fieldOfView >= 30)
            {
                GetComponent<Camera>().fieldOfView--;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (GetComponent<Camera>().fieldOfView <= 80)
            {
                GetComponent<Camera>().fieldOfView++;
            }

        }

    }
}
