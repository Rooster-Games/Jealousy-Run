//using System.Collections;
//using System.Collections.Generic;
//using NaughtyAttributes;
//using UnityEditor;
//using UnityEngine;

//public class Test : MonoBehaviour
//{
//    [SerializeField] string _path;

//    [SerializeField] float _buildableSurfaceTolerance;
//    [SerializeField] float _collisionClippingTolerance;
//    [SerializeField] float _collisionClippingSnappingTolerance;

//    [Button("Make Smoooth Prefabs")]
//    public void Smooth()
//    {
//        var objects = AssetDatabase.LoadAllAssetsAtPath(_path);
//        Debug.Log(objects.Length);
//        foreach (var obj in objects)
//        {
//            var prefab = obj as GameObject;
//            if (prefab == null)
//                continue;

//            var piece = prefab.GetComponent<PieceBehaviour>();

//            if (piece != null)
//            {
//                piece.CollisionCondition.CollisionClippingToleranceWithBuildSurface = _buildableSurfaceTolerance;
//                piece.CollisionCondition.CollisionClippingTolerance = _collisionClippingTolerance;
//                piece.CollisionCondition.CollisionClippingSnappingTolerance = _collisionClippingSnappingTolerance;
//            }
//            else
//                Debug.Log($"{prefab.gameObject} do not have PieceBehaviour");
//        }
//    }
//}
