using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBtnCtrl : MonoBehaviour
{
    public bool BossModebool;

    [Header("[��ġ ����]")]
    public RectTransform CatBgRoot;
    public Vector2 EndSpot;
    public float speed;
    bool movebool;

    [Header("[������� Off �׸�]")]
    public GameObject MiddleMenubar;
    public Transform[] MiddlePos;
    public GameObject TapButtonCtrl;
    public GameObject goldbar;
    public GameObject fishbar;
    public GameObject TopBtnOffList;
    public Button BossModeBtn;
    public GameObject BossBtnLock;

    [Header("[������� On �׸�]")]
    public GameObject BossBackGround;
    public Image BossImae;
    public Canvas Canvas_5560_cam;
    public Canvas ShopPanel; 

    [Header("[������� Start]")]
    public GameObject BossRoot;
    public GameObject BossStartPanel;
    public GameObject BossClosePanel;

    [Header("[���� ��ũ��Ʈ]")]
    public BossTimerSliderCtrl bossTimerSlider;
    public CanRewardTimer canRewardTimer;


    //�̱��� �ν��Ͻ� ����
    public static BossBtnCtrl instance = null;

    void Awake()
    {
        //�̱��� �ν��Ͻ� �Ҵ�
        instance = this;
    }

    void Start()
    {
        Invoke("BossBtnLockCheck", 0.5f); // ��ư ������ üũ
    }

    void Update()
    {
        if (movebool)
        {
            CatBgRoot.localPosition = Vector2.MoveTowards(CatBgRoot.localPosition, EndSpot, speed * Time.deltaTime);
        }
        if (Vector2.Distance(CatBgRoot.localPosition, EndSpot) < 0.01f)
        {
            movebool = false;
        }
    }

    // ��ư ������ üũ
    public void BossBtnLockCheck()
    {
        int level = DataController.Instance.fishButton.level;
        if (level >= 4)
        {
            BossModeBtn.interactable = true;
            BossBtnLock.SetActive(false);
        }
        else
        {
            BossModeBtn.interactable = false;
            BossBtnLock.SetActive(true);
        }
    }

    public void BossModeOpen()
    {
        BossBtnLockCheck();// ��ư ������ üũ

        // ���� ��� ���̶�� ����
        if (FishButtonCtrl.instance.AutoStopBtn.activeSelf)
        {
            FishButtonCtrl.instance.OnClickBtn_AutoStart();
        }

        movebool = true;
        EndSpot = new Vector2(270f, 0f);

        BossModebool = true;
        MiddleMenubar.GetComponent<RectTransform>().localPosition = MiddlePos[1].localPosition;
        TapButtonCtrl.SetActive(false);
        goldbar.SetActive(false);
        fishbar.SetActive(false);
        TopBtnOffList.SetActive(false);

        BossBackGround.SetActive(true);
        BossRoot.SetActive(true);
        BossStartPanel.SetActive(true);
        BossClosePanel.SetActive(false);
        MainQuest_Ctrl.instance.MainRoot.SetActive(false);

        BossHPCtrl.instance.Start();
        WPCollectionCtrl.instance.UpgradeWeaponCheck();// ���׷��̵� ���ⰳ�� üũ ��ư Ȱ��ȭ

        // ��ư�˸�ǥ�� Ȱ��ȭ�Ǿ� �ִٸ� �̱� ������ �ٷΰ���
        if (canRewardTimer.GlowIcon_B.activeSelf)
        {
            BossTapButtonCtrl.instance.Tap_B_Btn();
        }

        Canvas_5560_cam.sortingOrder = 5560;
        ShopPanel.sortingOrder = 5561;
    }

    // Ȩ���� ���ư���
    public void BossModeClose()
    {
        movebool = true;
        EndSpot = new Vector2(0f, 0f);

        BossModebool = false;
        MiddleMenubar.GetComponent<RectTransform>().localPosition = MiddlePos[0].localPosition;
        TapButtonCtrl.SetActive(true);
        goldbar.SetActive(true);
        fishbar.SetActive(true);
        TopBtnOffList.SetActive(true);

        BossBackGround.SetActive(false);
        BossRoot.SetActive(false);

        // ���� ����Ʈ�� �̿Ϸ�� ����
        if (MainQuest_Ctrl.instance.MainQuestClear == 0) MainQuest_Ctrl.instance.MainRoot.SetActive(true);

        // Ÿ�̾� ����
        bossTimerSlider.updateTime = bossTimerSlider.coolTime;
        bossTimerSlider.ResetSlider();

        Canvas_5560_cam.sortingOrder = 5060;
        ShopPanel.sortingOrder = 5402;
    }

    public void BossStartBtn()
    {
        // Ÿ�̾� �����
        bossTimerSlider.updateTime = bossTimerSlider.coolTime;
        bossTimerSlider.canSlider = true;

        // ���� HP����
        BossHPCtrl.instance.current = BossHPCtrl.instance.max;
        BossHPCtrl.instance.UpdateUI();

        BossStartPanel.SetActive(false);
        BossClosePanel.SetActive(false);
    }

    // ���� ���Ž��� ����
    public void BossGiveUpBtn()
    {
        bossTimerSlider.canSlider = false;
        BossClosePanel.SetActive(true);
    }

    // �������� �¸� ��� ���� �����ҷ�����
    public void BossWinComplete()
    {
        // Ÿ�̾� �����
        bossTimerSlider.updateTime = bossTimerSlider.coolTime;
        bossTimerSlider.canSlider = true;

        BossHPCtrl.instance.NextBossLoad(); // �������� �ҷ�����
    }
}
