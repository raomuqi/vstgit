using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIVRChair : MonoBehaviour {
    public Slider[] sliderArray;
    public Text recv;
    public Toggle fanToggle;
    public Button SetAttitudeBtn;
    int ax_index=0;
    float ax_value=0;
    const string CONST_VALUE = "Value";
    Vector3 attitudeVector3= Vector3.zero;
    // Use this for initialization
    void Start ()
    {
        VRChairSDK.GetInstance().Init();
        VRChairSDK.GetInstance().RegisterBtnChangeCallback(OnBtnEvent);
        recv.text = "no data input..";
        sliderArray[0].onValueChanged.AddListener(SetRX);
        sliderArray[1].onValueChanged.AddListener(SetDY);
        sliderArray[2].onValueChanged.AddListener(SetRZ);

        sliderArray[3].onValueChanged.AddListener(SetAttitudeY);
        sliderArray[4].onValueChanged.AddListener(SetAttitudeX);
        sliderArray[5].onValueChanged.AddListener(SetAttitudeZ);

        sliderArray[6].onValueChanged.AddListener(OnAxIndex);
        sliderArray[7].onValueChanged.AddListener(OnAxValue);

        SetAttitudeBtn.onClick.AddListener(SetAttitude);

        fanToggle.onValueChanged.AddListener(OnFanToggle);
        
    }
    public void OnBtnEvent(byte index,byte status)
    {
        recv.text = "第" + index + "按钮状态：" + status;
    }
	// Update is called once per frame
	void Update () {
		
	}
     public void SetRX(float value)
    {
        VRChairSDK.GetInstance().SetRX(value);
        SetSliderValue(0);
    }

    public void SetDY(float value)
    {
        VRChairSDK.GetInstance().SetDY(value);
        SetSliderValue(1);
    }
    public void SetRZ(float value)
    {
        VRChairSDK.GetInstance().SetRZ(value);
        SetSliderValue(2);
    }

    public void SetAttitudeY(float value)
    {
        SetSliderValue(3);
        attitudeVector3.x = value;
    }

    public void SetAttitudeX(float value)
    {
        SetSliderValue(4);
        attitudeVector3.y = value;
    }
    public void SetAttitudeZ(float value)
    {
        attitudeVector3.z = value;
        SetSliderValue(5);
    }

    public void SetAttitude()
    {
        VRChairSDK.GetInstance().SetAttitude(attitudeVector3.x, attitudeVector3.y, attitudeVector3.z);
    }

  
    /// <summary>
    /// 风扇
    /// </summary>
    public void OnFanToggle(bool isOpen)
    {
        VRChairSDK.GetInstance().SetFan(isOpen);
    }

    public void OnAxIndex(float index)
    {
        this.ax_index = (int)index;
        SetSliderValue(6);
    }
    public void OnAxValue(float value)
    {
        this.ax_value = value;
        SetSliderValue(7);
        VRChairSDK.GetInstance().SetAx(this.ax_index, this.ax_value);
    }


    void SetSliderValue(int index)
    {
        Slider slider = sliderArray[index];
        slider.transform.Find(CONST_VALUE).GetComponent<Text>().text = slider.value.ToString();
    }
}
