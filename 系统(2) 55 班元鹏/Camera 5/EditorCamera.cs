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
            mouseDelta = Input.mousePosition - mousePosition;//对增量进行赋值
            mousePosition = Input.mousePosition;//重新定义鼠标的位置

            if (Input.GetKey(KeyCode.W))//获取W键
                velocity.z += Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 10 : 3);//移动的速度。。leftshift是什么？？？？？
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


            if (Input.GetMouseButton(1))//获取鼠标按键
            {//重点：mouseDelta是鼠标的增量，鼠标移动是是平面移动，只有x和y，鼠标向Y轴移动时，游戏中视角绕X轴旋转，同理鼠标向X轴移动，游戏中的视角绕y轴移动
                angles.x += mouseDelta.y;//角度等于鼠标增量//
                angles.y += mouseDelta.x;
            }

            transform.Translate(velocity * Time.deltaTime, Space.Self);//向某方向移动某物体

            var yaw = Quaternion.AngleAxis(angles.y, Vector3.up);//Var类型预先不用知道变量的类型；根据你给变量赋值来判定变量属于什么类型
            var pitch = Quaternion.AngleAxis(angles.x, Vector3.left);
            transform.localRotation = originRotation * yaw * pitch;//Transform.localRotation 局部旋转//该变换的旋转角度相对于父级变换的旋转角度。 


        }
    }
}
