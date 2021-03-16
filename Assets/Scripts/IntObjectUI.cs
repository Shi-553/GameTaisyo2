using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntObjectUI : MonoBehaviour
{
    [SerializeField] Vector3 firstPos;
    [SerializeField] Vector3 nextDiff;
     List<GameObject> objects = new List<GameObject>();
    [SerializeField] GameObject objectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Set(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Remove();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Add();
        }
    }
    public void Add()
    {
        objects.Add(Instantiate(objectPrefab, transform.position+firstPos + (nextDiff * objects.Count), Quaternion.identity));
    }
    public void Remove()
    {

        Destroy(objects[objects.Count - 1]);
        objects.RemoveAt(objects.Count-1);
        
    }
    public void Clear()
    {
        foreach (var obj in objects)
        {
            Destroy(obj);
           
        }
        objects.Clear();
    }
    public void Set(int num)
    {
        Clear();
        for (int i =0;i< num; i++)
        {

            Add();
        }
    }
}
