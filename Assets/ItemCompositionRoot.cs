using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class ItemCompositionRoot : MonoBehaviour
    {
        public void Init(InitParameters initParameters)
        {
            var itemGraphicCollection = GetComponentsInChildren<ItemGraphics>(true);

            foreach (var itemGraphic in itemGraphicCollection)
            {
                Debug.Log("item composition root:" + initParameters.WhoIsProtecting);
                itemGraphic.CheckGameType(initParameters.WhoIsProtecting);
            }
        }

        public class InitParameters
        {
            public Gender WhoIsProtecting { get; set; }
        }
    }
}