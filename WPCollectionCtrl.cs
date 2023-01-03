using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WPCollectionCtrl : MonoBehaviour
{
    string key = "CatFishingTycoon";

    public WPCollectInfo[] wpCollectInfo;

    public List<WPCollectInfo> wpCollectInfoList = new List<WPCollectInfo>();

    [Header("[뽑기 표기창]")]
    public GameObject WeaponPickPanel;
    public GameObject ContentRoot;
    public GameObject PrefabPick;
    public Button PickOkBtn;
    public Button WeaponPick1Btn;
    public Button WeaponPick15Btn;
    bool pickLoopbool;
    int pickLoopcount = 0;

    [Header("[강화 관리]")]
    public Button AllUpgradeBtn;
    public ScrollRect ScrollView_A;
    public GameObject Content_WP;
    public GameObject Content_Facke;

    public static WPCollectionCtrl instance;
    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Resources.Load Start()에서 미리 불러오기
        for (int i = 0; i < wpCollectInfo.Length; i++)
        {
            wpCollectInfo[i].image.sprite = Resources.Load<Sprite>("Weapon/Weapon_" + i);
        }

        Invoke("StartDelay", 1f);
    }

    void StartDelay()
    {
       WPCollectList_ReLineup();// 장비 리스트 재정렬 (높은레벨 위로)
    }

    // 뽑기 결과 (타입, 상자 위치)
    public void BoxRandomResult(string type, GameObject pos)
    {
        int ran = Random.Range(0, wpCollectInfo.Length); // 랜덤 뽑기

        // 뽑기 아이템표기
        if (type.Contains("pickpanel")) PickPanelResult("pick", "null", ran);

        // 낚시상자 및 탐사선 무기표기
        if (type.Contains("box")) FishPopupCtrl.instance.BoxWeaponPopup(ran, pos); // 상자 뽑기
        if (type.Contains("ship")) pos.GetComponent<Image>().sprite = wpCollectInfo[ran].image.sprite; // 탐사선 상자

        // 신규 체크
        if (wpCollectInfo[ran].isPurchased == false)
        {
            wpCollectInfo[ran].count++;
            wpCollectInfo[ran].sliderInput();

            wpCollectInfo[ran].gameObject.SetActive(true);
            wpCollectInfo[ran].isPurchased = true;
        }
        else
        {
            // 중복
            wpCollectInfo[ran].count++;
            wpCollectInfo[ran].sliderInput();
        }
        // 장비수집 정보 저장 - (레벨, 개수, 공격력) 아이콘 활성화 여부
        BossDataController.instance.SaveWPCollButton(wpCollectInfo[ran]);
    }

    // 뽑기 표기창 - (타입, 공격력, 배열넘버)
    public void PickPanelResult(string type, string atkstr, int num)
    {
        WeaponPickPanel.SetActive(true);
        GameObject obj = Instantiate(PrefabPick, transform.position, transform.rotation);
        obj.transform.SetParent(ContentRoot.transform);
        obj.SetActive(true);
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        obj.GetComponent<WeaponPick>().image.sprite = wpCollectInfo[num].image.sprite;

        if (type.Contains("pick")) // 일반뽑기
        {
            obj.GetComponent<WeaponPick>().AtkgrowText.gameObject.SetActive(false);
        }
        if (type.Contains("upgrade")) // 강화시
        {
            obj.GetComponent<WeaponPick>().AtkgrowText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(atkstr);
            obj.GetComponent<WeaponPick>().AtkgrowText.gameObject.SetActive(true);
        }
    }

    // 뽑기 확인
    public void OnClickPickOkBtn()
    {
        WeaponPickPanel.SetActive(false);
        for(int i = 0; i < ContentRoot.transform.childCount; i++)
        {
            Destroy(ContentRoot.transform.GetChild(i).gameObject);
        }

        UpgradeWeaponCheck();// 업그레이드 무기개수 체크 버튼 활성화
    }

    // 장비 1개 뽑기
    public void OnClickPick_1count()
    {
        if (DataController.Instance.Dia >= 10)
        {
            DataController.Instance.Dia -= 10;
            OnClickPickOkBtn();// 뽑기 확인 - 기존뽑기장비 제거
            BoxRandomResult("pickpanel", null);
            WeaponPick1Btn.gameObject.SetActive(true);
            WeaponPick15Btn.gameObject.SetActive(true);
        }
        else // 캔 부족 - 상점 오픈
        {
            Language_Ctrl.instance.WarningbarReset();
            Language_Ctrl.instance.Warningbar.SetActive(true);
            Language_Ctrl.instance.Warningbar.transform.GetChild(4).gameObject.SetActive(true);
            GM.Instance.ShopPanel_Open();
        }
    }

    // 장비 15개 뽑기
    public void OnClickPick_15count()
    {
        if (DataController.Instance.Dia >= 140)
        {
            DataController.Instance.Dia -= 140;
            OnClickPickOkBtn();// 뽑기 확인 - 기존뽑기장비 제거
            pickLoopbool = true;
            PickOkBtn.interactable = false;
            WeaponPick1Btn.gameObject.SetActive(true);
            WeaponPick15Btn.gameObject.SetActive(true);
            WeaponPick1Btn.interactable = false;
            WeaponPick15Btn.interactable = false;
            pickLoopcount = 0;
            StartCoroutine(PickcountLoop()); // 15개 뽑기 반복
        }
        else // 캔 부족 - 상점 오픈
        {
            Language_Ctrl.instance.WarningbarReset();
            Language_Ctrl.instance.Warningbar.SetActive(true);
            Language_Ctrl.instance.Warningbar.transform.GetChild(4).gameObject.SetActive(true);
            GM.Instance.ShopPanel_Open();
        }
    }
    // 15개 뽑기 반복
    IEnumerator PickcountLoop()
    {
        while (pickLoopbool)
        {
            BoxRandomResult("pickpanel", null);// 뽑기 결과 (타입, 상자 위치)
            pickLoopcount++;
            if (pickLoopcount >= 15)
            {
                pickLoopbool = false;
                PickOkBtn.interactable = true;
                WeaponPick1Btn.interactable = true;
                WeaponPick15Btn.interactable = true;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // 강화 버튼 - 전체 장비개수 체크 활성화
    public void UpgradeWeaponCheck()
    {
        for (int i = 0; i < wpCollectInfo.Length; i++)
        {
            if (wpCollectInfo[i].count >= 10)
            {
                AllUpgradeBtn.interactable = true;
                return;
            }
            else
            {
                AllUpgradeBtn.interactable = false;
            }
        }
    }
    // 강화 버튼 - 10개이상 장비강화
    public void OnClickButton_AllUpgrade()
    {
        WeaponPick1Btn.gameObject.SetActive(false);
        WeaponPick15Btn.gameObject.SetActive(false);

        for (int i = 0; i < wpCollectInfo.Length; i++)
        {
            if (wpCollectInfo[i].count >= 10)
            {
                wpCollectInfo[i].LevelUpgrade();// 장비 강화 - 레벨증가, 개수초기화, 공격력증가
            }
        }

        WPCollectList_ReLineup();// 장비 리스트 재정렬 (높은레벨 위로)
    }

    // 장비탭 리스트 재정렬 (높은레벨 위로)
    public void WPCollectList_ReLineup()
    {
        wpCollectInfoList.Clear(); // 리스트 초기화
        for (int i = 0; i < wpCollectInfo.Length; i++)
        {
            wpCollectInfoList.Add(wpCollectInfo[i]);
        }
        // 장비탭 높은레벨 리스트 위로
        wpCollectInfoList.Sort(delegate (WPCollectInfo A, WPCollectInfo B)
        {
            if (A.level < B.level) return 1;
            else if (A.level > B.level) return -1;
            return 0;
        });
        // 장비탭 재배치
        for (int i = 0; i < wpCollectInfoList.Count; i++)
        {
            wpCollectInfoList[i].transform.SetParent(Content_Facke.transform);
            wpCollectInfoList[i].transform.SetParent(Content_WP.transform);
            wpCollectInfoList[i].transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }  
}