using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabManager : MonoBehaviour
{
    [SerializeField] Button volumeTabButton;
    [SerializeField] Button controlsTabButton;

    [SerializeField] List<GameObject> tabs = new List<GameObject>();

    [SerializeField] bool isVolumeTabActive = true;
    [SerializeField] bool isControlsTabActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if (volumeTabButton == null)
        {
            volumeTabButton = GameObject.Find("VolumeTabButton").GetComponent<Button>();
        }

        if (controlsTabButton == null)
        {
            controlsTabButton = GameObject.Find("ControlsTabButton").GetComponent<Button>();
        }

        volumeTabButton.gameObject.GetComponent<Image>().color = Color.white;
        tabs[0].SetActive(true);

        controlsTabButton.gameObject.GetComponent<Image>().color = Color.red;
        tabs[1].SetActive(false);
    }

    public void SelectVolumeTab()
    {
        if (!isVolumeTabActive && isControlsTabActive)
        {
            volumeTabButton.gameObject.GetComponent<Image>().color = Color.white;
            tabs[0].SetActive(true);

            controlsTabButton.gameObject.GetComponent<Image>().color = Color.red;
            tabs[1].SetActive(false);

            isVolumeTabActive = true;
            isControlsTabActive = false;
        }
    }

    public void SelectControlsTab()
    {
        if (!isControlsTabActive && isVolumeTabActive)
        {
            controlsTabButton.gameObject.GetComponent<Image>().color = Color.white;
            tabs[1].SetActive(true);

            volumeTabButton.gameObject.GetComponent<Image>().color = Color.red;
            tabs[0].SetActive(false);

            isControlsTabActive = true;
            isVolumeTabActive = false;
        }
    }
}
