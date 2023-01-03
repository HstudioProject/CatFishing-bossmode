using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class WPCollectInfo : MonoBehaviour
{
    public bool isPurchased; // 열렸는지 체크용
    public Image image;
    public string SaveName; // 저장용 네임
    public int num; //구분용 네임

    public int level; //레벨
    public Text levelText;
    public int count; // 누적 수집개수
    public Slider slider; // 카운트 슬라이더
    public int startatk; // 최초 시작 공격력
    public BigInteger atk; // 개당 공격력
    public string atkstr; // 개당 공격력 확인용 표기

    // Start is called before the first frame update
    void Start()
    {
        BossDataController.instance.LoadWPCollButton(this);
        sliderInput(); // 카운트 slider값 입력

        if (isPurchased)
        {
            gameObject.gameObject.SetActive(true);
        }
        else
        {
            gameObject.gameObject.SetActive(false);
        }
    }

    // 장비 강화 - 레벨증가, 개수초기화, 공격력증가
    public void LevelUpgrade()
    {
        level++;
        count -= 9; // 다음레벨 보유개수 1개는 가지고 있어야 해서 -9
        atk = atk * 12; // 공격력 증가 이전 레벨 기본값에 12배
        WPCollectionCtrl.instance.PickPanelResult("upgrade", atk.ToString(), num);

        sliderInput(); // 슬라이더 개수 표기
        BossDataController.instance.SaveWPCollButton(this);
    }

    // 카운트 slider값 입력
    public void sliderInput()
    {
        levelText.text = "Lv " + level.ToString();

        int Maxcount = 10;
        slider.value = (float)count / (float)Maxcount;

        atkstr = atk.ToString();
    }
}
