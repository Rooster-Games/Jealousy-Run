using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using System.Linq;
//using PathCreation;

public class Road : MonoBehaviour
{
    public CinemachineSmoothPath targetSmoothPath;
    public CinemachinePath cinemachinePath;
    [SerializeField]
    public Transform pathStartPos;
    public MeshFilter roadMesh;

    public float distanceBetweenNodes = 1.5f;
    [SerializeField]
    float simplifyDistance = 0.1f;

    public List<Vector3> path = new List<Vector3>();

    [SerializeField]
    GameObject cube;

    void FindMiddlePoints()
    {
        path.Clear();
        gizmoPoses.Clear();

        Vector3 currentMiddlePos = pathStartPos.position;
        List<Vector3> poses = new List<Vector3>();
        List<Vector3> tempPathPoses = new List<Vector3>();
        List<Vector3> pathPoses = new List<Vector3>();
        Matrix4x4 localToWorld = roadMesh.transform.localToWorldMatrix;
        for (int i = 0; i < roadMesh.sharedMesh.vertices.Length; i++)
        {
            Vector3 pos;
            pos = localToWorld.MultiplyPoint3x4(roadMesh.sharedMesh.vertices[i]);
            poses.Add(pos);

        }

        gizmoPoses.AddRange(poses);

        while (poses.Count > 0)
        {
          
            List<Vector3> points = new List<Vector3>();
            for(int k = 0; k < 2; k++)
            {
                float distanceTreshold = 2000;
                int indexOfClosest = 0;
                for (int i = 0; i < poses.Count; i++)
                {
                    float distance = Vector3.Distance(poses[i], currentMiddlePos);

                    if (distance < distanceTreshold)
                    {
                        indexOfClosest = i;
                        distanceTreshold = distance;
                    }
                }
      
                 points.Add(poses[indexOfClosest]);

                 poses.Remove(poses[indexOfClosest]);
                
            }

            if(points.Count > 1)
            {
                currentMiddlePos = (points[0] + points[1]) * 0.5f;
                tempPathPoses.Add(currentMiddlePos);
            }
            else
            {
                currentMiddlePos = points[0];
                tempPathPoses.Add(currentMiddlePos);
            }
     
        }

        path.AddRange(tempPathPoses);

    }

    public void CreateSmoothPath()
    {
        if (targetSmoothPath == null)
        {
            Debug.LogError("Please assign a CinemachineSmoothPath!!");
            return;
        }

        FindMiddlePoints();

        List<Vector3> smoothPath = new List<Vector3>();
        int index = 0;
        smoothPath.Add(path[index]);
        Vector3 pos = smoothPath[0];
        //

        for (int i = 0; i < path.Count - 1; i++)
        {

            Vector3 direction = (path[i + 1] - pos).normalized;
            if (Vector3.Distance(pos, path[i + 1]) < distanceBetweenNodes)
            {
                pos = path[i];

                smoothPath.Add(pos);

                index++;
            }
            else
            {
                while (Vector3.Distance(pos, path[i + 1]) > distanceBetweenNodes)
                {
                    smoothPath.Add(pos);

                    pos = pos + direction * distanceBetweenNodes;

                }
            }
        }
        path.Clear();
        path.AddRange(smoothPath);

        path = SimplifyPath(path);

        CinemachineSmoothPath.Waypoint[] waypoints = new CinemachineSmoothPath.Waypoint[path.Count];

        for (int i = 0; i < path.Count; i++)
        {

            
            waypoints[i].position = path[i];
            //float tan = Mathf.Atan2(waypoints[i].position.x, waypoints[i].position.z);

            if (i < path.Count - 1)
            {

                //waypoints[i].roll = Mathf.Tan(Vector3.SignedAngle(waypoints[i + 1].position, waypoints[i].position, Vector3.up));
            }
            else
            {

                //waypoints[i].roll = 0;
            }

            //waypoints[i].roll = Mathf.Tan(Vector3.SignedAngle(waypoints[i+1].position, waypoints[i].position,Vector3.up));


        }

        targetSmoothPath.m_Waypoints = waypoints;

        //CinemachinePath.Waypoint[] waypoints2 = new CinemachinePath.Waypoint[path.Count];

        //for (int i = 0; i < path.Count; i++)
        //{
        //    waypoints2[i].position = path[i];

        //    if(i < path.Count - 1)
        //    {
        //        Vector3 dir = new Vector3(waypoints2[i + 1].position.x, waypoints2[i + 1].position.y, waypoints2[i + 1].position.z) - new Vector3(waypoints2[i].position.x, waypoints2[i + 1].position.y, waypoints2[i].position.z);
        //        waypoints2[i].tangent = Vector3.(dir);
        //    }
        //    else
        //    {
        //        waypoints2[i].tangent = Vector3.zero;
        //    }
           

        //}
        //cinemachinePath.m_Waypoints = waypoints2;


    }

    List<Vector3> SimplifyPath(List<Vector3> path)
    {
        List<Vector3> returnList = new List<Vector3>();

        for(int i = 0; i < path.Count - 1; i++)
        {
            float dist = Vector3.Distance(path[i], path[i + 1]);

            if(dist > simplifyDistance)
            {
                returnList.Add(path[i]);
            }
        }

        return returnList;
    }

    [SerializeField]
    List<Vector3> gizmoPoses = new List<Vector3>();
    public bool showPath = false;
    [SerializeField]
    float gizmoRadius = 0.3f;
    [SerializeField]
    Color pathColor;
    [SerializeField]
    Color gizmoPathColor;
    [SerializeField]
    int currentNode = 0;
    private void OnDrawGizmos()
    {
        if (showPath)
        {
            Gizmos.color = pathColor;
            foreach (Vector3 pos in path)
            {
                Gizmos.DrawSphere(pos, gizmoRadius);
            }
            int i = 0;
            Gizmos.color = gizmoPathColor;
            foreach (Vector3 pos in gizmoPoses)
            {
                Gizmos.DrawSphere(pos, gizmoRadius * 1.1f);
                i++;
            }

            if(path.Count > 0)
            {
                Gizmos.color = Color.green;
                currentNode = Mathf.Clamp(currentNode,0, path.Count -1);
                Gizmos.DrawSphere(path[currentNode], gizmoRadius * 1.2f);
            }
     

        }
    }
}

# if UNITY_EDITOR
[CustomEditor(typeof(Road))]
public class AIPathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Road road = (Road)target;

        if (GUILayout.Button("CreatePath"))
        {
            road.CreateSmoothPath();
        }
    }
}
#endif
