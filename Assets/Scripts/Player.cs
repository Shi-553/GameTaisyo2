using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] LayerMask mask;

    Transform cameraT;
    public bool isAbsMove = false;

    public float speed = 1;

    void Start() {

        cameraT = Camera.main.transform;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <returns>0=底辺 1=高さ 2=斜辺</returns>
    public PointDistance[] GetUpRight(RaycastHit hit, Vector3 up, Vector3 right) {
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (meshCollider == null || meshCollider.sharedMesh == null) {
            return new PointDistance[] { };
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

        Debug.DrawLine(order[0].P1, order[0].P2, Color.red);
        Debug.DrawLine(order[1].P1, order[1].P2, Color.blue);
        Debug.DrawLine(order[2].P1, order[2].P2, Color.green);
        return order;
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            isAbsMove = !isAbsMove;
        }

        Ray forwerdRay = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

            var forwardPoints = GetUpRight(forwerdHit, transform.up, transform.right);


            Vector3 upFV, rightFV;
            if (isAbsMove) {
                upFV = cameraT.up;
                rightFV = cameraT.right;
            }
            else {
                upFV = forwardPoints[1].NomalD;
                rightFV = forwardPoints[0].NomalD;
            }

            //hitT.position = hit.point;
            var dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) {
                dir += upFV;
            }
            if (Input.GetKey(KeyCode.A)) {
                dir += -rightFV;
            }
            if (Input.GetKey(KeyCode.S)) {
                dir += -upFV;

            }
            if (Input.GetKey(KeyCode.D)) {
                dir += rightFV;

            }
            dir.Normalize();


            transform.position += Vector3.Slerp(Vector3.zero,forwerdHit.normal*(1-forwerdHit.distance),0.1f);


            if (dir != Vector3.zero) {
            dir = Vector3.ProjectOnPlane(dir, forwerdHit.normal).normalized;
                // Debug.Log(dir);
                transform.position += dir * speed / 50;
            }


            var look = Vector3.Slerp(transform.position + transform.forward, transform.position - forwerdHit.normal, 0.5f);

            transform.LookAt(look, upFV);

        }
    }
}


public class PointDistance {
    public Vector3 Distance { get; private set; }
    public Vector3 NomalD{ get ; private set ; }
    public Vector3 P1 { get ; private set ; }
    public Vector3 P2 { get; private set; }

    public bool IsChange { get ; private set ; }

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
};