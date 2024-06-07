using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Araba : MonoBehaviour
{
    public bool ilerle;
    public Transform parent;
    public GameObject[] tekerIzleri;

    public GameManager gameManager;

    bool durusNoktasiDurum = false;
    public GameObject parcPoint;
    float yukselmeDeger;
    bool platformYukselt;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (!durusNoktasiDurum)
        {
            transform.Translate(7f * Time.deltaTime * transform.forward);
        }
        if (ilerle)
        {
            transform.Translate(15f * Time.deltaTime * transform.forward);
        }

        if (platformYukselt)
        {
            if(yukselmeDeger > gameManager.platform1.transform.position.y)
            {
                gameManager.platform1.transform.position = Vector3.Lerp(gameManager.platform1.transform.position, new Vector3(gameManager.platform1.transform.position.x,
                gameManager.platform1.transform.position.y + 1.3f, gameManager.platform1.transform.position.z), 0.010f);
            }
            else
            {
                platformYukselt = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("parking"))
        {
            ArabaTeknikIslemi();
            transform.SetParent(parent);
            gameManager.YeniArabaGetir();
            if (gameManager.yukselecekPlatformVarmi)
            {
                yukselmeDeger = gameManager.platform1.transform.position.y + 1.3f;
                platformYukselt = true;
            }
        }
        else if (collision.gameObject.CompareTag("araba"))
        {
            gameManager.carpmaEfekti.transform.position = parcPoint.transform.position;
            gameManager.carpmaEfekti.Play();
            ArabaTeknikIslemi();
            gameManager.Kaybettin();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("durusNoktasi"))
        {
            durusNoktasiDurum = true;
        }
        else if (other.gameObject.CompareTag("Elmas"))
        {
            other.gameObject.SetActive(false);
            gameManager.elmasSayisi++;
            gameManager.sesler[0].Play();
        }
        else if (other.gameObject.CompareTag("ortaGobek"))
        {
            gameManager.carpmaEfekti.transform.position = parcPoint.transform.position;
            gameManager.carpmaEfekti.Play();
            ArabaTeknikIslemi();
            gameManager.Kaybettin();
        }
        else if (other.CompareTag("onParking"))
        {
            other.gameObject.GetComponent<OnParking>().ParkingAktiflestir();
        }
    }

    void ArabaTeknikIslemi()
    {
        ilerle = false;
        tekerIzleri[0].SetActive(false);
        tekerIzleri[1].SetActive(false);
    }
}
