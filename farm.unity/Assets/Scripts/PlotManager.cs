using UnityEngine;

public class PlotManager : MonoBehaviour
{
    bool isPlanted = false;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;

    int plantStage = 0;
    float timer;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;

    SpriteRenderer plot;

    private PlantObject _selectedPlant;

    private FarmManager _fm;

    private bool _isDry = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite;

    private float _speed = 1f;
    public bool isBought = true;

    private void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        _fm = transform.parent.GetComponent<FarmManager>();
        plot = GetComponent<SpriteRenderer>();
        if (isBought)
        {
            plot.sprite = drySprite;
        }
        else
        {
            plot.sprite = unavailableSprite;
        }
    }

    private void Update()
    {
        if (isPlanted && !_isDry)
        {
            timer -= _speed * Time.deltaTime;

            if (timer < 0 && plantStage < _selectedPlant.plantStages.Length - 1)
            {
                timer = _selectedPlant.timeBtwStages;
                plantStage++;
                UpdatePlant();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if (plantStage == _selectedPlant.plantStages.Length - 1 && !_fm.isPlanting && !_fm.isSelecting)
            {
                Harvest();
            }
        }
        else if (_fm.isPlanting && _fm.selectPlant.plant.buyPrice <= _fm.money && isBought)
        {
            Plant(_fm.selectPlant.plant);
        }

        if (_fm.isSelecting)
        {
            switch (_fm.selectedTool)
            {
                case 1:
                    if (isBought)
                    {
                        _isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlanted) UpdatePlant();
                    }

                    break;
                case 2:
                    if (_fm.money >= 10 && isBought)
                    {
                        _fm.Transaction(-10);
                        if (_speed < 2) _speed += .2f;
                    }

                    break;
                case 3:
                    if (_fm.money >= 100 && !isBought)
                    {
                        _fm.Transaction(-100);
                        isBought = true;
                        plot.sprite = drySprite;
                    }

                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if (_fm.isPlanting)
        {
            if (isPlanted || _fm.selectPlant.plant.buyPrice > _fm.money || !isBought)
            {
                //can't buy
                plot.color = unavailableColor;
            }
            else
            {
                //can buy
                plot.color = availableColor;
            }
        }

        if (_fm.isSelecting)
        {
            switch (_fm.selectedTool)
            {
                case 1:
                case 2:
                    if (isBought && _fm.money >= (_fm.selectedTool - 1) * 10)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }

                    break;
                case 3:
                    if (!isBought && _fm.money >= 100)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }

                    break;
                default:
                    plot.color = unavailableColor;
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        plot.color = Color.white;
    }

    private void Harvest()
    {
        isPlanted = false;
        plant.gameObject.SetActive(false);
        _fm.Transaction(_selectedPlant.sellPrice);
        _isDry = true;
        plot.sprite = drySprite;
        _speed = 1f;
    }

    private void Plant(PlantObject newPlant)
    {
        _selectedPlant = newPlant;
        isPlanted = true;

        _fm.Transaction(-_selectedPlant.buyPrice);

        plantStage = 0;
        UpdatePlant();
        timer = _selectedPlant.timeBtwStages;
        plant.gameObject.SetActive(true);
    }

    private void UpdatePlant()
    {
        if (_isDry)
        {
            plant.sprite = _selectedPlant.dryPlanted;
        }
        else
        {
            plant.sprite = _selectedPlant.plantStages[plantStage];
        }

        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y / 2);
    }
}