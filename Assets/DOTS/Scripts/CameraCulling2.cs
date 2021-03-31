using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCulling2:MonoBehaviour
{
    public Transform inView;

    public Transform outView;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(TestCameraView2(inView.position));
        Debug.Log(TestCameraView2(outView.position));
    }

    public bool TestInCamreaView1(Vector3 pos)
    {
        Vector4[] planeInfo = new Vector4[6];
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        for (int i = 0; i < 6; i++)
        {
            Plane p = planes[i];
            //此时的normal已经为归一化的单位向量
            planeInfo[i] = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        }

        for (int i = 0; i < 6; i++)
        {
            var normal = new Vector3(planeInfo[i].x, planeInfo[i].y, planeInfo[i].z);
            var dist = planeInfo[i].w;
            //根据点到平面距离的公式，此处不加绝对值，若点在视锥体外，距离为负数
            if (Vector3.Dot(normal, pos) + dist <= 0)
            {
                return false;
            }
        }

        return true;
    }

    //这里以左右剪裁面为例，其他可以自行求证
    public bool TestCameraView2(Vector3 pos)
    {
        var camera = Camera.main;
        var farClipPlane = camera.farClipPlane;
        //w为相机远剪裁平面宽的一半
        var w = Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * farClipPlane * camera.aspect;
        //向量相加
        Vector3 vR = farClipPlane * transform.forward + w * transform.right;
        //向量相减
        Vector3 vL = farClipPlane * transform.forward - w * transform.right;
        //向量叉积，在左手坐标系遵从左手定则，右手坐标系遵从右手定则，unity为左手坐标系，所以遵从左手定则。
        Vector3 RightPlane_N = Vector3.Cross(vR, transform.up);//视椎体右平面法线
        Vector3 LeftPlane_N = Vector3.Cross(transform.up, vL);//视椎体左平面法线

        //摄像机到物体的向量
        var vect = pos - camera.transform.position;
        if (Vector3.Dot(vect, RightPlane_N) > 0 && Vector3.Dot(vect, LeftPlane_N) > 0)
        {
            return true;
        }
        return false;
    }
}
