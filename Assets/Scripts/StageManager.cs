using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageManager : SingletonMonoBehaviour<StageManager> {

    int currentStar = 0;
    [SerializeField] bool isSetSubMesh = false;

    void Start() {
        currentStar = 0;



        //Mesh mesh = GameObject.Find("mebiusu").GetComponent<MeshFilter>().sharedMesh;
        //int[] triangles = mesh.triangles;
        //mesh.subMeshCount = 2;

        //mesh.SetTriangles(triangles, 0);
        //mesh.SetTriangles(new int[] { }, 1);

    }

    // Update is called once per frame
    void Update() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (isSetSubMesh && Input.GetMouseButton(0) && Physics.Raycast(ray, out var hit,Mathf.Infinity)) {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            Mesh mesh = meshCollider.sharedMesh;
            int[] triangles = mesh.triangles;


            var t = mesh.GetTriangles(1).ToList();

            t.Add(triangles[hit.triangleIndex * 3 + 0]);
            t.Add(triangles[hit.triangleIndex * 3 + 1]);
            t.Add(triangles[hit.triangleIndex * 3 + 2]);
            mesh.SetTriangles(t, 1);
            
        }
    }
    void AddStar() {
        currentStar++;
    }
}
