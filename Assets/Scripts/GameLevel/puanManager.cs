using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class puanManager : MonoBehaviour {

    int toplamPuan;
    int puanArtis;

    [SerializeField]
    Text puanText;
	void Start () {
        puanText.text = toplamPuan.ToString();
	}
    public void PuaniArtir(string zorlukSevitesi)
    {
        switch (zorlukSevitesi)
        {
            case "kolay":
                puanArtis = 5;
                break;

            case "orta":
                puanArtis = 10;
                break;

            case "zor":
                puanArtis = 15;
                break;
        }
        toplamPuan += puanArtis;
        puanText.text = toplamPuan.ToString();
    }
	
	
}
