using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class WPCollectInfo : MonoBehaviour
{
    public bool isPurchased; // ���ȴ��� üũ��
    public Image image;
    public string SaveName; // ����� ����
    public int num; //���п� ����

    public int level; //����
    public Text levelText;
    public int count; // ���� ��������
    public Slider slider; // ī��Ʈ �����̴�
    public int startatk; // ���� ���� ���ݷ�
    public BigInteger atk; // ���� ���ݷ�
    public string atkstr; // ���� ���ݷ� Ȯ�ο� ǥ��

    // Start is called before the first frame update
    void Start()
    {
        BossDataController.instance.LoadWPCollButton(this);
        sliderInput(); // ī��Ʈ slider�� �Է�

        if (isPurchased)
        {
            gameObject.gameObject.SetActive(true);
        }
        else
        {
            gameObject.gameObject.SetActive(false);
        }
    }

    // ��� ��ȭ - ��������, �����ʱ�ȭ, ���ݷ�����
    public void LevelUpgrade()
    {
        level++;
        count -= 9; // �������� �������� 1���� ������ �־�� �ؼ� -9
        atk = atk * 12; // ���ݷ� ���� ���� ���� �⺻���� 12��
        WPCollectionCtrl.instance.PickPanelResult("upgrade", atk.ToString(), num);

        sliderInput(); // �����̴� ���� ǥ��
        BossDataController.instance.SaveWPCollButton(this);
    }

    // ī��Ʈ slider�� �Է�
    public void sliderInput()
    {
        levelText.text = "Lv " + level.ToString();

        int Maxcount = 10;
        slider.value = (float)count / (float)Maxcount;

        atkstr = atk.ToString();
    }
}
