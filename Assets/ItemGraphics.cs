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
            if(_whoIsCollecting != whoIsProtecting)
            {
                gameObject.SetActive(false);
            }
        }
    }
}