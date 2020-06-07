using UnityEngine;
using System.IO;//C# System.IO文件操作整理
using System;
using System.Linq;//System.Linq 命名空间提供支持使用语言集成查询 (LINQ) 进行查询的类和接口。
using System.Collections.Generic;//System.Collections.Generic 命名空间包含定义泛型集合的接口和类，用户可以使用泛型集合来创建强类型集合，这种集合能提供比非泛型强类型集合更好的类型安全性和性能。

namespace Gamekit3D
{
    [RequireComponent(typeof(Camera))]
    public class ScreenshotWithAlpha : MonoBehaviour
    {
        public int UpScale = 4;
        public bool AlphaBackground = true;

        Texture2D Screenshot()
        {
            var camera = GetComponent<Camera>();
            int w = camera.pixelWidth * UpScale;//相机的宽度
            int h = camera.pixelHeight * UpScale;//相机的高度
            var rt = new RenderTexture(w, h, 32);//RenderTexture是可以被渲染的纹理。
            camera.targetTexture = rt;//描述渲染纹理
            var screenShot = new Texture2D(w, h, TextureFormat.ARGB32, false);//Texture2D 二维纹理
            var clearFlags = camera.clearFlags;//Camera.clearFlags 清除标识(可理解为背景填充)
            if (AlphaBackground)//字母背景
            {
                camera.clearFlags = CameraClearFlags.SolidColor;//CameraClearFlags.SolidColor（用单色填充背景）, 
                camera.backgroundColor = new Color(0, 0, 0, 0);//Camera.backgroundColor 背景颜色
            }
            var cameras = new List<Camera>(FindObjectsOfType<Camera>());//？？？？
            cameras.Sort((A, B) => A.depth.CompareTo(B.depth));//？？
            foreach (var c in cameras) c.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            var pixels = screenShot.GetPixels();

            for (int i = 0; i < pixels.Length; ++i)
                pixels[i].a = 1;

            screenShot.SetPixels(pixels);
            screenShot.Apply();
            camera.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);
            camera.clearFlags = clearFlags;
            return screenShot;
        }

        [ContextMenu("Capture Child Positions")]
        public void SaveChildScreenshots()
        {
            StartCoroutine(_SaveChildScreenshots());
        }

        IEnumerator<YieldInstruction> _SaveChildScreenshots()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            {
                var originP = transform.position;
                var originR = transform.rotation;
                for (var i = 0; i < transform.childCount; i++)
                {
                    var tx = transform.GetChild(i);
                    transform.position = tx.position;
                    transform.rotation = tx.rotation;
                    yield return new WaitForSeconds(1);
                    var filename = "SS-Group-" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".png";
                    File.WriteAllBytes(Path.Combine(path, filename), Screenshot().EncodeToPNG());
                    transform.position = originP;
                    transform.rotation = originR;
                }
            }
        }

        [ContextMenu("Capture Screenshot Custom")]
        public void SaveScreenshot()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filename = "SS-Custom-" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".png";
            File.WriteAllBytes(Path.Combine(path, filename), Screenshot().EncodeToPNG());
        }

        [ContextMenu("Capture Screenshot Builtin")]
        public void SaveScreenshotBuiltin()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filename = "SS-Builtin-" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".png";
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(path, filename), UpScale);
        }
    }
}