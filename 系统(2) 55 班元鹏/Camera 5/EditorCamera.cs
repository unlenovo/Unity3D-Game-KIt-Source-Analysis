using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit3D.Cameras
{
    /// <summary>
    /// A class that mimic's the Unity Editor camera.һ��ģ��ͳһ�༭��������ࡣ
    /// </summary>
    public class EditorCamera : MonoBehaviour
    {
        public Vector3 velocity;//�ٶ�
        public Vector3 angles;//�Ƕ�

        Vector3 mousePosition;//����λ��
        Vector3 mouseDelta;//��������
        Quaternion originRotation;//��Ԫ��

        void Start()
        {
            mousePosition = Input.mousePosition;//��������λ��
            originRotation = transform.localRotation;//�ֲ���ת//�ñ任����ת�Ƕ�����ڸ����任����ת�Ƕ�//��Ҫ�õ���Ԫ��
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
