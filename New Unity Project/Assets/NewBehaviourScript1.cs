using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript1 : MonoBehaviour {

    // Use this for initialization
    public int textureWidth = 400;
    public int textureHeight = 400;

    public Texture2D texture;
    private Image image;

    void Start()
    {
        image = this.transform.Find("Image1").GetComponent<Image>();
        StartCoroutine(Load());
    }

  

    IEnumerator Load()
    {
        WWW www = new WWW("https://art.hearthstonejson.com/v1/orig/EX1_001.png");
        yield return www;

        texture = www.texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
