using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartEslestirme : MonoBehaviour
{
    //Oyun ekranının yukarısındaki textle alakalı
    public GameObject TurPrefab;
    public GameObject panel;
    public Text turMetini;
    public int metninXpozisyonu = 302;
    public int metninYpozisyonu = 275;

    int turSayisi = 2;
    
    //Buttonlarla alakalı
    public GameObject buttonPrefab_1;
    public GameObject buttonPrefab_2;

    public Sprite[] kartRenkleri;

    public int buttonSayisi = 10;
    public GameObject[] soldakiButtonlar;
    public GameObject[] sagdakiButtonlar;

    public int solTarafBaslangicX = -263;
    public int solTarafBaslangicY = 201;

    public int sagTarafBaslangicX = 140;
    public int sagTarafBaslangicY = 201;

    public int sagaKaydirmaMiktari = 150;
    public int asagiKaydirmaMiktari = -115;

    public bool ikinciSutun;
    bool ikinciKartGrubu;
    public int xPozisyonu;
    public int yPozisyonu;

    //Kutuların içlerindeki değerlerin tutulmasıyla alakalı
    public int[,] degiskenler = new int[3, 10];
    public int[,] karisikSiradaDegiskenler = new int[3, 10];

    //Kutuların eşleştirilmesinin kontrolüyle alakalı
    public GameObject ilk;
    public int DogruYanit;
    public int ilkIndex;
    public int ikinciIndex;
    int ButtonSayisi = 10;
    bool tiklandimi;

    // Start is called before the first frame update
    void Start()
    {
        yaziyiEkle(panel);
        ButtonlariAyarla();
        ButtonlariAyarla();
    }

    public void yaziyiEkle(GameObject parentObj)
    {
        var obj = Instantiate(TurPrefab);
        obj.transform.SetParent(parentObj.GetComponent<RectTransform>(), false);
        RectTransform myRectTransform = obj.GetComponent<RectTransform>();
        myRectTransform.localPosition = new Vector3(metninXpozisyonu, metninYpozisyonu, 0);
        turMetini = obj.GetComponent<Text>();
    }

    public void turDusur()
    {
        turSayisi--;
        turMetini.text = "Kalan Tur: " + turSayisi;
        if (turSayisi != 0)
        {
            ButtonlariAyarla();
            ButtonlariAyarla();
        }
    }

    public void renkleriKaristir(Sprite[] renkDizisi)
    {
        for (int t = 0; t < renkDizisi.Length; t++)
        {
            var tmp = renkDizisi[t];
            int r = Random.Range(t, renkDizisi.Length);
            renkDizisi[t] = renkDizisi[r];
            renkDizisi[r] = tmp;
        }
    }

    public void ButtonlariAyarla()
    {
        renkleriKaristir(kartRenkleri);
        if (!ikinciKartGrubu)
        {
            xPozisyonu = solTarafBaslangicX;
            yPozisyonu = solTarafBaslangicY;
            ButtonlariOlustur(panel, buttonPrefab_1);
            sayilariYerlestir(soldakiButtonlar);
            ikinciKartGrubu = true;
        }
        else
        {
            xPozisyonu = sagTarafBaslangicX;
            yPozisyonu = sagTarafBaslangicY;
            ButtonlariOlustur(panel, buttonPrefab_2);
            metniDuzenle(sagdakiButtonlar);
            ikinciKartGrubu = false;
        }
    }

    public void ButtonlariOlustur(GameObject parentObj, GameObject uygunPrefab)
    {
        for (int i = 0; i < buttonSayisi; i++)
        {
            var obj = Instantiate(uygunPrefab);
            obj.transform.SetParent(parentObj.GetComponent<RectTransform>(), false);
            RectTransform myRectTransform = obj.GetComponent<RectTransform>();
            myRectTransform.localPosition = new Vector3(xPozisyonu, yPozisyonu, 0);
            obj.GetComponent<Image>().sprite = kartRenkleri[i];
            // Sıradaki adım için gereken pozisyon işlemleri
            if (!ikinciSutun)
            {
                xPozisyonu += sagaKaydirmaMiktari;
                ikinciSutun = true;
            }
            else
            {
                xPozisyonu -= sagaKaydirmaMiktari;
                ikinciSutun = false;
                yPozisyonu += asagiKaydirmaMiktari;
            }
            //Objeleri arrayde tut ve bu arraylere göre tıklanma özelliği ekle
            if (!ikinciKartGrubu)
            {
                soldakiButtonlar[i] = obj;
                obj.GetComponent<Button>().onClick.AddListener(() => eslesmeControl(obj, System.Array.IndexOf(soldakiButtonlar, obj)));
            }
            else
            {
                sagdakiButtonlar[i] = obj;
                obj.GetComponent<Button>().onClick.AddListener(() => eslesmeControl(obj, System.Array.IndexOf(sagdakiButtonlar, obj)));
            }
        }
    }

    // Bu Fonksiyon sol taraftaki kartlar için
    public void sayilariYerlestir(GameObject[] Buttonlar)
    {
        for (int i = 0; i < Buttonlar.Length; i++)
        {
            degiskenler[0, i] = Random.Range(1, 10);
            degiskenler[1, i] = Random.Range(1, 10);
            degiskenler[2, i] = degiskenler[0, i] + degiskenler[1, i];
            Buttonlar[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = degiskenler[0, i] + "+" + degiskenler[1, i] + "=" + degiskenler[2, i];
        }
    }

    //Bu Fonksiyon sağ taraftaki kartlar için
    public void metniDuzenle(GameObject[] yeniButtonlar)
    {
        //Kutuların sırasını değiştirme
        kartlariKaristir();
        for (int i = 0; i < yeniButtonlar.Length; i++)
        {
            //Toplamadaki rakamların sırasını rastgele değiştirme
            var siraBelirleyici = Random.Range(1, 3);
            if (siraBelirleyici % 2 == 1)
            {
                yeniButtonlar[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = karisikSiradaDegiskenler[0, i] + "";
                yeniButtonlar[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = karisikSiradaDegiskenler[1, i] + "";
            }
            else
            {
                yeniButtonlar[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = karisikSiradaDegiskenler[0, i] + "";
                yeniButtonlar[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = karisikSiradaDegiskenler[1, i] + "";
            }
            yeniButtonlar[i].transform.GetChild(2).gameObject.GetComponent<Text>().text = "+";
            yeniButtonlar[i].transform.GetChild(3).gameObject.GetComponent<Text>().text = "____";
            yeniButtonlar[i].transform.GetChild(4).gameObject.GetComponent<Text>().text = karisikSiradaDegiskenler[2, i] + "";
        }
    }

    public void kartlariKaristir()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                karisikSiradaDegiskenler[i, j] = degiskenler[i, j];
            }
        }
        //karisikSiradaDegiskenler = degiskenler;
        Random rand = new Random();
        //Indislerin sırasını değiştir
        for (int j = 0; j < 10; j++)
        {
            int[] tmp = new int[3];
            for (var i = 0; i < tmp.Length; i++)
            {
                tmp[i] = karisikSiradaDegiskenler[i, j];
            }
            int r = Random.Range(j, 10);
            for (var i = 0; i < tmp.Length; i++)
            {
                karisikSiradaDegiskenler[i, j] = karisikSiradaDegiskenler[i, r];
                karisikSiradaDegiskenler[i, r] = tmp[i];
            }
        }
    }

    public void eslesmeControl(GameObject tiklanan, int index)
    {
        if (tiklandimi == false)
        {
            ilk = tiklanan;
            tiklandimi = true;
            ilkIndex = index;
        }
        else
        {
            ikinciIndex = index;
            //Aynı gruptan kart seçmediğini kontrol et
            if (tiklanan.tag != ilk.tag)
            {
                //Önce sol gruptaki kartı sonra sağ gruptaki kartı seçmişse
                if (ilk.tag != "Sag")
                {
                    //sol gruptaki sonuç == sağ gruptaki sonuç
                    //sol gruptaki ilk değişken == sağ gruptaki ilk değişken veya sağ gruptaki ikinci değişken
                    //yukardaki iki koşul da aynı anda sağlanıcak yani  ((a == b) && ((c == d) || (c == e)))
                    if (((degiskenler[2, ilkIndex]) == (karisikSiradaDegiskenler[2, ikinciIndex]))
                        &&
                        ((degiskenler[0, ilkIndex]) == (karisikSiradaDegiskenler[0, ikinciIndex]))
                        ||
                        ((degiskenler[0, ilkIndex]) == (karisikSiradaDegiskenler[1, ikinciIndex]))
                        )
                    {
                        Destroy(tiklanan);
                        Destroy(ilk);
                        tiklandimi = false;
                        DogruYanit++;
                    }
                    else
                    {
                        tiklanan = null;
                        tiklandimi = false;
                    }
                }
                //Önce sağ gruptaki kartı sonra sol gruptaki kartı seçmişse
                else if (ilk.tag != "Sol")
                {
                    //sol gruptaki sonuç == sağ gruptaki sonuç
                    //sol gruptaki ilk değişken == sağ gruptaki ilk değişken veya sağ gruptaki ikinci değişken
                    //yukardaki iki koşul da aynı anda sağlanıcak yani  ((a == b) && ((c == d) || (c == e)))
                    if (((karisikSiradaDegiskenler[2, ilkIndex]) == (degiskenler[2, ikinciIndex]))
                        &&
                       ((karisikSiradaDegiskenler[0, ilkIndex]) == (degiskenler[0, ikinciIndex]))
                       ||
                       ((karisikSiradaDegiskenler[0, ilkIndex]) == (degiskenler[1, ikinciIndex]))
                       )
                    {
                        Destroy(tiklanan);
                        Destroy(ilk);
                        tiklandimi = false;
                        DogruYanit++;
                    }
                    else
                    {
                        tiklanan = null;
                        tiklandimi = false;
                    }
                }
            }
            else
            {
                tiklanan = null;
                tiklandimi = false;
            }
            ilk = null;
            if (DogruYanit == ButtonSayisi)
            {
                DogruYanit = 0;
                turDusur();
            }
        }

    }

}
