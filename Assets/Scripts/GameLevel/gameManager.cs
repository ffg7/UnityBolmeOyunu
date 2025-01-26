using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {

    [SerializeField]
    private GameObject karePrefab;
    [SerializeField]
    private Transform karelerPaneli;
    [SerializeField]
    private Transform soruPaneli;
    [SerializeField]
    private Text soruText;
    [SerializeField]
    private GameObject sonucPaneli;
    private GameObject[] karelerDizisi = new GameObject[25];
    List<int> bolumDegerleriListesi = new List<int>();

    [SerializeField]
    private Sprite[] kareSprites;

    int bolunenSayi, bolenSayi;
    int kacinciSoru;
    int butonDegeri;
    bool butonaBasilsinmi;
    int dogruSonuc;
    int kalanHak;
    string sorununZorlukDerecesi;
    KalanHakManager kalanHakManager;
    puanManager puanManager;
    GameObject gecerliKare;

    [SerializeField]
    AudioSource audioSource;

    public AudioClip butonSesi;

    private void Awake()
    {
        kalanHak = 3;
        audioSource = GetComponent<AudioSource>();
        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;
        kalanHakManager = Object.FindObjectOfType<KalanHakManager>();
        kalanHakManager.KalanHaklariKontrolEt(kalanHak);
        puanManager = Object.FindObjectOfType<puanManager>();
    }
    void Start () {
        butonaBasilsinmi = false;
        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;
        kareleriOlustur();
	}
	
	
    public void kareleriOlustur()
    {
        for (int i = 0; i < 25; i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPaneli);
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)];
            kare.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            karelerDizisi[i] = kare;
        }
        BolumDegerleriniTexteYazdir();
        StartCoroutine(DoFadeRoutine());
        Invoke("SoruPaneliAc", 2f);
    }

    void ButonaBasildi()
    {
        if (butonaBasilsinmi)
        {
            audioSource.PlayOneShot(butonSesi);
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);
            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            SonucuKontrolEt();
        }
        
    }

    void SonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = "";
            gecerliKare.transform.GetComponent<Button>().interactable = false;
            puanManager.PuaniArtir(sorununZorlukDerecesi);

            bolumDegerleriListesi.RemoveAt(kacinciSoru);
            if (bolumDegerleriListesi.Count > 0)
            {
                SoruPaneliAc();
            }
            else
            {
                OyunBitti();
            }
            
        }
        else
        {
            kalanHak--;
            kalanHakManager.KalanHaklariKontrolEt(kalanHak);

        }
        if (kalanHak <= 0)
        {
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        butonaBasilsinmi = false;
        sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    IEnumerator DoFadeRoutine()
    {
        foreach (var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void BolumDegerleriniTexteYazdir()
    {
        foreach (var kare in karelerDizisi)
        {
            int rastgeleDeger = Random.Range(1, 13);
            bolumDegerleriListesi.Add(rastgeleDeger);
            kare.transform.GetChild(0).GetComponent<Text>().text = rastgeleDeger.ToString();
        }
    }

    void SoruPaneliAc()
    {

        SoruyuSor();
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
        butonaBasilsinmi = true;
    }
    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);
        kacinciSoru = Random.Range(0, bolumDegerleriListesi.Count);
        dogruSonuc = bolumDegerleriListesi[kacinciSoru];
        bolunenSayi = bolenSayi * dogruSonuc;
        if (bolunenSayi <= 40)
        {
            sorununZorlukDerecesi = "kolay";
        }
        else if (bolunenSayi > 40 && bolunenSayi <= 80)
        {
            sorununZorlukDerecesi = "orta";
        }
        else
        {
            sorununZorlukDerecesi = "zor";
        }
        soruText.text = bolunenSayi.ToString() + " / " + bolenSayi.ToString();
    }
}
