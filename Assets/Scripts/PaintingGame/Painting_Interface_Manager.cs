using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;

public class Painting_Interface_Manager : MonoBehaviour
{
    public List<GameObject> colorsButtonList;
    public Painting_GameManager game_Controller;

    void clearTheButtons()
    {
        foreach(GameObject button in colorsButtonList)
        {
            button.GetComponent<Outline>().effectColor = new Color(0,0,0,0);
        }
    }

    public void SelectColor(string newColor)
    {
        clearTheButtons();

        switch (newColor)
        {
            case "Red":
                game_Controller.ChangeTheColor(Color.red);
                colorsButtonList[0].GetComponent<Outline>().effectColor = Color.green;
                break;
            case "Green":
                game_Controller.ChangeTheColor(Color.green);
                colorsButtonList[1].GetComponent<Outline>().effectColor = Color.green;
                break;
            case "Blue":
                game_Controller.ChangeTheColor(Color.blue);
                colorsButtonList[2].GetComponent<Outline>().effectColor = Color.green;
                break;
            case "Yellow":
                game_Controller.ChangeTheColor(Color.yellow);
                colorsButtonList[3].GetComponent<Outline>().effectColor = Color.green;
                break;
        }
    }

    public Sprite copyTextureToImage()
    {
        //Se guarda en memoria la imagen realizada por el niñato
        P3dPaintableTexture paintableTexture = (P3dPaintableTexture)FindObjectOfType(typeof(P3dPaintableTexture));
        byte[] tex = paintableTexture.GetPngData();

        //Se carga la imagen previamente guardada en una textura
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(tex);

        //finalmente se crea un sprite con la textura creada y se devuelve el resultado
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
}
