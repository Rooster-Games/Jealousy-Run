using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JR
{

    public abstract class BaseBar : MonoBehaviour
    {
        [SerializeField] Gender _boundedGender;

        public Gender BoundedGender => _boundedGender;

        public virtual void Init(InitParameters initParameters)
        {
            ChangeAmount(initParameters.StartingPercent);
        }

        public abstract void ChangeAmount(float percent);

        public class InitParameters
        {
            public float StartingPercent { get; set; }
        }

    }

    public class ImageBar : BaseBar
    {
        Image _myImage;

        public override void Init(InitParameters initParameters)
        {
            _myImage = GetComponent<Image>();
            base.Init(initParameters);
        }

        public override void ChangeAmount(float percent)
        {
            _myImage.fillAmount = percent;
        }

    }
}