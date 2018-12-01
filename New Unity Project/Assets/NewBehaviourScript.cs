using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
    public int textureWidth = 400;
    public int textureHeight = 400;
    //public Texture2D texture;
    //private Image image;
    public Transform Brick;
    ReadLog newlog;
    int i;
    int j;
    int k;
    int frame;
    int p1dt;
    int p2dt;
    GameObject player1deck;
    GameObject player2deck;
    GameObject player1Hero;
    GameObject player2Hero;
    GameObject player1hAbility;
    GameObject player2hAbility;

    int player1deckCount;
    int player2deckCount;
    string player1HeroName;
    string player2HeroName;
    string p1haName, p2haName;
    GameObject gobj = null;


    // Use this for initialization
    void Start()
    {
        newlog = new ReadLog();
        i = 0;
        j = 0;
        k = 0;
        player1deckCount = -1;
        player2deckCount = -1;
        p1dt = 0;
        p2dt = 0;

        player1deck = new GameObject();
        player2deck = new GameObject();
        player1Hero = new GameObject();
        player2Hero = new GameObject();
        player1hAbility = new GameObject();
        player2hAbility = new GameObject();
        player1deck.transform.position = new Vector3(1, 1, 20);
        player2deck.transform.position = new Vector3(1, 1, 40);
        

        player1Hero.transform.position = new Vector3(1, 20, 20);
        player2Hero.transform.position = new Vector3(1, 20, 40);
        player1hAbility.transform.position = new Vector3(1, 10, 20);
        player2hAbility.transform.position = new Vector3(1, 10, 40);

 
        player1deck.AddComponent<TextMesh>();
        player2deck.AddComponent<TextMesh>();
        player1Hero.AddComponent<TextMesh>();
        player2Hero.AddComponent<TextMesh>();
        player1hAbility.AddComponent<TextMesh>();
        player2hAbility.AddComponent<TextMesh>();

   
        // Display the file contents to the console. Variable text is a string.
        //print(text);
        // print(card1.card_name);
        print(newlog.strs.Length);
        GameObject text2 = new GameObject();
        text2.transform.position = Vector3.zero;
        text2.AddComponent<TextMesh>();

        text2.GetComponent<TextMesh>().text = "3D Text";

        text2.GetComponent<TextMesh>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Object obj = Resources.Load("card2");
            gobj = Instantiate(obj) as GameObject;

            //Instantiate(obj);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Material[] m = new Material[2];
            Object obj = Resources.Load("back");
            Material myNewMaterial = new Material(Shader.Find("Standard"));
            myNewMaterial.mainTexture = newlog.t;
            m[1] = myNewMaterial;
            m[0] = (Material)obj;
            //GameObject gobj = Instantiate(obj) as GameObject;
            gobj.GetComponent<Renderer>().materials = m;
            //Instantiate(obj);

        }

        if (false)
        {
            foreach (KeyValuePair<int, card> kvp in newlog.entity)
            {
                if (kvp.Value.player == 1 && kvp.Value.zone == Zone.DECK)
                {
                    p1dt++;

                }

                if (kvp.Value.player == 2 && kvp.Value.zone == Zone.DECK)
                {
                    p2dt++;

                }

                if (kvp.Value.type != null && kvp.Value.type.Equals("HERO") && kvp.Value.player == 1)
                {
                    Image image = GameObject.Find("Image1").GetComponent<Image>(); ;
                    player1HeroName = kvp.Value.cardID;
                    StartCoroutine(Load(kvp.Key, image));
                }

                if (kvp.Value.type != null && kvp.Value.type.Equals("HERO") && kvp.Value.player == 2)
                {
                    Image image = GameObject.Find("Image2").GetComponent<Image>(); ;
                    player2HeroName = kvp.Value.cardID;
                    print(kvp.Value.cardID);
                    StartCoroutine(Load(kvp.Key, image));

                }

                if (kvp.Value.type != null && kvp.Value.type.Equals("HERO_POWER") && kvp.Value.player == 1 && kvp.Value.zone==Zone.PLAY)
                {
                    p1haName = kvp.Value.cardID;
                    Image image = GameObject.Find("Image3").GetComponent<Image>(); ;
                    //player2HeroName = kvp.Value.cardID;
                    image.sprite = newlog.imageMap[p1haName];
                }

                if (kvp.Value.type != null && kvp.Value.type.Equals("HERO_POWER") && kvp.Value.player == 2 && kvp.Value.zone == Zone.PLAY)
                {
                    p2haName = kvp.Value.cardID;
                    Image image = GameObject.Find("Image4").GetComponent<Image>(); ;
                    //player2HeroName = kvp.Value.cardID;
                    image.sprite = newlog.imageMap[p2haName];

                }
            }

            if (p1dt != player1deckCount)
            {
                player1deckCount = p1dt;

            }
            if (p2dt != player2deckCount)
            {
                player2deckCount = p2dt;
            }
            p1dt = 0;
            p2dt = 0;


            player1deck.GetComponent<TextMesh>().text = player1deckCount.ToString();

            player1deck.GetComponent<TextMesh>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");




            player2deck.GetComponent<TextMesh>().text = player2deckCount.ToString();

            player2deck.GetComponent<TextMesh>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            player1Hero.GetComponent<TextMesh>().text = player1HeroName;

            player1Hero.GetComponent<TextMesh>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            player2Hero.GetComponent<TextMesh>().text = player2HeroName;

            player2Hero.GetComponent<TextMesh>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            player1hAbility.GetComponent<TextMesh>().text = p1haName;

            player1hAbility.GetComponent<TextMesh>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            player2hAbility.GetComponent<TextMesh>().text = p2haName;

            player2hAbility.GetComponent<TextMesh>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        }
    }

    void FixedUpdate()
    {
        
        print(newlog.strs[j]);
        if (k != -1)
        {
            k = newlog.read_log(j);
            if (k == -1)
            {
                print("over");

            }
            else
            {
                j = k;
            }
        }
       
       

        
    }

    IEnumerator Load(int id, Image image)
    {
        string urlbase= "https://art.hearthstonejson.com/v1/render/latest/enUS/256x/";
        string end = ".png";
        string gameID = newlog.entity[id].cardID;
        string url = urlbase + gameID + end;
        print(url);
        WWW www = new WWW(url);
        yield return www;

        Texture2D texture = www.texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }


}
