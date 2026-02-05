using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // components
    public List<GameObject> prefabList;

    private TMP_Text fingerTMP;
    private TMP_Text gestureTMP;
    private TMP_Text prefabTMP;
    private TMP_Text planeTMP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fingerTMP = GameObject.Find("Text (TMP) (1)").GetComponent<TMP_Text>();
        gestureTMP = GameObject.Find("Text (TMP) (2)").GetComponent<TMP_Text>();
        prefabTMP = GameObject.Find("Text (TMP) (3)").GetComponent<TMP_Text>();
        planeTMP = GameObject.Find("Text (TMP) (4)").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int tc = Input.touchCount;
        fingerTMP.text = tc.ToString();

        if (tc > 0)
        {
            Touch primerDedo = Input.GetTouch(0);
            string phaseName = "";
            switch (primerDedo.phase)
            {
                case TouchPhase.Began:
                    phaseName = "Pulsar";
                    break;
                case TouchPhase.Moved:
                    phaseName = "Mover";
                    break;
                case TouchPhase.Ended:
                    phaseName = "Soltar";
                    break;
                case TouchPhase.Stationary:
                    phaseName = "Mantener";
                    break;
                case TouchPhase.Canceled:
                    phaseName = "Cancelar";
                    break;
                default:
                    phaseName = "Estado inesperado";
                    break;
            }
            gestureTMP.text = phaseName;
        }
    }

    /**
    // metodo OnButtonPress
    public void OnButtonPress() { instantiatePrefab(); }

    // metodo instantiatePrefab
    private void instantiatePrefab()
    {
        List<ARRaycastHit> raycastHitList = new List<ARRaycastHit>();

        if (raycastManager.Raycast(
                Input.GetTouch(0).position,     // poscion del primer dedo
                //out// raycastHitList,         // lista vacia de posiciones
                TrackableType.Planes            // Tipo de colision: Planos
            )
        ) {
            Instantiate(
                prefab,                             // GameObject a instanciar
                raycastHitList[0].pose.position,    // posicion del primer raycastHit
                Quaternion.identity                 // quaternion de 'la identidad' (No preguntes)
            );
}
    }
    */
}
