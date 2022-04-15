using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RGLevel
{
    public interface ILevelPathCreator
    {
        CinemachineSmoothPath SmoothPath { get; }
    }

    public class LevelPath : MonoBehaviour, ILevelPathCreator
    {
        [SerializeField] private CinemachineSmoothPath _smoothPath;

        [SerializeField] private List<Vector3> _path = new List<Vector3>();

        public List<Vector3> Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public CinemachineSmoothPath SmoothPath
        {
            get
            {
                if (_smoothPath != null)
                {
                    return _smoothPath;
                }
                else
                {
                    Debug.LogError("Thers is no assigned smooth path!!");
                    return null;
                }
            }

            set => _smoothPath = value;
        }

        public int levelStartIndex = 0;

        [Space(20)]
        [Header("Gizmo Configs")]
        [SerializeField]
        private bool _showPath = false;
        [SerializeField]
        private float _gizmoRadius = 0.25f;
        [SerializeField]
        private Color _gizmoColor = Color.blue;
        [SerializeField]
        private int _currentIndex = 0;
        private void OnDrawGizmos()
        {
            if (_path.Count > 0 && _showPath)
            {
                Gizmos.color = _gizmoColor;
                for (int i = 0; i < _path.Count; i++)
                {
                    Gizmos.DrawSphere(_path[i], _gizmoRadius);
                }

                if (_path.Count > 0)
                {
                    Gizmos.color = Color.green;
                    _currentIndex = Mathf.Clamp(_currentIndex, 0, _path.Count - 1);
                    Gizmos.DrawSphere(_path[_currentIndex], _gizmoRadius * 1.2f);
                }
            }
        }
    }

}

