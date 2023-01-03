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

    //싱글턴 인스턴스 선언
    public static BossDataController instance = null;

    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
    }

    void Start()
    {
        StartCoroutine(UpdateLoop());
    }

    // 총합 공격력 증가값
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

    // 총합 공격력 불러오기 
    public void TotalAttackValueLoad() 
    {
        attack = GetTotalAttack();
        string attackStr = attack.ToString();
        attackText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(attackStr);
    }

    // 공격력 반복체크 최적화
    private IEnumerator UpdateLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return delay;
            TotalAttackValueLoad(); // 총합 공격력 불러오기
            PickdiaText.text = DataController.Instance.Dia.ToString(); //다이아 개수 동기화
        }
    }

    // 보스정보 불러오기 - (레벨, 이미지, 최대HP)
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
    // 보스정보 저장 - (레벨, 이미지, 최대HP)
    public void SaveBossButton(BossHPCtrl bossHPCtrl)
    {
        string name = bossHPCtrl.name;
        string max = bossHPCtrl.max.ToString();

        PlayerPrefs.SetInt(name + key + "_level", bossHPCtrl.level);
        PlayerPrefs.SetInt(name + key + "_ImageCount", bossHPCtrl.ImageCount);
        PlayerPrefs.SetString(name + key + "_max", max);
    }

    // 장비수집 정보 불러오기 - (레벨, 개수, 공격력) 아이콘 활성화 여부
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
    // 장비수집 정보 저장 - (레벨, 개수, 공격력) 아이콘 활성화 여부
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