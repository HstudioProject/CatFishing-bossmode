using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGuideCtrl : MonoBehaviour
{
    string key = "CatFishingTycoon";

    public GameObject[] BossGuidebar;

    //������ ���ݷ����� ���� ����â
    public GameObject BossGuideBtnPanel;

    // ���̵� Ʃ�� ��ܰ����� üũ
    public int BossGuideLevel
    {
        get { return PlayerPrefs.GetInt("BossGuideLevel_" + key, 0); }
        set { PlayerPrefs.SetInt("BossGuideLevel_" + key, value); }
    }
    // ���� ���̵� �Ϸ�üũ
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

    // ���̵� ����Ʈ ����
    public void GQcurrentPlus(int num)
    {
        if (BossGuideComplete >= 1) return; // �Ϸ�üũ

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
            BossGuideComplete += 1; //���� ���̵� Ʃ�� �Ϸ�
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
