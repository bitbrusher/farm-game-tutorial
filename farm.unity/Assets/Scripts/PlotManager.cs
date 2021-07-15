using Farm;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    private bool _isPlanted = false;
    private SpriteRenderer _plant;
    private BoxCollider2D _plantCollider;

    private int _plantStage = 0;
    private float _timer;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;

    private SpriteRenderer _plot;

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
        _plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        _fm = transform.parent.GetComponent<FarmManager>();
        _plot = GetComponent<SpriteRenderer>();

        if (isBought)
        {
            _plot.sprite = drySprite;
        }
        else
        {
            _plot.sprite = unavailableSprite;
        }
    }

    private void Update()
    {
        if (_isPlanted && !_isDry)
        {
            _timer -= _speed * Time.deltaTime;

            if (_timer < 0 && !IsLastPlantStage())
            {
                _timer = _selectedPlant.timeBtwStages;
                _plantStage++;
                UpdatePlant();
            }
        }
    }

    private void OnMouseDown()
    {
        if (_isPlanted)
        {
            if (IsLastPlantStage() && !_fm.isPlanting && !_fm.isSelecting)
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
                case PlotSelectionTool.Water:
                    if (isBought)
                    {
                        _isDry = false;
                        _plot.sprite = normalSprite;
                        if (_isPlanted) UpdatePlant();
                    }

                    break;

                case PlotSelectionTool.Fertilizer:
                    if (_fm.money >= 10 && isBought)
                    {
                        _fm.Transaction(-10);
                        if (_speed < 2) _speed += .2f;
                    }

                    break;

                case PlotSelectionTool.PlotBuyer:
                    if (_fm.money >= 100 && !isBought)
                    {
                        _fm.Transaction(-100);
                        isBought = true;
                        _plot.sprite = drySprite;
                    }

                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if (_fm.isPlanting)
        {
            if (_isPlanted || _fm.selectPlant.plant.buyPrice > _fm.money || !isBought)
            {
                //can't buy
                _plot.color = unavailableColor;
            }
            else
            {
                //can buy
                _plot.color = availableColor;
            }
        }

        if (_fm.isSelecting)
        {
            switch (_fm.selectedTool)
            {
                case PlotSelectionTool.Water:
                    if (isBought && _isDry)
                    {
                        _plot.color = availableColor;
                    }
                    else
                    {
                        _plot.color = unavailableColor;
                    }

                    break;

                case PlotSelectionTool.Fertilizer:
                    if (isBought && _fm.money >= 10)
                    {
                        _plot.color = availableColor;
                    }
                    else
                    {
                        _plot.color = unavailableColor;
                    }

                    break;

                case PlotSelectionTool.PlotBuyer:
                    if (!isBought && _fm.money >= 100)
                    {
                        _plot.color = availableColor;
                    }
                    else
                    {
                        _plot.color = unavailableColor;
                    }

                    break;

                default:
                    _plot.color = unavailableColor;
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        _plot.color = Color.white;
    }

    private void Harvest()
    {
        _isPlanted = false;
        _plant.gameObject.SetActive(false);
        _fm.Transaction(_selectedPlant.sellPrice);
        _isDry = true;
        _plot.sprite = drySprite;
        _speed = 1f;
    }

    private void Plant(PlantObject newPlant)
    {
        _selectedPlant = newPlant;
        _isPlanted = true;

        _fm.Transaction(-_selectedPlant.buyPrice);

        _plantStage = 0;
        UpdatePlant();
        _timer = _selectedPlant.timeBtwStages;
        _plant.gameObject.SetActive(true);
    }

    private void UpdatePlant()
    {
        if (_isDry)
        {
            _plant.sprite = _selectedPlant.dryPlanted;
        }
        else
        {
            _plant.sprite = _selectedPlant.plantStages[_plantStage];
        }

        _plantCollider.size = _plant.sprite.bounds.size;
        _plantCollider.offset = new Vector2(0, _plant.bounds.size.y / 2);
    }

    private bool IsLastPlantStage()
    {
        return _plantStage == _selectedPlant.plantStages.Length - 1;
    }
}