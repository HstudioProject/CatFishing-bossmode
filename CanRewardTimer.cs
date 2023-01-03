using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CanRewardTimer : MonoBehaviour
{
    string key = "CatFishingTycoon";

    public string name; //key 저장용 네임값
    public int Keycount; //현재 키값
    public string Timerstr; // 첫 번째나무 타이머 

    public int MaxTime;

    public int Maxcount; //키 최대값

    DateTime startTime;
    string startTimestr;

    public GameObject AdsBtn;
    public GameObject GetBtn;
    public GameObject TimerRoot;
    public Text TimerText;

    public GameObject GlowIcon_B; // 탭버튼 알림표시 GetBtn 활성화시 켜주기
    public GameObject BossModeGlow; // 보스모드버튼 알림표시

    // 즉시완료 연속사용 제한 체크
    public int AdsFastCompleteCheck
    {
        get { return PlayerPrefs.GetInt("AdsFastCompleteCheck_" + key + name, 0); }
        set { PlayerPrefs.SetInt("AdsFastCompleteCheck_" + key + name, value); }
    }

    public void Start()
    {
        if (PlayerPrefs.HasKey("KEY_COUNT" + key + name) == false)
        {
            Keycount = 1;
            PlayerPrefs.SetInt("KEY_COUNT" + key + name, Keycount);
            TimerReset();
            RefilKey();
            TimerCheck();
        }
        else
        {
            Keycount = PlayerPrefs.GetInt("KEY_COUNT" + key + name);
            if (Keycount >= Maxcount)
            {
                Keycount = Maxcount;
                Timerstr = "Complete";
                TimerText.text = Timerstr;
                AdsFastCompleteCheck = 0;
                TimerCheck();
            }
            else
            {
                TimerCheck();
            }
            PlayerPrefs.SetInt("KEY_COUNT" + key + name, Keycount);

            RefilKey();
        }
    }
    // 타이머바 - 켜기 끄기
    public void TimerCheck()
    {
        if (AdsFastCompleteCheck == 1)
        {
            TimerRoot.SetActive(true); // 타이머바 켜기
            GetBtn.SetActive(false); // 보상버튼
            AdsBtn.SetActive(false); // 광고버튼
            GlowIcon_B.SetActive(false);
            BossModeGlow.SetActive(false);
        }
        else
        {
            if (Keycount > 0)
            {
                TimerRoot.SetActive(false); // 타이머바 끄기 
                GetBtn.SetActive(true);  // 보상버튼
                AdsBtn.SetActive(false); // 광고버튼
                GlowIcon_B.SetActive(true);
                BossModeGlow.SetActive(true);
            }
            else
            {
                TimerRoot.SetActive(false); // 타이머바 끄기
                GetBtn.SetActive(false);// 보상버튼
                AdsBtn.SetActive(true); // 광고버튼
                GlowIcon_B.SetActive(false);
                BossModeGlow.SetActive(false);
            }
        }
    }

    TimeSpan ts;
    public void RefilKey()
    {
        Keycount = PlayerPrefs.GetInt("KEY_COUNT" + key + name);
        Time_Load();

        if (Keycount < Maxcount)
        {
            // 지난시간
            ts = DateTime.Now - startTime;
            // 최대시간 넘었는지 체크
            if (ts.TotalSeconds >= MaxTime)   //12시간 43200 //3시간 10800
            {
                Keycount += 1;
                TimerReset(); // Time Reset
                //Debug.Log(ts.TotalSeconds);

                if (Keycount >= Maxcount)
                {
                    Keycount = Maxcount;
                    Timerstr = "Complete";
                    TimerText.text = Timerstr;
                    AdsFastCompleteCheck = 0; // 즉시완료 연속사용 제한 초기화
                    TimerCheck();
                }
                PlayerPrefs.SetInt("KEY_COUNT" + key + name, Keycount);

                RefilKey();
            }
            else
            {
                KeyTimer_Play = true;
                StartCoroutine(RoutineTimer()); // Timer Start
            }
        }
        else
        {
            Timerstr = "Complete";
            TimerText.text = Timerstr;
            TimerCheck();
        }
    }

    bool KeyTimer_Play;
    public TimeSpan timercount;
    private IEnumerator RoutineTimer()
    {
        while (KeyTimer_Play == true)
        {
            ts = DateTime.Now - startTime;
            timercount = new TimeSpan(0, 0, MaxTime) - ts; // MaxTime - currentTime

            // 남은 시간 불러오기
            string timeStr = timercount.TotalSeconds.ToString("N0");
            timeStr = timeStr.Replace(",", ""); //TotalSeconds 가 1,000 을 넘으면 (,)콤마가 생김 버그방지용으로 콤마빼기
            int timeInt = int.Parse(timeStr);

            if (timercount.TotalSeconds <= 0)
            {
                //타임 정지
                KeyTimer_Play = false;
                RefilKey();
                AdsFastCompleteCheck = 0; // // 즉시완료 연속사용 제한 초기화
                break;
            }
            else // 타이머 표기
            {
                Timerstr = string.Format("{0:D2}:{1:D2}:{2:D2}", timercount.Hours, timercount.Minutes, timercount.Seconds);
                TimerText.text = Timerstr;

                yield return new WaitForSeconds(1.0f);
                if (Keycount >= Maxcount) break;

                Timerstr = string.Format("{0:D2}:{1:D2}:{2:D2}", timercount.Hours, timercount.Minutes, timercount.Seconds);
                TimerText.text = Timerstr;
            }
        }
    }

    // 시작시간 불러오기
    void Time_Load()
    {
        startTimestr = PlayerPrefs.GetString("KEYTimer" + key + name);
        startTime = DateTime.Parse(startTimestr);
    }

    // 타이머 리셋
    public void TimerReset()
    {
        startTime = DateTime.Now;
        startTimestr = startTime.ToString();
        PlayerPrefs.SetString("KEYTimer" + key + name, startTimestr);
    }

    public void KeyUse()
    {
        if (Keycount > 0)
        {
            if (Keycount == Maxcount) TimerReset();
            Keycount--;
            PlayerPrefs.SetInt("KEY_COUNT" + key + name, Keycount);
            RefilKey();
        }
    }

    // 타이머 직접 완료
    public void TimerDirectComplete()
    {
        Keycount += 1;
        TimerReset(); // Time Reset

        if (Keycount >= Maxcount)
        {
            Keycount = Maxcount;
            Timerstr = "Complete";
            TimerText.text = Timerstr;
            TimerCheck();
        }
        PlayerPrefs.SetInt("KEY_COUNT" + key + name, Keycount);

        RefilKey();
    }

    // 즉시완료 광고완료시 호출
    public void AdsBtnDirectComplete()
    {
        //IEnumerator RoutineTimer() //RefilKey() 2곳에서 0으로 초기화 됨
        // 1) 게임이 켜진 상태에서 타이머가 돌때 2) 게임이 재실행 타이머 완료 시
        TimerDirectComplete();
        AdsFastCompleteCheck = 1;
    }

    // 보상받기
    public void OnClickButton_GetBtn()
    {
        CoinsManager.instance.MissionCoinList_call("dia", 10, "10", GetBtn.transform);
        TimerReset();
        KeyUse();
        TimerCheck();
    }
}