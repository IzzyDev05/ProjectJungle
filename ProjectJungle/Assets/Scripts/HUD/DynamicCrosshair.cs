using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    public static DynamicCrosshair Instance;

    [SerializeField] Transform mainCam;
    [SerializeField] Transform aimCam;
    [SerializeField] float grappleDistance;

    [SerializeField] Image crosshair;
    [SerializeField] Color defaultColor;
    [SerializeField] Color grappleColor;

    [SerializeField] LayerMask grappleLayer;
    [SerializeField] LayerMask ignoredLayer;
 
    private void Awake()
    {
        #region SINGLETON
        if (Instance != null)
        {
            Debug.LogError("Multiple Dynamic Crosshair Instances found.");
        }

        Instance = this;
        #endregion

        if (crosshair == null)
        {
            crosshair = gameObject.GetComponent<Image>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
        defaultColor = crosshair.color;

        if(ignoredLayer.value == 0)
        {
            ignoredLayer = LayerMask.NameToLayer("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectGrapplePoint();
    }

    void DetectGrapplePoint()
    {
        Vector3 camAim = new Vector3(mainCam.forward.x, mainCam.forward.y, mainCam.forward.z);
        Ray ray = new Ray(aimCam.position, camAim);

        if (Physics.Raycast(ray, out RaycastHit hit, grappleDistance, grappleLayer) == true)
        {
            crosshair.color = grappleColor;
        }
        else
        {
            crosshair.color = defaultColor;
        }

    }

    
    public void SetupDynamicCrosshair(LayerMask layer, Transform aimCameraTransform, float distance)
    {
        if (grappleLayer.value == 0) 
        {
            grappleLayer = layer;
        }

        aimCam = aimCameraTransform;
        grappleDistance = distance;
    }
}
