using Farm;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectPlant;
    public bool isPlanting = false;
    public int money=100;
    public TMP_Text moneyTxt;

    public Color buyColor = Color.green;
    public Color cancelColor = Color.red;

    public bool isSelecting = false;
    public PlotSelectionTool selectedTool = PlotSelectionTool.None;

    public Image[] buttonsImg;
    public Sprite normalButton;
    public Sprite selectedButton;

    private void Start()
    {
        moneyTxt.text = "$" + money;
    }

    public void SelectPlant(PlantItem newPlant)
    {
        if(selectPlant == newPlant)
        {
            CheckSelection();
            
        }
        else
        {
            CheckSelection();
            selectPlant = newPlant;
            selectPlant.btnImage.color = cancelColor;
            selectPlant.btnTxt.text = "Cancel";
            isPlanting = true;
        }
    }

    //UI event from unity
    public void SelectTool(int toolNumber)
    {
        var plotSelectionTool = (PlotSelectionTool) toolNumber;
        
        if(plotSelectionTool == selectedTool)
        {
            //deselect
            CheckSelection();
        }
        else
        {
            //select tool number and check to see if anything was also selected
            CheckSelection();
            isSelecting = true;
            selectedTool = plotSelectionTool;
            buttonsImg[toolNumber - 1].sprite = selectedButton;
        }
    }

    void CheckSelection()
    {
        if (isPlanting)
        {
            isPlanting = false;
            if (selectPlant != null)
            {
                selectPlant.btnImage.color = buyColor;
                selectPlant.btnTxt.text = "Buy";
                selectPlant = null;
            }
        }
        if (isSelecting)
        {
            if (selectedTool > 0)
            {
                buttonsImg[(int)selectedTool - 1].sprite = normalButton;
            }
            isSelecting = false;
            selectedTool = 0;
        }
    }

    public void Transaction(int value)
    {
        money += value;
        moneyTxt.text = "$" + money;
    }

}
