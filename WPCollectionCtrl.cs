using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WPCollectionCtrl : MonoBehaviour
{
    string key = "CatFishingTycoon";

    public WPCollectInfo[] wpCollectInfo;

    public List<WPCollectInfo> wpCollectInfoList = new List<WPCollectInfo>();

    [Header("[�̱� ǥ��â]")]
    public GameObject WeaponPickPanel;
    public GameObject ContentRoot;
    public GameObject PrefabPick;
    public Button PickOkBtn;
    public Button WeaponPick1Btn;
    public Button WeaponPick15Btn;
    bool pickLoopbool;
    int pickLoopcount = 0;

    [Header("[��ȭ ����]")]
    public Button AllUpgradeBtn;
    public ScrollRect ScrollView_A;
    public GameObject Content_WP;
    public GameObject Content_Facke;

    public static WPCollectionCtrl instance;
    void Awake()
    {
        //�̱��� �ν��Ͻ� �Ҵ�
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Resources.Load Start()���� �̸� �ҷ�����
        for (int i = 0; i < wpCollectInfo.Length; i++)
        {
            wpCollectInfo[i].image.sprite = Resources.Load<Sprite>("Weapon/Weapon_" + i);
        }

        Invoke("StartDelay", 1f);
    }

    void StartDelay()
    {
       WPCollectList_ReLineup();// ��� ����Ʈ ������ (�������� ����)
    }

    // �̱� ��� (Ÿ��, ���� ��ġ)
    public void BoxRandomResult(string type, GameObject pos)
    {
        int ran = Random.Range(0, wpCollectInfo.Length); // ���� �̱�

        // �̱� ������ǥ��
        if (type.Contains("pickpanel")) PickPanelResult("pick", "null", ran);

        // ���û��� �� Ž�缱 ����ǥ��
        if (type.Contains("box")) FishPopupCtrl.instance.BoxWeaponPopup(ran, pos); // ���� �̱�
        if (type.Contains("ship")) pos.GetComponent<Image>().sprite = wpCollectInfo[ran].image.sprite; // Ž�缱 ����

        // �ű� üũ
        if (wpCollectInfo[ran].isPurchased == false)
        {
            wpCollectInfo[ran].count++;
            wpCollectInfo[ran].sliderInput();

            wpCollectInfo[ran].gameObject.SetActive(true);
            wpCollectInfo[ran].isPurchased = true;
        }
        else
        {
            // �ߺ�
            wpCollectInfo[ran].count++;
            wpCollectInfo[ran].sliderInput();
        }
        // ������ ���� ���� - (����, ����, ���ݷ�) ������ Ȱ��ȭ ����
        BossDataController.instance.SaveWPCollButton(wpCollectInfo[ran]);
    }

    // �̱� ǥ��â - (Ÿ��, ���ݷ�, �迭�ѹ�)
    public void PickPanelResult(string type, string atkstr, int num)
    {
        WeaponPickPanel.SetActive(true);
        GameObject obj = Instantiate(PrefabPick, transform.position, transform.rotation);
        obj.transform.SetParent(ContentRoot.transform);
        obj.SetActive(true);
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        obj.GetComponent<WeaponPick>().image.sprite = wpCollectInfo[num].image.sprite;

        if (type.Contains("pick")) // �Ϲݻ̱�
        {
            obj.GetComponent<WeaponPick>().AtkgrowText.gameObject.SetActive(false);
        }
        if (type.Contains("upgrade")) // ��ȭ��
        {
            obj.GetComponent<WeaponPick>().AtkgrowText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(atkstr);
            obj.GetComponent<WeaponPick>().AtkgrowText.gameObject.SetActive(true);
        }
    }

    // �̱� Ȯ��
    public void OnClickPickOkBtn()
    {
        WeaponPickPanel.SetActive(false);
        for(int i = 0; i < ContentRoot.transform.childCount; i++)
        {
            Destroy(ContentRoot.transform.GetChild(i).gameObject);
        }

        UpgradeWeaponCheck();// ���׷��̵� ���ⰳ�� üũ ��ư Ȱ��ȭ
    }

    // ��� 1�� �̱�
    public void OnClickPick_1count()
    {
        if (DataController.Instance.Dia >= 10)
        {
            DataController.Instance.Dia -= 10;
            OnClickPickOkBtn();// �̱� Ȯ�� - �����̱���� ����
            BoxRandomResult("pickpanel", null);
            WeaponPick1Btn.gameObject.SetActive(true);
            WeaponPick15Btn.gameObject.SetActive(true);
        }
        else // ĵ ���� - ���� ����
        {
            Language_Ctrl.instance.WarningbarReset();
            Language_Ctrl.instance.Warningbar.SetActive(true);
            Language_Ctrl.instance.Warningbar.transform.GetChild(4).gameObject.SetActive(true);
            GM.Instance.ShopPanel_Open();
        }
    }

    // ��� 15�� �̱�
    public void OnClickPick_15count()
    {
        if (DataController.Instance.Dia >= 140)
        {
            DataController.Instance.Dia -= 140;
            OnClickPickOkBtn();// �̱� Ȯ�� - �����̱���� ����
            pickLoopbool = true;
            PickOkBtn.interactable = false;
            WeaponPick1Btn.gameObject.SetActive(true);
            WeaponPick15Btn.gameObject.SetActive(true);
            WeaponPick1Btn.interactable = false;
            WeaponPick15Btn.interactable = false;
            pickLoopcount = 0;
            StartCoroutine(PickcountLoop()); // 15�� �̱� �ݺ�
        }
        else // ĵ ���� - ���� ����
        {
            Language_Ctrl.instance.WarningbarReset();
            Language_Ctrl.instance.Warningbar.SetActive(true);
            Language_Ctrl.instance.Warningbar.transform.GetChild(4).gameObject.SetActive(true);
            GM.Instance.ShopPanel_Open();
        }
    }
    // 15�� �̱� �ݺ�
    IEnumerator PickcountLoop()
    {
        while (pickLoopbool)
        {
            BoxRandomResult("pickpanel", null);// �̱� ��� (Ÿ��, ���� ��ġ)
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

    // ��ȭ ��ư - ��ü ��񰳼� üũ Ȱ��ȭ
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
    // ��ȭ ��ư - 10���̻� ���ȭ
    public void OnClickButton_AllUpgrade()
    {
        WeaponPick1Btn.gameObject.SetActive(false);
        WeaponPick15Btn.gameObject.SetActive(false);

        for (int i = 0; i < wpCollectInfo.Length; i++)
        {
            if (wpCollectInfo[i].count >= 10)
            {
                wpCollectInfo[i].LevelUpgrade();// ��� ��ȭ - ��������, �����ʱ�ȭ, ���ݷ�����
            }
        }

        WPCollectList_ReLineup();// ��� ����Ʈ ������ (�������� ����)
    }

    // ����� ����Ʈ ������ (�������� ����)
    public void WPCollectList_ReLineup()
    {
        wpCollectInfoList.Clear(); // ����Ʈ �ʱ�ȭ
        for (int i = 0; i < wpCollectInfo.Length; i++)
        {
            wpCollectInfoList.Add(wpCollectInfo[i]);
        }
        // ����� �������� ����Ʈ ����
        wpCollectInfoList.Sort(delegate (WPCollectInfo A, WPCollectInfo B)
        {
            if (A.level < B.level) return 1;
            else if (A.level > B.level) return -1;
            return 0;
        });
        // ����� ���ġ
        for (int i = 0; i < wpCollectInfoList.Count; i++)
        {
            wpCollectInfoList[i].transform.SetParent(Content_Facke.transform);
            wpCollectInfoList[i].transform.SetParent(Content_WP.transform);
            wpCollectInfoList[i].transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }  
}