using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using DG.Tweening;
public class ARController : MonoBehaviour
{
    [SerializeField] private GameObject AR;
    [SerializeField] private RawImage iconClima;
    [SerializeField] private TextMeshProUGUI cuidad;
    [SerializeField] private TextMeshProUGUI pais;
    [SerializeField] private TextMeshProUGUI temperatura;
    [SerializeField] private TextMeshProUGUI clima;
    [SerializeField] private RectTransform panelInput;
    [SerializeField] private InputField inputCuidad;
    
    private string url_api = "http://api.weatherstack.com/current?access_key=290e7bc98d0654fc98db16ad7f3fa85e&query=";
    private string cuidadActual = "Lima";
    private string url_img;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ObtenerInfo(cuidadActual));
    }

    IEnumerator ObtenerInfo(string city)
    {
        UnityWebRequest www = UnityWebRequest.Get(url_api + city);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            JSONNode info = JSON.Parse(www.downloadHandler.text);
            string cuidadNombre = info["location"]["name"];
            string paisNombre = info["location"]["country"];
            string temp = info["current"]["temperature"];
            string image = info["current"]["weather_icons"][0];
            string climaNombre = info["current"]["weather_descriptions"][0];
            string isDay = info["current"]["is_day"];
            
            Debug.Log(image);
            UnityWebRequest img = UnityWebRequestTexture.GetTexture(image);
            Debug.Log(img);
            yield return img.SendWebRequest();

            if (isDay == "yes")
            {
                iconClima.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(img);
            }
            else
            {
                iconClima.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(img);
            }

            cuidad.text = cuidadNombre;
            pais.text = paisNombre;
            temperatura.text = temp + " °C";
            clima.text = climaNombre;

            AR.SetActive(true);
        }
    }

    public void MostrarPanelInput()
    {
        panelInput.DOAnchorPos(new Vector2(0.0f, -232.0f), 0.5f);
    }

    public void BuscarCuidad()
    {
        StartCoroutine(ObtenerInfo(inputCuidad.text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


