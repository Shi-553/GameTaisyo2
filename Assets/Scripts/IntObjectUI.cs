using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntObjectUI : MonoBehaviour
{
    [SerializeField] Vector3 firstPos;
    [SerializeField] Vector3 nextDiff;
     List<GameObject> objects = new List<GameObject>();
    [SerializeField]
    int current=0;
    [SerializeField] int max=3;

    [SerializeField] GameObject objectPrefab;
    [SerializeField] GameObject removeObjectPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        SetMax(max,current);
    }

    GameObject GetObject(GameObject prefab,int num) {
        return Instantiate(prefab, transform.position + firstPos + (nextDiff * num), Quaternion.identity);
    }
    public void Add(int addCount=1)
    {
        var addedCurrent = current + addCount;
        if (addedCurrent > max) {
            addedCurrent = max;
        }
        for (int i = current; i < addedCurrent; i++) {
            Destroy(objects[i]);
            objects[i] = GetObject(objectPrefab, i);
            objects[i].transform.SetParent(transform);
        }
        current = addedCurrent;
    }
    public void Remove(int removeCount=1) {
        var removedCurrent = current - removeCount;
        if (removedCurrent < 0) {
            removedCurrent = 0;
        }
        for (int i = removedCurrent; i < current; i++) {
            Destroy(objects[i]);
            objects[i] = GetObject(removeObjectPrefab, i);
            objects[i].transform.SetParent(transform);
        }
        current = removedCurrent;
    }
    public void Clear()
    {
        for (int i = 0; i < objects.Count; i++) {
            Destroy(objects[i]);
        }
        objects.Clear();
    }
    public void SetMax(int m,int c)
    {
        max = m;
        current = c;
        Clear();
        for (int i =0;i< c; i++) {
            var obj = GetObject(objectPrefab, i);
            obj.transform.SetParent(transform);
            objects.Add(obj);
        }
        for (int i = c; i < m; i++) {
            var obj = GetObject(removeObjectPrefab, i);
            obj.transform.SetParent(transform);
            objects.Add(obj);
        }
    }
}
