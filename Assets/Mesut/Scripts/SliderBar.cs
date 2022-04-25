using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JR
{
    public class SliderBar : BaseBar
    {
        Slider _mySlider;

        public override void Init(InitParameters initParameters)
        {
            _mySlider = GetComponent<Slider>();
            base.Init(initParameters);
        }

        public override void ChangeAmount(float percent)
        {
            _mySlider.value = percent;
        }
    }
}
