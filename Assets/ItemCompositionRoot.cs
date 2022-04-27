using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class ItemCompositionRoot : MonoBehaviour
    {
        public void Init(InitParameters initParameters)
        {
            var itemGraphicCollection = GetComponentsInChildren<ItemGraphics>();

            foreach (var itemGraphic in itemGraphicCollection)
            {
                itemGraphic.CheckGameType(initParameters.WhoIsProtecting);
            }
        }

        public class InitParameters
        {
            public Gender WhoIsProtecting { get; set; }
        }
    }
}