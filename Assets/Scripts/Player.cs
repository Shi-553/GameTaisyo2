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


            Vector3 upFV, rightFV;
            if (isAbsMove) {
                upFV = cameraT.up;
                rightFV = cameraT.right;
            }
            else {
                upFV = forwardPoints[1].NomalD;
                rightFV = forwardPoints[0].NomalD;
            }

            var look = Vector3.Slerp(transform.position + transform.forward, transform.position - forwerdHit.normal, 0.9f);

            //transform.LookAt(look, upFV);
            transform.LookAt(transform.position - forwerdHit.normal, upFV);
        }
        }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Block") {
            hp--;
            rigidbody.AddForce((transform.position - collision.contacts[0].point).normalized*10, ForceMode.VelocityChange);


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
                upFV = forwardPoints[1].NomalD;
            }

            var befRoatte = transform.rotation;

            var look = Vector3.Slerp(transform.position + transform.forward, transform.position - forwerdHit.normal, 0.5f);

            transform.LookAt(look, upFV);

            var sa = Quaternion.Angle(befRoatte, transform.rotation);
            if (sa > 20) {
                transform.rotation = befRoatte;
            }
            //Debug.Log(sa);


        }
    }
    private void FixedUpdate() {


       var  forwerdRay = new Ray(transform.position - transform.forward, transform.forward);
        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            Vector3 upFV, rightFV;
            if (isAbsMove) {
                upFV = cameraT.up;
                rightFV = cameraT.right;
            }
            else {
                upFV = forwardPoints[1].NomalD;
                rightFV = forwardPoints[0].NomalD;
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

            rigidbody.AddForce(transform.forward * ( forwerdHit.distance* forwerdHit.distance), ForceMode.Acceleration);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <returns>0=底辺 1=高さ 2=斜辺</returns>
    public static PointDistance[] GetUpRight(RaycastHit hit, Vector3 up, Vector3 right) {
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
};