using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
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
    public string uzb;
    public string rus;
    public List<string> LangList = new List<string>();

    private bool _animButtons;
    private Color _color;
    private StandbyClass _standbyClass;
    public CartClass CurrentCart;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        _standbyClass = FindObjectOfType<StandbyClass>(true);
        _color = b_Uzb.image.color;
        b_Uzb.onClick.AddListener(OnUzb);
        b_Rus.onClick.AddListener(OnRus);
        OnRus();
        _animButtons = true;
        Carts = GetComponentsInChildren<CartClass>(true).ToList();
        foreach (var cart in Carts)
        {
            cart.Init();
        }
        OnAllButton();
        _standbyClass.Init();
        StartCoroutine(AnimActivateButtons());
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

    private void OnUzb()
    {
        CurrentLang = 0;
        StartCoroutine(ChangeLang(b_Uzb.image));
    }

    private void OnRus()
    {
        CurrentLang = 1;
        StartCoroutine(ChangeLang(b_Rus.image));
    }

    IEnumerator ChangeLang(Image image)
    {
        b_Rus.enabled = false;
        b_Uzb.enabled = false;
        b_Eng.enabled = false;
        b_Arab.enabled = false;
        TextJuicer.Text = LangList[CurrentLang];
        TextJuicer.SetProgress(0f);
        TextJuicer.Update();
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
            _color.a = progress;
            image.color = _color;
            yield return null;
        }
        b_Rus.enabled = true;
        b_Uzb.enabled = true;
        b_Eng.enabled = true;
        b_Arab.enabled = true;
    }


}
