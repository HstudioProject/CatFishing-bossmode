using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBtnCtrl : MonoBehaviour
{
    public bool BossModebool;

    [Header("[위치 지정]")]
    public RectTransform CatBgRoot;
    public Vector2 EndSpot;
    public float speed;
    bool movebool;

    [Header("[보스모드 Off 항목]")]
    public GameObject MiddleMenubar;
    public Transform[] MiddlePos;
    public GameObject TapButtonCtrl;
    public GameObject goldbar;
    public GameObject fishbar;
    public GameObject TopBtnOffList;
    public Button BossModeBtn;
    public GameObject BossBtnLock;

    [Header("[보스모드 On 항목]")]
    public GameObject BossBackGround;
    public Image BossImae;
    public Canvas Canvas_5560_cam;
    public Canvas ShopPanel; 

    [Header("[보스모드 Start]")]
    public GameObject BossRoot;
    public GameObject BossStartPanel;
    public GameObject BossClosePanel;

    [Header("[참고 스크립트]")]
    public BossTimerSliderCtrl bossTimerSlider;
    public CanRewardTimer canRewardTimer;


    //싱글턴 인스턴스 선언
    public static BossBtnCtrl instance = null;

    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
    }

    void Start()
    {
        Invoke("BossBtnLockCheck", 0.5f); // 버튼 락해제 체크
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

    // 버튼 락해제 체크
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
        BossBtnLockCheck();// 버튼 락해제 체크

        // 오토 사용 중이라면 끄기
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
        WPCollectionCtrl.instance.UpgradeWeaponCheck();// 업그레이드 무기개수 체크 버튼 활성화

        // 버튼알리표기 활성화되어 있다면 뽑기 탭으로 바로가기
        if (canRewardTimer.GlowIcon_B.activeSelf)
        {
            BossTapButtonCtrl.instance.Tap_B_Btn();
        }

        Canvas_5560_cam.sortingOrder = 5560;
        ShopPanel.sortingOrder = 5561;
    }

    // 홈으로 돌아가기
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

        // 메인 퀘스트바 미완료시 오픈
        if (MainQuest_Ctrl.instance.MainQuestClear == 0) MainQuest_Ctrl.instance.MainRoot.SetActive(true);

        // 타미어 리셋
        bossTimerSlider.updateTime = bossTimerSlider.coolTime;
        bossTimerSlider.ResetSlider();

        Canvas_5560_cam.sortingOrder = 5060;
        ShopPanel.sortingOrder = 5402;
    }

    public void BossStartBtn()
    {
        // 타미어 재시작
        bossTimerSlider.updateTime = bossTimerSlider.coolTime;
        bossTimerSlider.canSlider = true;

        // 보스 HP리셋
        BossHPCtrl.instance.current = BossHPCtrl.instance.max;
        BossHPCtrl.instance.UpdateUI();

        BossStartPanel.SetActive(false);
        BossClosePanel.SetActive(false);
    }

    // 보스 제거실패 포기
    public void BossGiveUpBtn()
    {
        bossTimerSlider.canSlider = false;
        BossClosePanel.SetActive(true);
    }

    // 보스에게 승리 즉시 다음 보스불러오기
    public void BossWinComplete()
    {
        // 타미어 재시작
        bossTimerSlider.updateTime = bossTimerSlider.coolTime;
        bossTimerSlider.canSlider = true;

        BossHPCtrl.instance.NextBossLoad(); // 다음보스 불러오기
    }
}
