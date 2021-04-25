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
    public int P1Index { get; private set; }
    public int P2Index { get; private set; }

    public bool IsChange { get; private set; }

    public PointDistance(Vector3 p1, Vector3 p2, int p1Index, int p2Index) {
        Init(p1, p2, p1Index, p2Index);
    }
    public void Init(Vector3 p1, Vector3 p2, int p1Index, int p2Index) {
        this.Distance = p1 - p2;
        NomalD = Distance.normalized;
        this.P1 = p1;
        this.P2 = p2;
        this.P1Index = p1Index;
        this.P2Index = p2Index;
        IsChange = false;
    }
    public void DotAdjust(Vector3 v) {
        if (Vector3.Dot(NomalD, v) < 0) {
            Distance = -Distance;
            NomalD = -NomalD;
            var t = P1;
            P1 = P2;
            P2 = t;

            var i = P1Index;
            P1Index = P2Index;
            P2Index = i;
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
                return TopWidth* (1 - TopRatio)+ BottomWidth  * TopRatio ;
            }
        }
        public Vector3 Up {
            get {
                return LeftHeight* (1 - LeftRatio)  + RightHeight* LeftRatio ;
            }
        }
        public Vector3 Normal1 { get; private set; }
        public Vector3 Normal2 { get; private set; }
        public Vector3 Normal { get; private set; }
        public float LeftRatio { get; private set; }
        public float TopRatio { get; private set; }

        public void SetPoint(Vector3 point) {
            var leftCenter = (LeftTop + LeftBottom) / 2;
            var rightCenter = (RightTop + RightBottom) / 2;
            var topCenter = (RightTop + LeftTop) / 2;
            var bottomCenter = (RightBottom + LeftBottom) / 2;

            var leftCenterDistance = Vector3.Distance(point, leftCenter);
            var rightCenterDistance = Vector3.Distance(point, rightCenter);
            var topCenterDistance = Vector3.Distance(point, topCenter);
            var bottomCenterDistance = Vector3.Distance(point, bottomCenter);

            LeftRatio = leftCenterDistance / (leftCenterDistance + rightCenterDistance);
            TopRatio = topCenterDistance / (topCenterDistance + bottomCenterDistance);

            //LeftRatio = 0.5f;
           // TopRatio = 0.5f;
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

        int[] indexs = { triangles[hit.triangleIndex * 3 + 0],
        triangles[hit.triangleIndex * 3 + 1],
        triangles[hit.triangleIndex * 3 + 2]};

        Vector3[] ps = indexs.Select(i => vertices[i]).ToArray();


        Transform hitTransform = hit.collider.transform;

        for (var i = 0; i < ps.Length; i++) {
            ps[i] = hitTransform.TransformPoint(ps[i]);
        }

        PointDistance[] ds = { 
            new PointDistance(ps[0], ps[1],indexs[0],indexs[1]),
            new PointDistance(ps[1], ps[2],indexs[1],indexs[2]),
            new PointDistance(ps[0], ps[2],indexs[0],indexs[2]) };

        //短いの順にするから底辺、高さ、斜辺の順番になる
        var ordered = ds.OrderBy(d => d.Distance.sqrMagnitude).ToArray();

        //p1が左か上、p2が右か下になるように変換
        ordered[0].DotAdjust(right);
        ordered[1].DotAdjust(up);

        //{斜辺１,斜辺２,それ以外の点}　の順にならべる
        int[] ins ={
            ordered[2].P1Index,
            ordered[2].P2Index,
            indexs.FirstOrDefault(i => i != ordered[2].P1Index && i != ordered[2].P2Index)
                };

        //三角を四角の一部として考えて残った点をもとめる
        var otherIndex = -1;

        for (int i = 0; i < triangles.Length; i++) {
            // 1 x 2　か 2 x 1　のとき
            if (triangles[i] == ins[0] && triangles[i + 2] == ins[1] ||
                triangles[i] == ins[1] && triangles[i + 2] == ins[0]) {
                if (!ins.Contains(triangles[i + 1])) {
                    otherIndex = triangles[i + 1];
                    break;
                }
            }
            // 1 2 x　か 2 1 x　か
            // x 1 2　か x 2 1　のとき
            if (triangles[i] == ins[0] && triangles[i + 1] == ins[1] ||
                triangles[i] == ins[1] && triangles[i + 1] == ins[0]) {
                if (!ins.Contains(triangles[i + 2])) {
                    otherIndex = triangles[i + 2];
                    break;
                }
                if (!ins.Contains(triangles[i - 1])) {
                    otherIndex = triangles[i - 1];
                    break;
                }
            }
        }

        if (otherIndex == -1) {
            throw new Exception("other point not found");
        }

        var otherV = vertices[otherIndex];
        otherV = hitTransform.TransformPoint(otherV);

        Quad v;

        if (ordered[0].P1Index == ordered[1].P2Index) {
            v = new Quad(otherV, ordered[1].P1, ordered[0].P2, ordered[0].P1);
        }
        else /*if(order[0].P2 == order[1].P1)*/ {
            v = new Quad(ordered[0].P2, ordered[0].P1, ordered[1].P2, otherV);
        }

        v.SetPoint(hit.point);

        Debug.DrawLine(v.LeftTop, v.RightTop, Color.red);
        Debug.DrawLine(v.RightTop, v.RightBottom, Color.blue);
        Debug.DrawLine(v.RightBottom, v.LeftBottom, Color.green);
        Debug.DrawLine(v.LeftBottom, v.LeftTop, Color.cyan);


        return v;
    }


};