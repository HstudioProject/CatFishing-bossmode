using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class BossHPCtrl : MonoBehaviour
{
    public string name;
    public Text currentText;
    public Text levelText;
    public int level;
    public int ImageCount; //Sprite 이미지 반복사용용 변수

    public BigInteger current;
    public BigInteger max;
    public int startMax = 200; // 시작 시 개수

    public Slider slider;

    public GameObject DamagePos;
    public Color[] damageColor;

    public BigInteger Bigvalue; // 바에 백분율 적용
    public string Bigvaluestring;


    //싱글턴 인스턴스 선언
    public static BossHPCtrl instance = null;

    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
    }

    public void Start()
    {
        BossDataController.instance.LoadBossButton(this);
        BossBtnCtrl.instance.BossImae.sprite = Resources.Load<Sprite>("Boss/boss_" + ImageCount);
        UpdateUI();
    }

    // 다음보스 불러오기
    public void NextBossLoad()
    {
        level++;
        BossImageCount(); // 보스 이미지 불러오기
        UpdateUpgrade();
        BossDataController.instance.SaveBossButton(this);
        BossDataController.instance.LoadBossButton(this);
        UpdateUI();

        MainQuest_Ctrl.instance.Start(); //메인 퀘스트 카운트 동기화
    }

    // 보스 max값 증가
    public void UpdateUpgrade()
    {
        max = max + max/100 * 30;// 30% 증가
        current = max;
    }

    // 보스 이미지 불러오기
    void BossImageCount()
    {
        if(ImageCount >= 49) //0~49 이미지 반복
        {
            ImageCount = 0;
        }
        else
        {
            ImageCount++;
        }
        BossBtnCtrl.instance.BossImae.sprite = Resources.Load<Sprite>("Boss/boss_" + ImageCount);
    }

    // 보스 BigInteger(큰숫자) 게이지바 HP표기
    public void UpdateUI()
    {
        currentText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(current.ToString())+ "/" + BigIntegerCtrl_global.bigInteger.ChangeMoney(max.ToString());
        levelText.text = "Boss " + level.ToString();

        slider.minValue = 0;
        slider.maxValue = 100;

        // BigInteger 게이지바로 표현하기
        Bigvalue = current * 100 / max; // 현재값 * 100 / 최대값(업글가능 금액)
        Bigvaluestring = Bigvalue.ToString();

        slider.value = int.Parse(Bigvaluestring); // 현재 백분율 값
    }

    // 보스 데미지 - 크리티컬 5% 확률로 발동 5배의 공격력
    public void BossDamage(string damage)
    {
        if (current > 0)
        {
            //크리티컬 확률
            float Percent = 5f; // 5%
            bool balancebool = BoxPercentManager.instance.GetThisChanceResult_Percentage(Percent);
            if (balancebool)
            {
                // 크리티컬 발동
                BigInteger damageBig = new BigInteger(damage);
                damageBig = damageBig * 5; // 기본공격력 x5배
                damage = damageBig.ToString();
                current -= damageBig;
            }
            else // 기본 공격력
            {
                current -= new BigInteger(damage);
            }
            CoinsManager.instance.BossDamage_call(damage, DamagePos.transform); // 보스 데미지 Text 호출


            BossBtnCtrl.instance.BossImae.color = damageColor[1];
            BossBtnCtrl.instance.BossRoot.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            Invoke("DamageColor", 0.1f);

            if (current <= 0) // 보스 제거 승리
            {
                current = 0;
                BossBtnCtrl.instance.BossWinComplete();
            }
        }
        UpdateUI();
    }

    void DamageColor()
    {
        BossBtnCtrl.instance.BossImae.color = damageColor[0];
        BossBtnCtrl.instance.BossRoot.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
