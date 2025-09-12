using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CartClass : MonoBehaviour
{

    public Image Filling;
    public Image Gradient;
    public Button b_Activate;

    [HideInInspector] public Image Cart;
    [HideInInspector] public TMP_TextJuicer NameText;
    [HideInInspector] public TMP_TextJuicer DiscriptionText;
    [HideInInspector] public Button b_Back;
    
    private Image _areolla;
    private List<SubCartClass> _subCarts = new List<SubCartClass>();
    
    public void Init()
    {
        _subCarts = GetComponentsInChildren<SubCartClass>(true).ToList();
        b_Activate.onClick.AddListener(Show);
        _areolla = b_Activate.transform.GetChild(0).GetComponent<Image>();
        _areolla.transform.localScale = Vector3.one/2f;
        foreach (var subCart in _subCarts)
        {
            subCart.Init(this);
            subCart.b_Back.onClick.AddListener(OnBack);
        }
        Hide();
    }

    public void Hide()
    {
        Filling.color = Color.clear;
        _subCarts[GameManager.instance.CurrentLang].Hide();
        if(b_Back != null)
            b_Back.gameObject.SetActive(false);
        Gradient.color = Color.clear;
        gameObject.SetActive(false);
        GameManager.instance.CurrentCart = null;
        GameManager.instance.OnAllButton();
    }

    private void Show()
    {
        GameManager.instance.CurrentCart = this;
        _subCarts[GameManager.instance.CurrentLang].Show();
        
        Cart.color = Color.clear;
        
        b_Back.gameObject.SetActive(false);
        GameManager.instance.OffAllButton();
        GameManager.instance.DeactivateAnimButtons();
        _areolla.transform.DOPunchScale(Vector3.one*0.5f, 0.4f, 1, 1f).OnComplete(ShowFill);
        b_Activate.transform.DOPunchScale(b_Activate.transform.localScale * 0.2f, 0.4f, 0, 1f);
        gameObject.SetActive(true);
        
        NameText.SetProgress(0f);
        NameText.Update();
        DiscriptionText.SetProgress(0f);
        DiscriptionText.Update();
    }

    private void ShowFill()
    {
        Filling.DOColor(Color.white, 0.5f).OnComplete(ShowGradient);
    }

    private void ShowGradient()
    {
        Gradient.color = new Color(1, 1f, 1f, 0f);
        Cart.DOColor(Color.white, 0.4f).SetDelay(0.1f).OnComplete(ShowCart);
        Gradient.DOColor(Color.white, 0.5f);
        Cart.gameObject.SetActive(true);
        StartCoroutine(ProgressName());
    }

    private void ShowCart()
    {
        
        StartCoroutine(ProgressAnimation());
    }

    IEnumerator ProgressName()
    {
        yield return new WaitForSeconds(0.5f);
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * GameManager.instance.SpeedAnim*6f;
            NameText.SetProgress(progress);
            NameText.Update();
            yield return null;
        }
    }

    IEnumerator ProgressAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * GameManager.instance.SpeedAnim * 2f;

            DiscriptionText.SetProgress(progress);
            DiscriptionText.Update();

            yield return null;
        }

        b_Back.gameObject.SetActive(true);
    }

    private void OnBack()
    {
        GameManager.instance.ActivateAnimButtons();
        b_Back.gameObject.SetActive(false);
        StartCoroutine(BackCoroutine());
    }

    IEnumerator BackCoroutine()
    {
        
        float progress = 1f;
        while (progress > 0f)
        {
            progress -= Time.deltaTime * GameManager.instance.SpeedAnim * 4f;

            NameText.SetProgress(progress);
            NameText.Update();
            DiscriptionText.SetProgress(progress);
            DiscriptionText.Update();

            yield return null;
        }

        Filling.DOColor(Color.clear, 0.4f);
        Gradient.DOColor(Color.clear, 0.4f);
        Cart.DOColor(Color.clear, 0.4f).OnComplete(Hide);
    }

}
