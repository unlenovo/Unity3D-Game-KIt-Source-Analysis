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
            mouseDelta = Input.mousePosition - mousePosition;//���������и�ֵ
            mousePosition = Input.mousePosition;//���¶�������λ��

            if (Input.GetKey(KeyCode.W))//��ȡW��
                velocity.z += Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 10 : 3);//�ƶ����ٶȡ���leftshift��ʲô����������
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


            if (Input.GetMouseButton(1))//��ȡ��갴��
            {//�ص㣺mouseDelta����������������ƶ�����ƽ���ƶ���ֻ��x��y�������Y���ƶ�ʱ����Ϸ���ӽ���X����ת��ͬ�������X���ƶ�����Ϸ�е��ӽ���y���ƶ�
                angles.x += mouseDelta.y;//�Ƕȵ����������//
                angles.y += mouseDelta.x;
            }

            transform.Translate(velocity * Time.deltaTime, Space.Self);//��ĳ�����ƶ�ĳ����

            var yaw = Quaternion.AngleAxis(angles.y, Vector3.up);//Var����Ԥ�Ȳ���֪�����������ͣ��������������ֵ���ж���������ʲô����
            var pitch = Quaternion.AngleAxis(angles.x, Vector3.left);
            transform.localRotation = originRotation * yaw * pitch;//Transform.localRotation �ֲ���ת//�ñ任����ת�Ƕ�����ڸ����任����ת�Ƕȡ� 


        }
    }
}
