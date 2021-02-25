using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] LayerMask mask;

    Transform cameraT;
    [SerializeField] bool isAbsMove = false;

    [SerializeField] float speed = 1;
    [SerializeField] float addSpeed = 0;
    [SerializeField] GameObject hammer;

    new Rigidbody rigidbody;

    int hp = 3;

    int hammerFrame = 0;

    void Start() {
        hp = 3;
        cameraT = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();

        Ray forwerdRay = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


            Vector3 upFV;
            if (isAbsMove) {
                upFV = cameraT.up;
            }
            else {
                upFV = forwardPoints.LeftHeight.normalized;
            }

            var look = Vector3.Slerp(transform.position + transform.forward, transform.position - forwerdHit.normal, 0.9f);

            //transform.LookAt(look, upFV);
            transform.LookAt(transform.position - forwardPoints.Normal3, upFV);
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Block") {
            hp--;
            rigidbody.AddForce((transform.position - collision.contacts[0].point).normalized * 10, ForceMode.VelocityChange);


            if (hp <= 0) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
            }
        }
    }



    void Update() {
        if (addSpeed > 0) {
            addSpeed -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            addSpeed = 3;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            isAbsMove = !isAbsMove;
        }
        if (hammerFrame > 0) {
            hammerFrame++;
            hammer.transform.RotateAround(transform.position, transform.up, hammerFrame);

            if (hammerFrame > 100) {
                hammerFrame = 0;
                hammer.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            hammerFrame++;
            hammer.SetActive(true);
            hammer.transform.RotateAround(transform.position, transform.up, 0);
        }

        Ray forwerdRay = new Ray(transform.position - transform.forward, transform.forward);

        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


            Vector3 upFV;
            if (isAbsMove) {
                upFV = cameraT.up;
            }
            else {
                upFV = forwardPoints.LeftHeight.normalized;
            }

            var befRoatte = transform.rotation;

            var look = Vector3.Slerp(transform.position + transform.forward, transform.position - forwardPoints.Normal3, 0.5f);

            transform.LookAt(look, upFV);

            var sa = Quaternion.Angle(befRoatte, transform.rotation);
            if (sa > 20) {
                transform.rotation = befRoatte;
            }
            //Debug.Log(sa);


        }
    }
    private void FixedUpdate() {


        var forwerdRay = new Ray(transform.position - transform.forward, transform.forward);
        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            Vector3 upFV, rightFV;
            if (isAbsMove) {
                upFV = cameraT.up;
                rightFV = cameraT.right;
            }
            else {
                upFV = forwardPoints.LeftHeight.normalized;
                rightFV = forwardPoints.LonggerWidth.normalized;
            }


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

            if (dir != Vector3.zero) {
                // rigidbody.AddForce(-transform.forward * 20, ForceMode.Acceleration);
                rigidbody.AddForce(dir * (speed + addSpeed), ForceMode.VelocityChange);

            }

            rigidbody.AddForce(transform.forward * 10, ForceMode.Acceleration);
        }
    }
}


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

        public Vector3 RightHeight { get { return RightTop - RightBottom; } }
        public Vector3 LeftHeight { get { return LeftTop - LeftBottom; } }
        public Vector3 TopWidth { get { return RightTop - LeftTop; } }
        public Vector3 BottomWidth { get { return RightBottom - LeftBottom; } }

        public Vector3 LonggerWidth { get { return TopWidth.sqrMagnitude > BottomWidth.sqrMagnitude ? TopWidth : BottomWidth; } }

        public Vector3 Normal1 { get ; private set; }
        public Vector3 Normal2 { get ; private set; }
        public Vector3 Normal3 { get ; private set; }
        public Quad() {
        }
        public Quad(Vector3 leftTop, Vector3 rightTop, Vector3 leftBottom, Vector3 rightBottom) {
            LeftTop = leftTop;
            RightTop = rightTop;
            LeftBottom = leftBottom;
            RightBottom = rightBottom;

            Normal1 = Vector3.Cross(RightTop - LeftTop, LeftBottom - LeftTop).normalized;
            Normal2 = Vector3.Cross( LeftBottom - RightBottom, RightTop - RightBottom).normalized;
            Normal3 = (Normal1 + Normal2).normalized;
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

        Debug.DrawLine(v.LeftTop, v.RightTop, Color.red);
        Debug.DrawLine(v.RightTop, v.RightBottom, Color.blue);
        Debug.DrawLine(v.RightBottom, v.LeftBottom, Color.green);
        Debug.DrawLine(v.LeftBottom, v.LeftTop, Color.cyan);
        return v;
    }
};