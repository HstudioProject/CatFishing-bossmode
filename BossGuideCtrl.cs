using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGuideCtrl : MonoBehaviour
{
    string key = "CatFishingTycoon";

    public GameObject[] BossGuidebar;

    //별도의 공격력증가 조건 설명창
    public GameObject BossGuideBtnPanel;

    // 가이드 튜토 몇단계인지 체크
    public int BossGuideLevel
    {
        get { return PlayerPrefs.GetInt("BossGuideLevel_" + key, 0); }
        set { PlayerPrefs.SetInt("BossGuideLevel_" + key, value); }
    }
    // 보스 가이드 완료체크
    public int BossGuideComplete
    {
        get { return PlayerPrefs.GetInt("BossGuideComplete_" + key, 0); }
        set { PlayerPrefs.SetInt("BossGuideComplete_" + key, value); }
    }

    void Start()
    {
        GuidbarReset();
        if (BossGuideComplete == 0)
        {
            GQcurrentPlus(BossGuideLevel);
        }
    }

    // 가이드 퀘스트 순서
    public void GQcurrentPlus(int num)
    {
        if (BossGuideComplete >= 1) return; // 완료체크

        if (0 == num) //0 
        {
            GuidbarReset();
            BossGuidebar[num].SetActive(true);
        }
        if (1 == num) //1 
        {
            BossGuideLevel = num;
            GuidbarReset();
            BossGuidebar[num].SetActive(true);
        }
        if (2 == num) //2 
        {
            BossGuideLevel = num;
            GuidbarReset();
            BossGuidebar[num].SetActive(true);
        }
        if (3 == num) //3
        {
            BossGuideLevel = num;
            GuidbarReset();
            BossGuidebar[num].SetActive(true);
        }
        if (4 == num) //4
        {
            GuidbarReset();
            BossGuideComplete += 1; //보스 가이드 튜토 완료
        }
    }

    void GuidbarReset()
    {
        for (int i = 0; i < BossGuidebar.Length; i++)
        {
            BossGuidebar[i].SetActive(false);
        }
    }

    public void OnClickBossGuideBtnPanel_Open() => BossGuideBtnPanel.SetActive(true);
    public void OnClickBossGuideBtnPanel_Close() => BossGuideBtnPanel.SetActive(false);

}
