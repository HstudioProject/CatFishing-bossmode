using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTapButtonCtrl : MonoBehaviour
{
    public GameObject scrollview_A;
    public GameObject scrollview_B;

    //public GameObject IconReturn; // C 버튼이 눌릴때 켜지기

    [Header("[탭 버튼]")]
    public GameObject Tap_A;
    public GameObject Tap_B;
    public Text Text_A;
    public Text Text_B;

    public GameObject fakeRoot;
    public GameObject parentRoot;

    [Header("[탭 버튼 컬러]")]
    public Color[] ColorList;
    public Color[] TextColor;

    //싱글턴 인스턴스 선언
    public static BossTapButtonCtrl instance = null;

    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
    }

    void Start()
    {
        if (GM.Instance.TutoCheck != 0) Tap_A_Btn();
    }

    void TapReset()
    {
        Tap_A.gameObject.GetComponent<Image>().color = ColorList[1];
        Tap_B.gameObject.GetComponent<Image>().color = ColorList[1];
        Text_A.color = TextColor[1];
        Text_B.color = TextColor[1];
    }


    public void Tap_A_Btn()
    {
        TapReset();

        Tap_A.gameObject.GetComponent<Image>().color = ColorList[0];
        Text_A.color = TextColor[0];

        scrollview_A.transform.SetParent(fakeRoot.transform);
        scrollview_A.transform.SetParent(parentRoot.transform);

        scrollview_A.GetComponent<RectTransform>().transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void Tap_B_Btn()
    {
        TapReset();

        Tap_B.gameObject.GetComponent<Image>().color = ColorList[0];
        Text_B.color = TextColor[0];

        scrollview_B.transform.SetParent(fakeRoot.transform);
        scrollview_B.transform.SetParent(parentRoot.transform);

        scrollview_B.GetComponent<RectTransform>().transform.localScale = new Vector3(1f, 1f, 1f);

    }
}
