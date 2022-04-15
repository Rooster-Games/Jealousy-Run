using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using System.Linq;

namespace RGLevel
{
    public class RoadFromMesh : MonoBehaviour
    {
        [Header("Roads")]
        [SerializeField] private List<Vector3> _path = new List<Vector3>();
       

        [Header("Road Config")]
        [SerializeField] private Transform pathStartPos;
        [SerializeField] private MeshFilter roadMesh;
        [SerializeField] private float distanceBetweenNodes = 1.5f;
        [SerializeField] private float simplifyDistance = 0.1f;

        [Header("Mesut Settings")]
        [SerializeField] bool _debugMode;

        private GameObject CreateGameObjectAt(Vector3 pos, Transform parent, string name, Color color)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = pos;
            go.transform.localScale = Vector3.one * 0.2f;

            if(parent != null)
                go.transform.SetParent(parent);

            go.GetComponent<Renderer>().material.color = color;
            return go;
        }

        void FindMiddlePoints()
        {
            _path.Clear();
            gizmoPoses.Clear();

            Vector3 currentMiddlePos = pathStartPos.position;
            List<Vector3> poses = new List<Vector3>();
            List<Vector3> tempPathPoses = new List<Vector3>();
            Matrix4x4 localToWorld = roadMesh.transform.localToWorldMatrix;

            Transform parent = null;
            if (_debugMode)
            {
                parent = CreateGameObjectAt(Vector3.zero, transform, "parent", Color.white).transform;
            }

            for (int i = 0; i < roadMesh.sharedMesh.vertices.Length; i++)
            {
                Vector3 pos;
                pos = localToWorld.MultiplyPoint3x4(roadMesh.sharedMesh.vertices[i]);

                if (!poses.Contains(pos))
                {
                    if(_debugMode)
                        CreateGameObjectAt(pos, parent, "point_"+i , Color.red);
                    poses.Add(pos);
                }
            }

            gizmoPoses.AddRange(poses);

            List<Vector3> testList = new List<Vector3>();
            testList.AddRange(poses);

            List<int> checkedIndexList = new List<int>();

            int lastestCount = testList.Count;

            int u = 0;
            while (testList.Count > 0)
            {
                List<Vector3> points = new List<Vector3>();
                for (int k = 0; k < 2; k++) // iki tane bulmak icin
                {
                    int indexOfClosest = 0;
                    float closestDistance = float.PositiveInfinity;

                    for (int i = 0; i < poses.Count; i++)
                    {
                        bool skipThis = false;

                        for (int j = 0; j < checkedIndexList.Count; j++)
                        {
                            skipThis = checkedIndexList[j] == i;
                            if (skipThis) break;
                        }

                        if (skipThis) continue;

                        float distance = Vector3.Distance(poses[i], currentMiddlePos);
                        if (distance < closestDistance)
                        {
                            indexOfClosest = i;
                            closestDistance = distance;
                        }
                    }

                    points.Add(poses[indexOfClosest]);
                    testList.Remove(poses[indexOfClosest]);
                    checkedIndexList.Add(indexOfClosest);
                }

                if (points.Count > 1)
                {
                    currentMiddlePos = (points[0] + points[1]) * 0.5f;
                    tempPathPoses.Add(currentMiddlePos);

                    if(_debugMode)
                        CreateGameObjectAt(currentMiddlePos, parent, "middle_" + u, Color.blue);
                }
                else
                {
                    currentMiddlePos = points[0];
                    tempPathPoses.Add(currentMiddlePos);
                }

                if (testList.Count == 1)
                {
                    break;
                }

                if(testList.Count == lastestCount)
                {
                    Debug.Log("biryanlislikvar");
                    break;
                }
                u++;
            }

            _path.AddRange(tempPathPoses);

        }

        public void CreateSmoothPath()
        {
            FindMiddlePoints();

            List<Vector3> smoothPath = new List<Vector3>();
            int index = 0;
            smoothPath.Add(_path[index]);
            Vector3 pos = smoothPath[0];
            //

            for (int i = 0; i < _path.Count - 1; i++)
            {

                Vector3 direction = (_path[i + 1] - pos).normalized;
                if (Vector3.Distance(pos, _path[i + 1]) < distanceBetweenNodes)
                {
                    pos = _path[i];

                    smoothPath.Add(pos);

                    index++;
                }
                else
                {
                    while (Vector3.Distance(pos, _path[i + 1]) > distanceBetweenNodes)
                    {
                        smoothPath.Add(pos);

                        pos = pos + direction * distanceBetweenNodes;
                    }
                }
            }

            _path.Clear();
            _path.AddRange(smoothPath);

            _path = SimplifyPath(_path);

            CinemachineSmoothPath.Waypoint[] waypoints = new CinemachineSmoothPath.Waypoint[_path.Count];

            for (int i = 0; i < _path.Count; i++)
            {

                waypoints[i].position = _path[i];

                if (i < _path.Count - 1)
                {

                }
                else
                {

                }
            }

            GameObject go = new GameObject(" -- LevelPath -- " + roadMesh.gameObject.name);
            go.transform.position = Vector3.zero;

            if (transform.parent != null)
                go.transform.SetParent(transform.parent);

            LevelPath levelPath = go.AddComponent<LevelPath>();
            CinemachineSmoothPath cinemachineSmoothPath = go.AddComponent<CinemachineSmoothPath>();
            levelPath.SmoothPath = cinemachineSmoothPath;

            levelPath.Path = _path;

            if (levelPath.SmoothPath)
                levelPath.SmoothPath.m_Waypoints = waypoints;
        }

        List<Vector3> SimplifyPath(List<Vector3> path)
        {
            List<Vector3> returnList = new List<Vector3>();

            for (int i = 0; i < path.Count - 1; i++)
            {
                float dist = Vector3.Distance(path[i], path[i + 1]);

                if (dist > simplifyDistance)
                {
                    returnList.Add(path[i]);
                }
            }

            return returnList;
        }


        [Header("Nodes Gizmo")]
        [SerializeField]
        List<Vector3> gizmoPoses = new List<Vector3>();

        [SerializeField] private bool _showPath = false;

        [SerializeField] private float _gizmoRadius = 0.3f;

        [SerializeField] private Color _pathColor = Color.blue;

        [SerializeField] private Color _gizmoPathColor = Color.yellow;

        [SerializeField] private int _currentNode = 0;

        private void OnDrawGizmos()
        {
            if (_showPath)
            {
                Gizmos.color = _pathColor;
                foreach (Vector3 pos in _path)
                {
                    Gizmos.DrawSphere(pos, _gizmoRadius);
                }
                int i = 0;
                Gizmos.color = _gizmoPathColor;
                foreach (Vector3 pos in gizmoPoses)
                {
                    Gizmos.DrawSphere(pos, _gizmoRadius * 1.1f);
                    i++;
                }

                if (_path.Count > 0)
                {
                    Gizmos.color = Color.green;
                    _currentNode = Mathf.Clamp(_currentNode, 0, _path.Count - 1);
                    Gizmos.DrawSphere(_path[_currentNode], _gizmoRadius * 1.2f);
                }

            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RoadFromMesh))]
    public class RoadEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RoadFromMesh road = (RoadFromMesh)target;

            if (GUILayout.Button("CreatePath"))
            {
                road.CreateSmoothPath();
            }
        }
    }
#endif

}
