using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CanRewardTimer : MonoBehaviour
{
    string key = "CatFishingTycoon";

    public string name; //key ����� ���Ӱ�
    public int Keycount; //���� Ű��
    public string Timerstr; // ù ��°���� Ÿ�̸� 

    public int MaxTime;

    public int Maxcount; //Ű �ִ밪

    DateTime startTime;
    string startTimestr;

    public GameObject AdsBtn;
    public GameObject GetBtn;
    public GameObject TimerRoot;
    public Text TimerText;

    public GameObject GlowIcon_B; // �ǹ�ư �˸�ǥ�� GetBtn Ȱ��ȭ�� ���ֱ�
    public GameObject BossModeGlow; // ��������ư �˸�ǥ��

    // ��ÿϷ� ���ӻ�� ���� üũ
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
    // Ÿ�̸ӹ� - �ѱ� ����
    public void TimerCheck()
    {
        if (AdsFastCompleteCheck == 1)
        {
            TimerRoot.SetActive(true); // Ÿ�̸ӹ� �ѱ�
            GetBtn.SetActive(false); // �����ư
            AdsBtn.SetActive(false); // �����ư
            GlowIcon_B.SetActive(false);
            BossModeGlow.SetActive(false);
        }
        else
        {
            if (Keycount > 0)
            {
                TimerRoot.SetActive(false); // Ÿ�̸ӹ� ���� 
                GetBtn.SetActive(true);  // �����ư
                AdsBtn.SetActive(false); // �����ư
                GlowIcon_B.SetActive(true);
                BossModeGlow.SetActive(true);
            }
            else
            {
                TimerRoot.SetActive(false); // Ÿ�̸ӹ� ����
                GetBtn.SetActive(false);// �����ư
                AdsBtn.SetActive(true); // �����ư
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
            // �����ð�
            ts = DateTime.Now - startTime;
            // �ִ�ð� �Ѿ����� üũ
            if (ts.TotalSeconds >= MaxTime)   //12�ð� 43200 //3�ð� 10800
            {
                Keycount += 1;
                TimerReset(); // Time Reset
                //Debug.Log(ts.TotalSeconds);

                if (Keycount >= Maxcount)
                {
                    Keycount = Maxcount;
                    Timerstr = "Complete";
                    TimerText.text = Timerstr;
                    AdsFastCompleteCheck = 0; // ��ÿϷ� ���ӻ�� ���� �ʱ�ȭ
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

            // ���� �ð� �ҷ�����
            string timeStr = timercount.TotalSeconds.ToString("N0");
            timeStr = timeStr.Replace(",", ""); //TotalSeconds �� 1,000 �� ������ (,)�޸��� ���� ���׹��������� �޸�����
            int timeInt = int.Parse(timeStr);

            if (timercount.TotalSeconds <= 0)
            {
                //Ÿ�� ����
                KeyTimer_Play = false;
                RefilKey();
                AdsFastCompleteCheck = 0; // // ��ÿϷ� ���ӻ�� ���� �ʱ�ȭ
                break;
            }
            else // Ÿ�̸� ǥ��
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

    // ���۽ð� �ҷ�����
    void Time_Load()
    {
        startTimestr = PlayerPrefs.GetString("KEYTimer" + key + name);
        startTime = DateTime.Parse(startTimestr);
    }

    // Ÿ�̸� ����
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

    // Ÿ�̸� ���� �Ϸ�
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

    // ��ÿϷ� ����Ϸ�� ȣ��
    public void AdsBtnDirectComplete()
    {
        //IEnumerator RoutineTimer() //RefilKey() 2������ 0���� �ʱ�ȭ ��
        // 1) ������ ���� ���¿��� Ÿ�̸Ӱ� ���� 2) ������ ����� Ÿ�̸� �Ϸ� ��
        TimerDirectComplete();
        AdsFastCompleteCheck = 1;
    }

    // ����ޱ�
    public void OnClickButton_GetBtn()
    {
        CoinsManager.instance.MissionCoinList_call("dia", 10, "10", GetBtn.transform);
        TimerReset();
        KeyUse();
        TimerCheck();
    }
}