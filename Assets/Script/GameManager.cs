using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("-----ARABA AYARLARI")]
    public GameObject[] arabalar;
    public int kacArabaOlsun;
    int kalanAracSayisiDegeri;
    int aktifAracIndex = 0;

    [Header("----CANVAS AYARLARI")]
    public GameObject[] arabaCanvasGorselleri;
    public Sprite aracGeldiGorseli;
    public Text[] Textler;
    public GameObject[] panellerim;
    public GameObject[] tapToButonlar;

    [Header("-----PLATFORM AYARLARI")]
    public GameObject platform1;
    public GameObject platform2;
    public float[] donusHizlari;
    bool donusVarmi;

    [Header("----LEVEL AYARLARI")]
    public int elmasSayisi;
    public ParticleSystem carpmaEfekti;
    public AudioSource[] sesler;
    public bool yukselecekPlatformVarmi;
    bool dokunmaKilidi;

    void Start()
    {
        VarsayilanDegerleriKontrolEt();

        kalanAracSayisiDegeri = kacArabaOlsun;

        for (int i = 0; i < kacArabaOlsun; i++)
        {
            arabaCanvasGorselleri[i].SetActive(true);
        }

        donusVarmi = true;
        dokunmaKilidi = true;
    }

    void Update()
    {
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                if (dokunmaKilidi)
                {
                    panellerim[0].SetActive(false);
                    panellerim[3].SetActive(true);
                    dokunmaKilidi = false;
                }
                else
                {
                    arabalar[aktifAracIndex].GetComponent<Araba>().ilerle = true;
                    aktifAracIndex++;
                }
            }
        }

        if (donusVarmi)
        {
            platform1.transform.Rotate(new Vector3(0, 0, donusHizlari[0]), Space.Self);
            if(platform2 != null)
            {
                platform2.transform.Rotate(new Vector3(0, 0, -donusHizlari[1]), Space.Self);
            }
        }
    }

    public void YeniArabaGetir()
    {
        kalanAracSayisiDegeri--;
        if (aktifAracIndex < kacArabaOlsun)
        {
            arabalar[aktifAracIndex].SetActive(true);
        }
        else
        {
            Kazandin();
        }
        arabaCanvasGorselleri[aktifAracIndex - 1].GetComponent<Image>().sprite = aracGeldiGorseli;
    }

    public void Kaybettin()
    {
        donusVarmi = false;
        Textler[6].text = PlayerPrefs.GetInt("elmas").ToString();
        Textler[7].text = SceneManager.GetActiveScene().name;
        Textler[8].text = (kacArabaOlsun - kalanAracSayisiDegeri).ToString();
        Textler[9].text = elmasSayisi.ToString();
        sesler[1].Play();
        sesler[3].Play();
        panellerim[1].SetActive(true);
        panellerim[3].SetActive(false);
        Invoke("KaybettinButonuOrtayaCikart", 2f);
    }

    void Kazandin()
    {
        PlayerPrefs.SetInt("elmas", PlayerPrefs.GetInt("elmas") + elmasSayisi);
        Textler[2].text = PlayerPrefs.GetInt("elmas").ToString();
        Textler[3].text = SceneManager.GetActiveScene().name;
        Textler[4].text = (kacArabaOlsun - kalanAracSayisiDegeri).ToString();
        Textler[5].text = elmasSayisi.ToString();
        sesler[2].Play();
        panellerim[2].SetActive(true);
        panellerim[3].SetActive(false);
        Invoke("KazandinButonuOrtayaCikart", 2f);
    }

    void KaybettinButonuOrtayaCikart()
    {
        tapToButonlar[0].SetActive(true);
    }

    void KazandinButonuOrtayaCikart()
    {
        tapToButonlar[1].SetActive(true);
    }

    // BELLEK YONETIMI

    void VarsayilanDegerleriKontrolEt()
    {
        Textler[0].text = PlayerPrefs.GetInt("elmas").ToString();
        Textler[1].text = SceneManager.GetActiveScene().name;
    }

    public void IzleVeDevamEt()
    {
        // istersen reklam kodu yazabilirsin.
    }

    public void IzleVeDahaFazlaKazan()
    {
        // istersen reklam kodu yazabilirsin.
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SonrakiLevel()
    {
        PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
