using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    
    public List<CartClass> Carts = new List<CartClass>();
    public List<Button> ActivateButtons = new List<Button>();

    public Button b_Uzb;
    public Button b_Rus;
    public Button b_Eng;
    public Button b_Arab;
    
    public int CurrentLang;
    public float SpeedAnim = 1f;
    
    public TMP_TextJuicer TextJuicer;
    public List<string> LangList = new List<string>();
    
    public TMP_TextJuicer YouHereJuicer;
    public List<string> LangYouHere = new List<string>();
    
    public TMP_TextJuicer ChoseZone;
    public List<string> LangChoseZone = new List<string>();

    public GameObject AnimHand;

    private bool _animButtons;
    private Color _color;
    private StandbyClass _standbyClass;
    [HideInInspector] public CartClass CurrentCart;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        CurrentLang = 0;
        _standbyClass = FindObjectOfType<StandbyClass>(true);
        _color = b_Uzb.image.color;
        b_Uzb.onClick.AddListener(OnUzb);
        b_Rus.onClick.AddListener(OnRus);
        _animButtons = true;
        Carts = GetComponentsInChildren<CartClass>(true).ToList();
        foreach (var cart in Carts)
        {
            cart.Init();
        }
        OnAllButton();
        _standbyClass.Init();
        StartCoroutine(AnimActivateButtons());
        StartCoroutine(ChangeLang(b_Uzb.image));
    }

    public void ChangeLang()
    {
        CurrentLang++;
        if (CurrentLang >= 2)
            CurrentLang = 0;
        OffLangButtons();
        OffStandby();
        if (CurrentLang == 0)
            StartCoroutine(ChangeLang(b_Uzb.image));
        if(CurrentLang == 1)
            StartCoroutine(ChangeLang(b_Rus.image));
    }

    public void OffAllButton()
    {
        foreach (var button in ActivateButtons)
        {
            button.enabled = false;
        }
    }

    public void OnAllButton()
    {
        foreach (var button in ActivateButtons)
        {
            button.enabled = true;
        }
    }

    public void ActivateAnimButtons()
    {
        _animButtons = true;
    }

    public void DeactivateAnimButtons()
    {
        _animButtons = false;
    }

    IEnumerator AnimActivateButtons()
    {
        while (true)
        {
            if (!_animButtons)
            {
                yield return null;
                continue;
            }

            foreach (var button in ActivateButtons)
            {
                button.transform.DOPunchScale(button.transform.localScale * 0.2f, 1f, 1);
                yield return new WaitForSeconds(2f);
            }
            
        }
    }

    public void OffLangButtons()
    {
        b_Rus.enabled = false;
        b_Eng.enabled = false;
        b_Uzb.enabled = false;
        b_Arab.enabled = false;
    }

    public void OnLangButtons()
    {
        b_Rus.enabled = true;
        b_Eng.enabled = true;
        b_Uzb.enabled = true;
        b_Arab.enabled = true;
    }

    private void OnUzb()
    {
        OffLangButtons();
        CurrentLang = 0;
        OffStandby();
        StartCoroutine(ChangeLang(b_Uzb.image));
        _standbyClass.Hide();
    }

    private void OnRus()
    {
        OffLangButtons();
        CurrentLang = 1;
        OffStandby();
        StartCoroutine(ChangeLang(b_Rus.image));
        _standbyClass.Hide();
    }

    IEnumerator ChangeLang(Image image)
    {
        if (CurrentCart != null && CurrentCart.gameObject.activeSelf)
        {
            CurrentCart.Show();
        }

        b_Rus.enabled = false;
        b_Uzb.enabled = false;
        b_Eng.enabled = false;
        b_Arab.enabled = false;
        TextJuicer.Text = LangList[CurrentLang];
        YouHereJuicer.Text = LangYouHere[CurrentLang];
        ChoseZone.Text = LangChoseZone[CurrentLang];
        TextJuicer.SetProgress(0f);
        TextJuicer.Update();
        YouHereJuicer.SetProgress(0f);
        YouHereJuicer.Update();
        ChoseZone.SetProgress(0f);
        ChoseZone.Update();
        float progress = 0f;
        _color.a = 0;
        b_Uzb.image.color = _color;
        b_Rus.image.color = _color;
        b_Eng.image.color = _color;
        b_Arab.image.color = _color;
        while (progress<1f)
        {
            progress += Time.deltaTime * SpeedAnim*10f;
            TextJuicer.SetProgress(progress);
            TextJuicer.Update();
            YouHereJuicer.SetProgress(progress);
            YouHereJuicer.Update();
            ChoseZone.SetProgress(progress);
            ChoseZone.Update();
            _color.a = progress;
            image.color = _color;
            yield return null;
        }
        b_Rus.enabled = true;
        b_Uzb.enabled = true;
        b_Eng.enabled = true;
        b_Arab.enabled = true;
    }

    public void OnStandby()
    {
        ChoseZone.gameObject.SetActive(false);
        TextJuicer.gameObject.SetActive(false);
        AnimHand.gameObject.SetActive(false);
    }

    public void OffStandby()
    {
        ChoseZone.gameObject.SetActive(true);
        TextJuicer.gameObject.SetActive(true);
        AnimHand.gameObject.SetActive(true);
    }

}
