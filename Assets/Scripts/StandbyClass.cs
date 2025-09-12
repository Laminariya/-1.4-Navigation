using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.TextJuicer;
using UnityEngine;
using UnityEngine.UI;

public class StandbyClass : MonoBehaviour
{
    
    private Button _button;
    
    public Image Gradient;
    public TMP_TextJuicer NameText;
    public TMP_TextJuicer DescriptionText;
    public TMP_TextJuicer HandText;
    public GameObject Hand;
    
    public List<string> Names = new List<string>();
    public List<string> Descriptions = new List<string>();
    public List<string> Hands = new List<string>();
    
    private float _time = 0f;
    private bool _isActive = false;
    
    public void Init()
    {
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(OnClick);
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive && Time.time - _time > 10f)
        {
            Show();
        }

        if (Input.anyKeyDown)
        {
            _time = Time.time;
        }
    }

    public void Show()
    {
        if(GameManager.instance.CurrentCart!=null)
            GameManager.instance.CurrentCart.Hide();
        _isActive = true;
        _button.gameObject.SetActive(false);
        Gradient.gameObject.SetActive(true);
        NameText.gameObject.SetActive(true);
        DescriptionText.gameObject.SetActive(true);
        HandText.gameObject.SetActive(true);
        //Hand.SetActive(true);
        StartCoroutine(ShowCoroutine());
    }

    public void Hide()
    {
        _button.gameObject.SetActive(false);
        StartCoroutine(HideCoroutine());
    }

    IEnumerator HideCoroutine()
    {
        NameText.Text = Names[GameManager.instance.CurrentLang];
        DescriptionText.Text = Descriptions[GameManager.instance.CurrentLang];
        HandText.Text = Hands[GameManager.instance.CurrentLang];
        Gradient.color = new Color(1f,1f,1f,1f);
        float progress = 1f;
        while (progress>0f)
        {
            progress -= Time.deltaTime*2f;
            NameText.SetProgress(progress);
            NameText.Update();
            DescriptionText.SetProgress(progress);
            DescriptionText.Update();
            HandText.SetProgress(progress);
            HandText.Update();
            yield return null;
        }
        Hand.SetActive(false);
        progress = 1f;
        while (progress>0f)
        {
            progress -= Time.deltaTime*2f;
            Gradient.color = new Color(1f,1f,1f,progress);
            yield return null;
        }
        
        _button.gameObject.SetActive(false);
        _isActive = false;
        _time = Time.time;
        Gradient.gameObject.SetActive(false);
        NameText.gameObject.SetActive(false);
        DescriptionText.gameObject.SetActive(false);
        HandText.gameObject.SetActive(false);
        Hand.SetActive(false);
    }

    IEnumerator ShowCoroutine()
    {
        NameText.Text = Names[GameManager.instance.CurrentLang];
        DescriptionText.Text = Descriptions[GameManager.instance.CurrentLang];
        HandText.Text = Hands[GameManager.instance.CurrentLang];
        Gradient.color = new Color(1f,1f,1f,0f);
        Hand.SetActive(false);
        float progress = 0f;
        while (progress<1f)
        {
            progress += Time.deltaTime*2f;
            NameText.SetProgress(progress);
            NameText.Update();
            DescriptionText.SetProgress(progress);
            DescriptionText.Update();
            HandText.SetProgress(progress);
            HandText.Update();
            Gradient.color = new Color(1f,1f,1f,progress);
            yield return null;
        }
        Hand.SetActive(true);
        _button.gameObject.SetActive(true);
    }

    private void OnClick()
    {
        Hide();
    }
}
