using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class GameManager : MonoBehaviour
{
    // components
    public List<Sprite> spriteList;
    public List<GameObject> prefabList;

    private List<GameObject> instanceList;
    private GameObject currentPrefab;
    private TMP_Text fingerTMP;
    private TMP_Text gestureTMP;
    private TMP_Text instanceTMP;
    private TMP_Text planeTMP;
    private RawImage prefabBtnImage;
    private RawImage planeBtnImage;
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    // variables
    private int touchCount;
    private int prefabIndex;
    private int planeCount;
    private bool planeDetectionActivated;

    // events
    public void OnPrefabButtonPress() { changePrefab(); }

    public void OnPlaneButtonPress() { togglePlaneDetection(); }

    public void OnResetButtonPress() { resetInstances(); }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        touchCount = 0;
        prefabIndex = 0;
        planeCount = 0;
        planeDetectionActivated = true;
        raycastManager = new ARRaycastManager();

        instanceList = new List<GameObject>();
        currentPrefab = prefabList[prefabIndex];
        fingerTMP = GameObject.Find("Text (TMP) (1)").GetComponent<TMP_Text>();
        gestureTMP = GameObject.Find("Text (TMP) (2)").GetComponent<TMP_Text>();
        instanceTMP = GameObject.Find("Text (TMP) (3)").GetComponent<TMP_Text>();
        planeTMP = GameObject.Find("Text (TMP) (4)").GetComponent<TMP_Text>();
        prefabBtnImage = GameObject.Find("Btn_Prefab").GetComponentInChildren<RawImage>();
        planeBtnImage = GameObject.Find("Btn_Planes").GetComponentInChildren<RawImage>();
        raycastManager = GameObject.Find("XR Origin (Mobile AR)").GetComponent<ARRaycastManager>();
        planeManager = GameObject.Find("XR Origin (Mobile AR)").GetComponent<ARPlaneManager>();

        fingerTMP.text = Input.touchCount.ToString();
        gestureTMP.text = "";
        instanceTMP.text = instanceList.Count.ToString();
        planeTMP.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        touchCount = Input.touchCount;
        fingerTMP.text = touchCount.ToString();

        if (touchCount > 0)
        {
            Touch primerDedo = Input.GetTouch(0);
            string phaseName = "";
            switch (primerDedo.phase)
            {
                case UnityEngine.TouchPhase.Began:
                    phaseName = "Pulsar";
                    instancePrefab();
                    break;
                case UnityEngine.TouchPhase.Moved:
                    phaseName = "Mover";
                    break;
                case UnityEngine.TouchPhase.Ended:
                    phaseName = "Soltar";
                    break;
                case UnityEngine.TouchPhase.Stationary:
                    phaseName = "Mantener";
                    break;
                case UnityEngine.TouchPhase.Canceled:
                    phaseName = "Cancelar";
                    break;
                default:
                    phaseName = "Estado inesperado";
                    break;
            }
            gestureTMP.text = phaseName;
        }
    }

    // actions
    private void instancePrefab()
    {
        List<ARRaycastHit> raycastHitList = new List<ARRaycastHit>();
        bool hitted = raycastManager.Raycast(
                            Input.GetTouch(0).position,     // poscion del primer dedo
                            raycastHitList,                 // lista vacia de posiciones
                            TrackableType.Planes            // Tipo de colision: Planos
                        );

        if (hitted)
        {
            Instantiate(
                currentPrefab,                      // GameObject a instanciar
                raycastHitList[0].pose.position,    // posicion del primer raycastHit
                Quaternion.identity                 // quaternion de 'la identidad' (No preguntes)
            );
            instanceList = GameObject.FindGameObjectsWithTag("Prefab").ToList<GameObject>();
            instanceTMP.text = instanceList.Count.ToString();
        }
    }

    private void changePrefab()
    {
        prefabIndex = (prefabIndex == (prefabList.Count - 1)) ? 0 : prefabIndex + 1;
        currentPrefab = prefabList[prefabIndex];
        prefabBtnImage.texture = spriteList[prefabIndex].texture;
    }

    private void togglePlaneDetection()
    {
        if (planeDetectionActivated)
        {
            planeManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
            GameObject trackables = GameObject.Find("Trackables");
            foreach (GameObject go in trackables.transform)
            {
                Debug.Log(go.name);
            }
        }
        else
        {
            planeManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;
        }

        planeDetectionActivated = !planeDetectionActivated;

        planeBtnImage.texture = (planeDetectionActivated) ? spriteList[3].texture : spriteList[4].texture ;
    }

    private void resetInstances()
    {
        if (instanceList.Count > 0)
        {
            foreach (GameObject instance in instanceList) { Destroy(instance); }
            instanceList.Clear();
            instanceTMP.text = instanceList.Count.ToString();
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
