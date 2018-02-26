using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiController : MonoBehaviour {

    int bf = 10;
    uint nf = 0;
    float lastWidth = 0;
    float lastHeight = 0;

    InputField NumFrom;
    Dropdown BaseFrom;
    Canvas Canvas;

    List<char> RestrNumFrom = new List<char>();

    void Start () {
        NumFrom = GameObject.Find("NumFrom").GetComponent<InputField>();
        BaseFrom = GameObject.Find("BaseFrom").GetComponent<Dropdown>();
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        bf = int.Parse(BaseFrom.options[BaseFrom.value].text);
        UpdateRestriction(RestrNumFrom, bf);
        NumFrom.onValidateInput += delegate (string input, int charIndex, char addedChar) { return ValidateInput(addedChar, RestrNumFrom); };
    }

    private void Update()
    {
        RectTransform crect = Canvas.transform as RectTransform;
        if (crect.rect.height != lastHeight || crect.rect.width != lastWidth)
        {
            lastHeight = crect.rect.height;
            lastWidth = crect.rect.width;
            UpdateDropdownSize();
        }
    }

    public void OnNumFromChanged()
    {
        nf = Get10BaseNum(NumFrom, bf);
        //Debug.Log("Num from " + nf.ToString());
    }

    public void OnBaseFromChanged()
    {
        int newbf = int.Parse(BaseFrom.options[BaseFrom.value].text);
        if (newbf != bf)
        {
            bf = newbf;
            UpdateRestriction(RestrNumFrom, bf);
            NumFrom.text = Convert.ToString(nf, bf);
        }
        
        //Debug.Log("Base from " + bf.ToString());
    }

    private uint Get10BaseNum(InputField input, int fromBase)
    {
        return input.text.Length > 0 ? Convert.ToUInt32(input.text, fromBase) : 0;
    }

    private char ValidateInput(char charToValidate, List<char> list)
    {
        if (list.IndexOf(charToValidate) > -1)
            return charToValidate;
        return '\0';
    }

    private void UpdateRestriction(List<char> list, int baseNum)
    {
        list.Clear();
        for (int i=0; i<baseNum; i++)
            list.Add(Convert.ToString(i, baseNum)[0]);        
    }

    private void UpdateDropdownSize()
    {
        RectTransform crect = Canvas.transform as RectTransform;
        RectTransform nrect = NumFrom.transform as RectTransform;
        Debug.Log("Screen height " + Screen.height + "Canvas height " + crect.rect.height.ToString() + " NumFrom y " + nrect.rect.y.ToString() + " height " + nrect.rect.height.ToString());
        float baseHeight = crect.rect.height - nrect.rect.height - 30;
        BaseFrom.template.sizeDelta = new Vector2(0, baseHeight);
    }

}
