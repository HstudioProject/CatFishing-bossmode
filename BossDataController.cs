using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class BossDataController : MonoBehaviour
{
    string key = "CatFishingTycoon";

    public Text attackText;
    public BigInteger attack;

    public Text PickdiaText;

    //�̱��� �ν��Ͻ� ����
    public static BossDataController instance = null;

    void Awake()
    {
        //�̱��� �ν��Ͻ� �Ҵ�
        instance = this;
    }

    void Start()
    {
        StartCoroutine(UpdateLoop());
    }

    // ���� ���ݷ� ������
    public BigInteger GetTotalAttack()
    {
        BigInteger TotalattackValue = 0;
        WPCollectionCtrl WPCollect = WPCollectionCtrl.instance;

        for (int i = 0; i < WPCollect.wpCollectInfo.Length; i++)
        {
            if (WPCollect.wpCollectInfo[i].isPurchased == true)
            {
                TotalattackValue += WPCollect.wpCollectInfo[i].atk * WPCollect.wpCollectInfo[i].count;
            }
        }
        return TotalattackValue;
    }

    // ���� ���ݷ� �ҷ����� 
    public void TotalAttackValueLoad() 
    {
        attack = GetTotalAttack();
        string attackStr = attack.ToString();
        attackText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(attackStr);
    }

    // ���ݷ� �ݺ�üũ ����ȭ
    private IEnumerator UpdateLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return delay;
            TotalAttackValueLoad(); // ���� ���ݷ� �ҷ�����
            PickdiaText.text = DataController.Instance.Dia.ToString(); //���̾� ���� ����ȭ
        }
    }

    // �������� �ҷ����� - (����, �̹���, �ִ�HP)
    public void LoadBossButton(BossHPCtrl bossHPCtrl)
    {
        string name = bossHPCtrl.name;
        string max;

        bossHPCtrl.level = PlayerPrefs.GetInt(name + key + "_level", 1);
        bossHPCtrl.ImageCount = PlayerPrefs.GetInt(name + key + "_ImageCount", 0);
        max = PlayerPrefs.GetString(name + key + "_max", bossHPCtrl.startMax.ToString());
        bossHPCtrl.max = new BigInteger(max);
        bossHPCtrl.current = bossHPCtrl.max;
    }
    // �������� ���� - (����, �̹���, �ִ�HP)
    public void SaveBossButton(BossHPCtrl bossHPCtrl)
    {
        string name = bossHPCtrl.name;
        string max = bossHPCtrl.max.ToString();

        PlayerPrefs.SetInt(name + key + "_level", bossHPCtrl.level);
        PlayerPrefs.SetInt(name + key + "_ImageCount", bossHPCtrl.ImageCount);
        PlayerPrefs.SetString(name + key + "_max", max);
    }

    // ������ ���� �ҷ����� - (����, ����, ���ݷ�) ������ Ȱ��ȭ ����
    public void LoadWPCollButton(WPCollectInfo wpCollectInfo)
    {
        string name = wpCollectInfo.SaveName;
        string atk;

        wpCollectInfo.level = PlayerPrefs.GetInt(name + key + "_level", 1);
        wpCollectInfo.count = PlayerPrefs.GetInt(name + key + "_count", 0);
        atk = PlayerPrefs.GetString(name + key + "_atk", wpCollectInfo.startatk.ToString());
        wpCollectInfo.atk = new BigInteger(atk);

        if (PlayerPrefs.GetInt(name + key + "_isPurchased") == 1)
        {
            wpCollectInfo.isPurchased = true;
        }
        else
        {
            wpCollectInfo.isPurchased = false;
        }
    }
    // ������ ���� ���� - (����, ����, ���ݷ�) ������ Ȱ��ȭ ����
    public void SaveWPCollButton(WPCollectInfo wpCollectInfo)
    {
        string name = wpCollectInfo.SaveName;
        string atk = wpCollectInfo.atk.ToString(); 

        PlayerPrefs.SetInt(name + key + "_level", wpCollectInfo.level);
        PlayerPrefs.SetInt(name + key + "_count", wpCollectInfo.count);
        PlayerPrefs.SetString(name + key + "_atk", atk);

        if (wpCollectInfo.isPurchased == true)
        {
            PlayerPrefs.SetInt(name + key + "_isPurchased", 1);
        }
        else
        {
            PlayerPrefs.SetInt(name + key + "_isPurchased", 0);
        }
    }
}