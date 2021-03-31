using UnityEngine;

public class CameraCulling1 : MonoBehaviour
{
    public Transform inView;

    public Transform outView;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(TestInCamreaView1(inView.position));
        Debug.Log(TestInCamreaView1(outView.position));
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

}