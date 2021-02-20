using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count;
    public Transform tfPoolParent;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    [SerializeField] ObjectInfo[] objectInfo = null;

    public Queue<GameObject> noteQueue = new Queue<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]);
    }

    Queue<GameObject> InsertQueue(ObjectInfo p_ObjectInfo)
    {
        Queue<GameObject> t_Queue = new Queue<GameObject>();
        for(int i=0; i<p_ObjectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_ObjectInfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            if (p_ObjectInfo.tfPoolParent != null)
                t_clone.transform.SetParent(p_ObjectInfo.tfPoolParent);
            else
                t_clone.transform.SetParent(this.transform);

            t_Queue.Enqueue(t_clone);
        }

        return t_Queue;
    }
}
