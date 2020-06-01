using System.Collections;//包括了.NET下的非泛型集合类以及非泛型接口等 https://blog.csdn.net/sibaison/article/details/66477716
using System.Collections.Generic;//命名空间System.Collections.Generic 中包含了一些基于泛型的集合类，使用泛型集合类可以提供更高的类型安全性，还有更高的性能，避免了非泛型集合的重复的装箱和拆箱。 https://blog.csdn.net/smalltt/article/details/3384737?utm_medium=distribute.pc_relevant.none-task-blog-BlogCommendFromMachineLearnPai2-1.nonecase&depth_1-utm_source=distribute.pc_relevant.none-task-blog-BlogCommendFromMachineLearnPai2-1.nonecase
using UnityEngine;
using Cinemachine;//本插件主要用于做相机控制 https://blog.csdn.net/lichoueve/article/details/88857674

namespace Gamekit3D
{
    public class CameraShake : MonoBehaviour
    {
        static protected List<CameraShake> s_Cameras = new List<CameraShake>();//List<T>T为列表中的类型，声明s_Cameras 为CameraShake类型
        //CameraShake Unity中用来抖动相机的插件
        public const float k_PlayerHitShakeAmount = 0.05f;//玩家摇动的数量
        public const float k_PlayerHitShakeTime = 0.4f;//摇动的时间

        protected float m_ShakeAmount;
        protected float m_RemainingShakeTime;

        protected CinemachineVirtualCameraBase m_CinemachineVCam;//VirtualCameraBase计算一个CameraState（包含了位置、旋转、视角、额外偏移值等数据）//可能相当于一个高级的Transform类，因为41用到了lookat的用法，并且VirtualCameraBase的用法和transform很像。？？
        protected bool m_IsShaking = false;//是否摇动
        protected Vector3 m_OriginalLocalPosition;//原始位置

        private void Awake()//附个组件
        {
            m_CinemachineVCam = GetComponent<CinemachineVirtualCameraBase>();
        }

        private void OnEnable()
        {
            s_Cameras.Add(this);//添加这个相机？？？？？？？
        }

        private void OnDisable()
        {
            s_Cameras.Remove(this);//删除这个相机？？？？
        }

        private void LateUpdate()
        {
            if (m_IsShaking)
            {
                m_CinemachineVCam.LookAt.localPosition = m_OriginalLocalPosition + Random.insideUnitSphere * m_ShakeAmount;//Random.insideUnitSphere 随机返回半径为1的球体内的一个点（只读）。（手册中的） 
                //抖动相机看的方向的距离
                m_RemainingShakeTime -= Time.deltaTime;//以秒计算，完成最后一帧的时间（只读）。 
                if (m_RemainingShakeTime <= 0)
                {
                    m_IsShaking = false;
                    m_CinemachineVCam.LookAt.localPosition = m_OriginalLocalPosition;//抖动相机回到原来的位置
                }
            }
        }

        private void StartShake(float amount, float time)
        {
            if (!m_IsShaking)
            {
                m_OriginalLocalPosition = m_CinemachineVCam.LookAt.localPosition;
            }

            m_IsShaking = true;
            m_ShakeAmount = amount;
            m_RemainingShakeTime = time;
        }

        static public void Shake(float amount, float time)
        {
            for (int i = 0; i < s_Cameras.Count; ++i)
            {
                s_Cameras[i].StartShake(amount, time);

            }
        }

        void StopShake ()//停止抖动
        {
            m_OriginalLocalPosition = m_CinemachineVCam.LookAt.localPosition;
            m_IsShaking = false;
            m_ShakeAmount = 0f;
            m_RemainingShakeTime = 0f;
        }

        public static void Stop ()
        {
            for (int i = 0; i < s_Cameras.Count; i++)
            {
                s_Cameras[i].StopShake ();
            }
        }
    }

}//不是很了解这具体是哪个部分？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？