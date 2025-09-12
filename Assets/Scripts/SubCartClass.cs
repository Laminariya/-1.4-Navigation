using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubCartClass : MonoBehaviour
{
    
    [HideInInspector] public TMP_TextJuicer NameText;
    [HideInInspector] public TMP_TextJuicer DiscriptionText;
    [HideInInspector] public Button b_Back;
    [HideInInspector] public Image CartImage;
    
    private CartClass _cartClass;
    
    public void Init(CartClass cartClass)
    {
        _cartClass = cartClass;
        CartImage = GetComponent<Image>();
        List<TMP_TextJuicer> TextList = GetComponentsInChildren<TMP_TextJuicer>(true).ToList();
        NameText = TextList[0];
        DiscriptionText = TextList[1];
        b_Back = GetComponentInChildren<Button>(true);
        Hide();
    }

    public void Show()
    {
        _cartClass.NameText = NameText;
        _cartClass.DiscriptionText = DiscriptionText;
        _cartClass.Cart = CartImage;
        _cartClass.b_Back = b_Back;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        CartImage.color = Color.clear;
        gameObject.SetActive(false);
    }

}
