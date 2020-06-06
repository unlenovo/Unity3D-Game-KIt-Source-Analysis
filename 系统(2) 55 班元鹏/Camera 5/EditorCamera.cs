using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit3D.Cameras
{
    /// <summary>
    /// A class that mimic's the Unity Editor camera.一个模仿统一编辑器相机的类。
    /// </summary>
    public class EditorCamera : MonoBehaviour
    {
        public Vector3 velocity;//速度
        public Vector3 angles;//角度

        Vector3 mousePosition;//鼠标的位置
        Vector3 mouseDelta;//鼠标的增量
        Quaternion originRotation;//四元数

        void Start()
        {
            mousePosition = Input.mousePosition;//输入鼠标的位置
            originRotation = transform.localRotation;//局部旋转//该变换的旋转角度相对于父级变换的旋转角度//需要用到四元数
        }

        void Update()
        {
            mouseDelta = Input.mousePosition - mousePosition;
            mousePosition = Input.mousePosition;

            if (Input.GetKey(KeyCode.W))
                velocity.z += Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 10 : 3);
            else if (Input.GetKey(KeyCode.S))
                velocity.z -= Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 10 : 3);
            else
                velocity.z *= Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
                velocity.x -= Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 10 : 3);
            else if (Input.GetKey(KeyCode.D))
                velocity.x += Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 10 : 3);
            else
                velocity.x *= Time.deltaTime;


            if (Input.GetMouseButton(1))
            {
                angles.x += mouseDelta.y;
                angles.y += mouseDelta.x;
            }

            transform.Translate(velocity * Time.deltaTime, Space.Self);

            var yaw = Quaternion.AngleAxis(angles.y, Vector3.up);
            var pitch = Quaternion.AngleAxis(angles.x, Vector3.left);
            transform.localRotation = originRotation * yaw * pitch;

        }
    }
}
