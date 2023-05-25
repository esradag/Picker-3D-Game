using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class TopAlaniTeknikislemler
{
    public Animator TopAlaniAsansor;
    public TextMeshProUGUI SayiText;
    public int AtilmasiGerekenTop;
    public GameObject[] Toplar;

}

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject ToplayiciObje;
    [SerializeField] private GameObject TopKontrolObjesi;
    public bool ToplayiciHareketDurumu;

    int AtilanTopSayisi;
    int ToplamCheckPointSayisi;
    int MevcutCheckPointIndex;

    [SerializeField] private List<TopAlaniTeknikislemler> _TopAlaniTeknikislemler = new List<TopAlaniTeknikislemler>();

   

    // Start is called before the first frame update
    void Start()
    {
        ToplayiciHareketDurumu = true;
        for(int i=0; i<_TopAlaniTeknikislemler.Count; i++)
        {
            _TopAlaniTeknikislemler[i].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikislemler[i].AtilmasiGerekenTop;
        }

        ToplamCheckPointSayisi = _TopAlaniTeknikislemler.Count - 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ToplayiciHareketDurumu)
        {
            ToplayiciObje.transform.position += 5f * Time.deltaTime * ToplayiciObje.transform.forward;
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3
                    (ToplayiciObje.transform.position.x - .1f, ToplayiciObje.transform.position.y,
                    ToplayiciObje.transform.position.z), .05f);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3
                    (ToplayiciObje.transform.position.x + .1f, ToplayiciObje.transform.position.y,
                    ToplayiciObje.transform.position.z), .05f);
            }
        }
    }

    public void SiniraGelindi()
    {
        ToplayiciHareketDurumu = false;
        Invoke("AsamaKontrol", 2f);

        Collider[] HitColl = Physics.OverlapBox(TopKontrolObjesi.transform.position, TopKontrolObjesi.transform.localScale /
            2, Quaternion.identity);

        int i = 0;
        while(i < HitColl.Length)
        {
            HitColl[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, .8f), ForceMode.Impulse);
            i++;

        }

       
    }

    public void ToplariSay()
    {
        AtilanTopSayisi++;
        _TopAlaniTeknikislemler[MevcutCheckPointIndex].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikislemler[MevcutCheckPointIndex].AtilmasiGerekenTop;
    }
    void AsamaKontrol()
    {
        if(AtilanTopSayisi >= _TopAlaniTeknikislemler[MevcutCheckPointIndex].AtilmasiGerekenTop)
        {
            _TopAlaniTeknikislemler[MevcutCheckPointIndex].TopAlaniAsansor.Play("Asansor");

            

            foreach(var item in _TopAlaniTeknikislemler[MevcutCheckPointIndex].Toplar)
            {
                item.SetActive(false);
            }


            if (MevcutCheckPointIndex == ToplamCheckPointSayisi)
            {
                Debug.Log("OyunBitti-KAZANDIN PANELİ ORTAYA ÇIKABİLİR");
                Time.timeScale = 0;
            } else
            {
            MevcutCheckPointIndex++;
            AtilanTopSayisi = 0;
            
            }

    }
    else
    {
        Debug.Log("kaybettin");
     }
    }
}