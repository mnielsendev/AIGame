using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMap : MonoBehaviour
{
    public GameObject nodeObj;
    public Material LineMaterial;
    public int Depth;
    public int Amount;
    Transform nodeList;
    GameObject masterNode;

    public int seed;

    void Start()
    {
        if (seed == 0)
            seed = Random.Range(1, int.MaxValue);
        Random.InitState(seed);

        nodeList = new GameObject().transform;
        nodeList.name = "nodeList";

        CreateMasterNode();
        NodeCreator();
    }
    public static Vector3 arround(float radius)
    {
        float angle = Random.Range(0, 360);
        float a = angle / 180 * Mathf.PI;
        float x = Mathf.Cos(a) * radius;
        float y = Mathf.Sin(a) * radius;

        return new Vector3(x, y, 0);
    }

    void CreateMasterNode()
    {
        GameObject go = Instantiate(nodeObj, new Vector3(0, 0, 0), Quaternion.identity);
        go.transform.parent = nodeList;
        go.AddComponent<LineRenderer>();
        go.GetComponent<LineRenderer>().material = LineMaterial;
        go.GetComponent<LineRenderer>().positionCount = 0;
        masterNode = go;
        go.name = "Master Node";
    }

    void NodeCreator()
    {
        for(int d = 1; d <= Depth; d++)
        {
            GameObject dep = new GameObject();
            dep.name = d.ToString();
            dep.transform.parent = masterNode.transform;
            for (int i = 0; i < Amount; i++)
            {
                GameObject go = Instantiate(nodeObj, new Vector3(0,0,0), Quaternion.identity);
                go.AddComponent<LineRenderer>();
                go.GetComponent<LineRenderer>().material = LineMaterial;
                go.GetComponent<LineRenderer>().positionCount = 0;
                go.name = "d: " + d.ToString() + " id: " + i.ToString();
                go.transform.parent = dep.transform;
            }
        }
        NodeTransform();
    }

    void NodeTransform()
    {
        int id = 0;
        for(int child = 0; child < masterNode.transform.childCount; child++)
        {
            id = int.Parse(masterNode.transform.GetChild(child).name);
            for(int c = 0; c < masterNode.transform.GetChild(child).transform.childCount; c++)
            {
                masterNode.transform.GetChild(child).transform.GetChild(c).transform.position = arround((4 * id));
            }
        }
        TransformChecker();
    }

    void TransformChecker()
    {
        int id = 0;
        for(int child = 0; child < masterNode.transform.childCount; child++)
        {
            id = int.Parse(masterNode.transform.GetChild(child).name);
            for(int c = 0; c < masterNode.transform.GetChild(child).transform.childCount; c++)
            {
                for(int c2 = 0; c2 < masterNode.transform.GetChild(child).transform.childCount; c2++)
                {
                    if(c != c2)
                        if(Vector3.Distance(masterNode.transform.GetChild(child).GetChild(c).transform.position, masterNode.transform.GetChild(child).GetChild(c2).transform.position) < 3f)
                        {
                            masterNode.transform.GetChild(child).transform.GetChild(c).transform.position = arround((4 * id));
                            TransformChecker();
                        }
                }
            }
        }
        newConnection();
    }

    void newConnection(){
        GameObject parent = masterNode.gameObject;
        
        for(int id = 0; id < parent.transform.childCount; id++)
        { // 1,2,3 "circle ID
            for(int child = 0; child < parent.transform.GetChild(id).childCount; child++)
            { //0,1,2,3,4 "node ID's in circle
                if(id == 0)
                    DrawLines(parent.transform.GetChild(id).GetChild(child).gameObject, parent);
                else {
                    for(int child0 = 0; child0 < parent.transform.GetChild(id - 1).childCount; child0++)
                    {
                        GameObject c0 = parent.transform.GetChild(id - 1).GetChild(child0).gameObject;
                        GameObject c1 = parent.transform.GetChild(id).GetChild(child).gameObject;
                        if(Vector3.Distance(c1.transform.position, c0.transform.position) < 5.4f)
                        {
                            DrawLines(c1, c0);
                        }
                    }
                        CheckConnection(parent.transform.GetChild(id).GetChild(child).gameObject);
                }
            }
        }
    }
    void CheckConnection(GameObject go)
    {
       if(go.GetComponent<LineRenderer>().positionCount == 0)
        {
            GameObject tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = go.transform.position;
            for(int i = 0; i < go.transform.parent.childCount; i++)
            {
                GameObject t = go.transform.parent.GetChild(i).gameObject;
                if(t != go)
                {
                    float dist = Vector3.Distance(t.transform.position, currentPos);
                    if (dist < minDist)
                    {
                        tMin = t;
                        minDist = dist;
                    }
                }
            }
            DrawLines(go, tMin);
       }
    }
    void DrawLines(GameObject from, GameObject to)
    {
        LineRenderer lr = from.GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        for(int i = 0; i < lr.positionCount; i++)
        {
            if(lr.GetPosition(i) == to.transform.position)
                return;
        }
        lr.positionCount += 1;
        lr.SetPosition(lr.positionCount - 1, from.transform.position);
        lr.positionCount += 1;
        lr.SetPosition(lr.positionCount - 1, to.transform.position);
    }
}