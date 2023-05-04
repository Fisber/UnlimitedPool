using System;
using System.Threading.Tasks;
using Enums;
using TMPro;
using UnityEngine;
using Utils;


public class BoxView : MonoBehaviour
{
    public int CurrentIndex;
    public BoxView NextBoxView;
    public BoxView PreviosBoxView;
    public Direction Direction;
    public Vector3 InitialPosition;

    public int Value { private set; get; }

    [SerializeField] private Canvas Canvas;
    [SerializeField] private TextMeshProUGUI TextUi;

    public void UpdatePosition()
    {
        float Xposition = 0;
        
        if (GetDirection() < 0)
        {
            Xposition = InitialPosition.x - Configuration.DISTANCE_BETWEEN_BOXES;
        }
        else
        {
            Xposition = InitialPosition.x + Configuration.DISTANCE_BETWEEN_BOXES;
        }

        transform.position = new Vector3(Xposition, InitialPosition.y, InitialPosition.z);
        
        InitialPosition = transform.position;
    }

    public void UpdateText(int text)
    {
        Value = text;
        TextUi.text = text.ToString();
    }

    public async void MoveBack()
    {
        await Task.Delay(3000);
        var pos = transform.position;
        while (pos.y > 0)
        {
            pos.y -= 0.1f;
            await Task.Yield();
        }
        pos.y = 0;
        transform.position = pos;
    }

    public void SetVerticalPosition()
    {
        var position = transform.position;
        position.y = Math.Clamp(Value * 0.1f, 0, 3); 
        transform.position = position;
    }


    private void Start()
    {
        Canvas.worldCamera = Camera.main;
    }
    
    private int GetDirection()
    {
        return Direction == Direction.Right ? 1 : -1;
    }
}