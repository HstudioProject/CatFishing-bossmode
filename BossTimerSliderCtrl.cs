using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTimerSliderCtrl : MonoBehaviour
{
    public Slider itemSlider;
    public bool canSlider;

    public float updateTime;
    public float coolTime;

    public void ResetSlider()
    {
        itemSlider.value = updateTime / coolTime;
    }

    void Update()
    {
        if (canSlider)
        {
            updateTime = updateTime - Time.deltaTime;
            itemSlider.value = updateTime / coolTime;

            if (updateTime > 0)
            {

            }
            else
            {
                // 보스 제거 실패
                BossBtnCtrl.instance.BossGiveUpBtn();
                canSlider = false;
            }
        }
    }
}
