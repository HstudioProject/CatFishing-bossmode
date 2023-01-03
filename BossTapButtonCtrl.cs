using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTapButtonCtrl : MonoBehaviour
{
    public GameObject scrollview_A;
    public GameObject scrollview_B;

    //public GameObject IconReturn; // C ��ư�� ������ ������

    [Header("[�� ��ư]")]
    public GameObject Tap_A;
    public GameObject Tap_B;
    public Text Text_A;
    public Text Text_B;

    public GameObject fakeRoot;
    public GameObject parentRoot;

    [Header("[�� ��ư �÷�]")]
    public Color[] ColorList;
    public Color[] TextColor;

    //�̱��� �ν��Ͻ� ����
    public static BossTapButtonCtrl instance = null;

    void Awake()
    {
        //�̱��� �ν��Ͻ� �Ҵ�
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
