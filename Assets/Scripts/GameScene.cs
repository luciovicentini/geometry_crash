using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] GameObject chipContainer;
    [SerializeField] int rowsAmount = 20;
    [SerializeField] int columnsAmount = 10;

    [SerializeField] float chipMargin = 10f;

    [SerializeField] GameObject chip;

    Camera cam;


    void Start()
    {
        cam = Camera.main;
        GetChipContainerSize();
    }


    void Update()
    {

    }

    private SpriteRenderer GetSpriteRenderer()
    {
        if (chipContainer == null) return null;
        SpriteRenderer spriteRenderer = chipContainer.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null) return null;
        return spriteRenderer;
    }

    private float GetChipXSize()
    {
        SpriteRenderer spriteRenderer = GetSpriteRenderer();
        Vector3 scale = spriteRenderer.transform.localScale;
        return scale.x / columnsAmount;
    }

    private float GetChipYSize()
    {
        SpriteRenderer spriteRenderer = GetSpriteRenderer();
        Vector3 scale = spriteRenderer.transform.localScale;
        return scale.y / columnsAmount;
    }

    private void GetChipContainerSize()
    {


        /*Debug.Log("Min = " + min);
        Debug.Log("Max = " + max);

        Vector3 minScreenPoint = cam.WorldToScreenPoint(min);
        Vector3 maxScreenPoint = cam.WorldToScreenPoint(max);

        Debug.Log("minScreenPoint = " + minScreenPoint);
        Debug.Log("maxScreenPoint = " + maxScreenPoint);

        float screenWidth = maxScreenPoint.x - minScreenPoint.x; 
        float screenHeight = maxScreenPoint.y - minScreenPoint.y;

        Debug.Log("ScreenWidth = " + screenWidth);
        Debug.Log("ScreenHeight = " + screenHeight);
        
        InstantiateChip(min);
        InstantiateChip(max); */
        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        Vector3 startingPoint = spriteRenderer.bounds.min;

        float offsetX = GetChipXSize();
        float offsetY = GetChipYSize();

        for (int i = 0; i < rowsAmount; i++)
        {
            for (int j = 0; j < columnsAmount; j++)
            {
                Vector3 position = new Vector3(startingPoint.x + (offsetX * j), startingPoint.y + (offsetY * i), startingPoint.z);
                InstantiateChip(position);
            }
        }
    }

    public void InstantiateChip(Vector3 position)
    {
        GameObject newChip = Instantiate(chip, position, Quaternion.identity, gameObject.transform);
        newChip.transform.localScale = new Vector3(GetChipXSize(), GetChipYSize(), 1f);
    }
}
