using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PointDistance {
    public Vector3 Distance { get; private set; }
    public Vector3 NomalD { get; private set; }
    public Vector3 P1 { get; private set; }
    public Vector3 P2 { get; private set; }

    public bool IsChange { get; private set; }

    public PointDistance(Vector3 p1, Vector3 p2) {
        Init(p1, p2);
    }
    public void Init(Vector3 p1, Vector3 p2) {
        this.Distance = p1 - p2;
        NomalD = Distance.normalized;
        this.P1 = p1;
        this.P2 = p2;
        IsChange = false;
    }
    public void DotAdjust(Vector3 v) {
        if (Vector3.Dot(NomalD, v) < 0) {
            Distance = -Distance;
            NomalD = -NomalD;
            var t = P1;
            P1 = P2;
            P2 = t;
            IsChange = !IsChange;
        }
    }
    public class Quad {


        public Vector3 LeftTop { get; private set; }
        public Vector3 RightTop { get; private set; }
        public Vector3 LeftBottom { get; private set; }
        public Vector3 RightBottom { get; private set; }

        public Vector3 LeftHeight { get { return LeftTop - LeftBottom; } }
        public Vector3 RightHeight { get { return RightTop - RightBottom; } }
        public Vector3 TopWidth { get { return RightTop - LeftTop; } }
        public Vector3 BottomWidth { get { return RightBottom - LeftBottom; } }

        public Vector3 Right {
            get {
                return TopWidth * TopRatio + BottomWidth * (1 - TopRatio);
            }
        }
        public Vector3 Up {
            get {
                return LeftHeight * LeftRatio + RightHeight * (1 - LeftRatio);
            }
        }
        public Vector3 Normal1 { get; private set; }
        public Vector3 Normal2 { get; private set; }
        public Vector3 Normal { get; private set; }
        public float LeftRatio { get; private set; }
        public float TopRatio { get; private set; }

        public void SetPoint(Vector3 point) {
            var leftCenter = LeftTop + LeftBottom / 2;
            var rightCenter = RightTop + RightBottom / 2;
            var topCenter = RightTop + LeftTop / 2;
            var bottomCenter = RightBottom + LeftBottom / 2;

            var leftCenterDistance = Vector3.Distance(point, leftCenter);
            var rightCenterDistance = Vector3.Distance(point, rightCenter);
            var topCenterDistance = Vector3.Distance(point, topCenter);
            var bottomCenterDistance = Vector3.Distance(point, bottomCenter);

            LeftRatio = leftCenterDistance / (leftCenterDistance + rightCenterDistance);
            TopRatio = topCenterDistance / (topCenterDistance + bottomCenterDistance);
            LeftRatio = 0.5f;
            TopRatio = 0.5f;
        }
        public Quad() {
        }
        public Quad(Vector3 leftTop, Vector3 rightTop, Vector3 leftBottom, Vector3 rightBottom) {
            LeftTop = leftTop;
            RightTop = rightTop;
            LeftBottom = leftBottom;
            RightBottom = rightBottom;

            Normal1 = Vector3.Cross(RightTop - LeftTop, LeftBottom - LeftTop).normalized;
            Normal2 = Vector3.Cross(LeftBottom - RightBottom, RightTop - RightBottom).normalized;
            Normal = (Normal1 + Normal2).normalized;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Quad GetUpRight(RaycastHit hit, Vector3 up, Vector3 right) {
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (meshCollider == null || meshCollider.sharedMesh == null) {
            return new Quad();
        }

        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Vector3[] ps = { vertices[triangles[hit.triangleIndex * 3 + 0]]
            ,vertices[triangles[hit.triangleIndex * 3 + 1]]
            ,vertices[triangles[hit.triangleIndex * 3 + 2]]};


        Transform hitTransform = hit.collider.transform;

        for (var i = 0; i < ps.Length; i++) {
            ps[i] = hitTransform.TransformPoint(ps[i]);
        }

        PointDistance[] ds = { new PointDistance(ps[0], ps[1]), new PointDistance(ps[1], ps[2]), new PointDistance(ps[0], ps[2]) };
        var order = ds.OrderBy(d => d.Distance.sqrMagnitude).ToArray();


        order[0].DotAdjust(right);
        order[1].DotAdjust(up);

        var index = Array.FindIndex(ps, p => order[0].P1 != p && order[0].P2 != p);
        var otherV = vertices[triangles[hit.triangleIndex * 3 + index] + 1];
        otherV = hitTransform.TransformPoint(otherV);

        Quad v;

        if (order[0].P1 == order[1].P2) {
            v = new Quad(order[1].P1, otherV, order[0].P2, order[0].P1);
        }
        else /*if(order[0].P2 == order[1].P1)*/ {
            v = new Quad(order[0].P2, order[0].P1, otherV, order[1].P2);
        }
        v.SetPoint(hit.point);

        Debug.DrawLine(v.LeftTop, v.RightTop, Color.red);
        Debug.DrawLine(v.RightTop, v.RightBottom, Color.blue);
        Debug.DrawLine(v.RightBottom, v.LeftBottom, Color.green);
        Debug.DrawLine(v.LeftBottom, v.LeftTop, Color.cyan);
        return v;
    }


};