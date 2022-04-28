using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class ItemGraphics : MonoBehaviour
    {
        [SerializeField] Gender _whoIsCollecting;

        public void CheckGameType(Gender whoIsProtecting)
        {
            gameObject.SetActive(_whoIsCollecting == whoIsProtecting);
        }
    }
}