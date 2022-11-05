using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    Slider mslider;
    bool up;
    bool stap;
    float swingSpeed;

    public void StartSlider(float speed)
    {
        mslider = this.GetComponent<Slider>();
        up = true;
        stap = false;
        swingSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float slideval = mslider.value;
        if (up && !stap)
        {
            mslider.value += 0.01f * swingSpeed;
            if(slideval >= mslider.maxValue)
                up = false;
        }
        else if (!up && !stap)
        {
            mslider.value -= 0.01f * swingSpeed;
            if(slideval <= mslider.minValue)
                up = true;
        }
    }

    public void StopSlide()
    {
        stap = true;
    }
}
