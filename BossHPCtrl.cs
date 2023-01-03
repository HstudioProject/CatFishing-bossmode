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
    public int ImageCount; //Sprite �̹��� �ݺ����� ����

    public BigInteger current;
    public BigInteger max;
    public int startMax = 200; // ���� �� ����

    public Slider slider;

    public GameObject DamagePos;
    public Color[] damageColor;

    public BigInteger Bigvalue; // �ٿ� ����� ����
    public string Bigvaluestring;


    //�̱��� �ν��Ͻ� ����
    public static BossHPCtrl instance = null;

    void Awake()
    {
        //�̱��� �ν��Ͻ� �Ҵ�
        instance = this;
    }

    public void Start()
    {
        BossDataController.instance.LoadBossButton(this);
        BossBtnCtrl.instance.BossImae.sprite = Resources.Load<Sprite>("Boss/boss_" + ImageCount);
        UpdateUI();
    }

    // �������� �ҷ�����
    public void NextBossLoad()
    {
        level++;
        BossImageCount(); // ���� �̹��� �ҷ�����
        UpdateUpgrade();
        BossDataController.instance.SaveBossButton(this);
        BossDataController.instance.LoadBossButton(this);
        UpdateUI();

        MainQuest_Ctrl.instance.Start(); //���� ����Ʈ ī��Ʈ ����ȭ
    }

    // ���� max�� ����
    public void UpdateUpgrade()
    {
        max = max + max/100 * 30;// 30% ����
        current = max;
    }

    // ���� �̹��� �ҷ�����
    void BossImageCount()
    {
        if(ImageCount >= 49) //0~49 �̹��� �ݺ�
        {
            ImageCount = 0;
        }
        else
        {
            ImageCount++;
        }
        BossBtnCtrl.instance.BossImae.sprite = Resources.Load<Sprite>("Boss/boss_" + ImageCount);
    }

    // ���� BigInteger(ū����) �������� HPǥ��
    public void UpdateUI()
    {
        currentText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(current.ToString())+ "/" + BigIntegerCtrl_global.bigInteger.ChangeMoney(max.ToString());
        levelText.text = "Boss " + level.ToString();

        slider.minValue = 0;
        slider.maxValue = 100;

        // BigInteger �������ٷ� ǥ���ϱ�
        Bigvalue = current * 100 / max; // ���簪 * 100 / �ִ밪(���۰��� �ݾ�)
        Bigvaluestring = Bigvalue.ToString();

        slider.value = int.Parse(Bigvaluestring); // ���� ����� ��
    }

    // ���� ������ - ũ��Ƽ�� 5% Ȯ���� �ߵ� 5���� ���ݷ�
    public void BossDamage(string damage)
    {
        if (current > 0)
        {
            //ũ��Ƽ�� Ȯ��
            float Percent = 5f; // 5%
            bool balancebool = BoxPercentManager.instance.GetThisChanceResult_Percentage(Percent);
            if (balancebool)
            {
                // ũ��Ƽ�� �ߵ�
                BigInteger damageBig = new BigInteger(damage);
                damageBig = damageBig * 5; // �⺻���ݷ� x5��
                damage = damageBig.ToString();
                current -= damageBig;
            }
            else // �⺻ ���ݷ�
            {
                current -= new BigInteger(damage);
            }
            CoinsManager.instance.BossDamage_call(damage, DamagePos.transform); // ���� ������ Text ȣ��


            BossBtnCtrl.instance.BossImae.color = damageColor[1];
            BossBtnCtrl.instance.BossRoot.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            Invoke("DamageColor", 0.1f);

            if (current <= 0) // ���� ���� �¸�
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
