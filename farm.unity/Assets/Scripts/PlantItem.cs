using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    public PlantObject plant;

    public TMP_Text nameTxt;
    public TMP_Text priceTxt;
    public Image icon;

    public Image btnImage;
    public TMP_Text btnTxt;

    private FarmManager _fm;

    private void Start()
    {
        _fm = FindObjectOfType<FarmManager>();
        InitializeUi();
    }

    public void BuyPlant()
    {
        Debug.Log("Bought " + plant.plantName);
        _fm.SelectPlant(this);
    }

    private void InitializeUi()
    {
        nameTxt.text = plant.plantName;
        priceTxt.text = "$" + plant.buyPrice;
        icon.sprite = plant.icon;
    }
}