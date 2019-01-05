using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ReadLog
{
    public Transform Brick;

    public string line,name1,name2;
    public ArrayList hand = new ArrayList();
    public ArrayList playzone = new ArrayList();
    public Dictionary<int, card> entity = new Dictionary<int, card>();
    public Dictionary<string, Sprite> imageMap = new Dictionary<string, Sprite>();
    public Dictionary<string, Texture> textureMap = new Dictionary<string, Texture>();

    public ArrayList imagelist = new ArrayList();
    public ArrayList manap1 = new ArrayList();
    public ArrayList manap2 = new ArrayList();
    public int count, debug1=0, debug2=0;
    public string[] strs;
    public string test;
    public ArrayList choicesList=new ArrayList();
    public string debugstring;
    private Sprite defaultImage;
    private Texture2D defaultText;
    GameObject card = new GameObject();
    public Texture2D t;

    public ReadLog()
    {

        string newstring =null;
        strs = File.ReadAllLines(@"C:\Users\rongday\Downloads\Power.log.txt");
        int i, j = 0, length = strs.Length;
        count = 0;
        card a = new card();
        card b = new card();
        card c = new card();
        int idnum=-1;
        entity.Add(1, a);
        entity.Add(2, b);
        entity.Add(3, c);
        GameObject mono = new GameObject();
        mono.AddComponent<MonoStub>().StartCoroutine(LoadDefault());
        for (i = 0; i < length; i++)
        {
            if (strs[i].Contains("CardID") == true)
            {
                if (strs[i].EndsWith("CardID="))
                {
                    //
                }
                else
                {
                    //Regex reg = new Regex(@"(?<=id=)[0-9\.]*");
                    //Match match = reg.Match(strs[i]);
                    //string id = match.Groups[0].Value;
                    // idnum = Int32.Parse(id);

                    j = strs[i].IndexOf("CardID=");
                    newstring = strs[i].Substring(j + 7);
                    j = newstring.Length;
                    newstring = newstring.Substring(0, j);
                    if (imageMap.ContainsKey(newstring))
                    {
                        //
                    }
                    else
                    {
                        
                        mono.GetComponent<MonoStub>().StartCoroutine(LoadAll(newstring));
                       
                    }
                }
            }
           
        }
        //card=(GameObject)Resources.Load("card");
        //card.transform.position = new Vector3(0, 0, 0);
    }

    public int read_log(int j)
    {

        //StreamReader sr = new StreamReader(@"C: \Users\Public\TestFolder\output.txt");

        line = strs[j];
        //card ca = new card(strs[j]);
        //hand.Add(ca);
        if(entity[1].step== "BEGIN_MULLIGAN")
        {
            //
        }
        if (line.Contains("DebugPrintEntitiesChosen()"))
        {

        }
        if (strs[j].Contains("Creating ID") == true)
        {
           
            string newstring,tagnew;
            string id;
            int idnum;
            int i = 0;
            card newCard = new card();
            i = line.IndexOf("ID=");
            newstring = line.Substring(i + 3);
            i = newstring.IndexOf(" ");
            id = newstring.Substring(0, i);
            idnum = Int32.Parse(id);
            newCard.cardGameID = idnum;

            entity.Add(idnum, newCard);

            if (strs[j].Contains("CardID") == true)
            {
                if (strs[j].EndsWith("CardID="))
                {
                    id = null;
                }
                else
                {

                    i = line.IndexOf("CardID=");
                    newstring = line.Substring(i + 7);
                    i = newstring.Length;
                    id = newstring.Substring(0, i);
                }
                newCard.cardID = id;
                //读取直到下一个不为tag
                line = strs[++j];
                while (line.Contains("  tag"))
                {

                    i = line.IndexOf("value=");
                    newstring = line.Substring(i + 6);
                    // Regex reg = new Regex("tag=(.+)\b");
                    // Match match = reg.Match(line);
                    //string tag = match.Groups[1].Value;
                    string tag;
                    i = line.IndexOf("tag=");
                    tagnew = line.Substring(i + 4);
                    i = tagnew.IndexOf(" ");
                    id = tagnew.Substring(0, i);
                    tag = id;

                    if (tag.Equals("ZONE"))
                    {
                        newCard.zone = (Zone)Enum.Parse(typeof(Zone), newstring);
                        
                    }
                    if (line.Contains("CONTROLLER"))
                    {
                        newCard.player = Int32.Parse(newstring);
                    }
                    if (line.Contains("ENTITY_ID"))
                    {

                    }
                    if (line.Contains("CARDTYPE"))
                    {
                        newCard.type = newstring;
                    }
                    if (line.Contains("ZONE_POSITION"))
                    {
                        newCard.zPosition = Int32.Parse(newstring);
                        if (newCard.zone == Zone.HAND)
                        {
                            GameObject NewObj = new GameObject(); //Create the GameObject
                            Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
                            NewObj.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").transform);
                            NewObj.name = newCard.cardGameID.ToString();
                            NewImage = NewObj.GetComponent<Image>();
                            if (newCard.cardID!=null&&imageMap[newCard.cardID] != null)
                            {
                                NewImage.sprite = imageMap[newCard.cardID];
                            }
                            
                            if (newCard.player == 1)
                            {
                                NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400 + newCard.zPosition * 50,-400);
                               

                            }
                            else
                            {
                                NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400 + newCard.zPosition * 50, -500);
                            }
                            //new GameObject().AddComponent<MonoStub>().StartCoroutine(Load(newCard.cardGameID, NewImage));
                            NewObj.SetActive(true);
                            
                        }
                    }

                    if (j + 1 >= strs.Length)
                    {
                        return -1;
                    }
                    else
                    {
                        line = strs[++j];
                    }
                }
            }
        }
        else if (strs[j].Contains("SHOW_ENTITY - Updating Entity") == true)
        {
            string newstring;
            string id,id2;
            int idnum,idnum2;
            int i = 0;
            card currCard;


            i = line.IndexOf("Entity=");
            newstring = line.Substring(i + 7);
            i = newstring.IndexOf(" ");
            id = newstring.Substring(0, i);
            //idnum = Int32.Parse(id);
            if (Int32.TryParse(id, out idnum2))
            {
                currCard = entity[idnum2];
            }
            else
            {
                if (id.Equals("GameEntity"))
                {
                    currCard = entity[1];
                }
                else if (id.Equals(entity[2].card_name))
                {
                    currCard = entity[2];
                }
                else if (id.Equals(entity[3].card_name))
                {
                    currCard = entity[3];
                }
                else
                {
                    Regex reg = new Regex(@"(?<=id=)[0-9\.]*");
                    Match match = reg.Match(strs[j]);
                    string id3 = match.Groups[0].Value;
                    int idnum3 = Int32.Parse(id3);
                    currCard = entity[idnum3];
                }

            }

            //currCard = entity[idnum];
            i = line.IndexOf("CardID=");
            newstring = line.Substring(i + 7);
            i = newstring.Length;
            id = newstring.Substring(0, i);
            currCard.cardID = id;
            line = strs[++j];
         
            while (IsSubLine(line))
            {
                count++;
                i = line.IndexOf("value=");
                newstring = line.Substring(i + 6);

                if (line.Contains("ZONE"))
                {
                    Zone oldzone = currCard.zone;
                    if (oldzone != (Zone)Enum.Parse(typeof(Zone), newstring))
                    {
                        currCard.zone = (Zone)Enum.Parse(typeof(Zone), newstring);
                        if (oldzone != Zone.DECK)
                        {
                            //
                        }
                    }
                }
                if (line.Contains("CONTROLLER"))
                {
                    currCard.player = Int32.Parse(newstring);
                }
                if (line.Contains("ENTITY_ID"))
                {

                }
                if (line.Contains("CARDTYPE"))
                {
                    currCard.type = newstring;
                }
                if (j + 1 >= strs.Length)
                {
                    return -1;
                }
                else
                {
                    line = strs[++j];
                }
            }
        }
        else if (strs[j].Contains("TAG_CHANGE"))
        {
            string newstring;
            string id;
            string tag;
            int idnum, value;
            int i = 0;
            card currCard = null;

            //i = line.IndexOf("Entity=");
            //newstring = line.Substring(i + 7);
            //i = newstring.IndexOf(" ");
            //id = newstring.Substring(0, i);
            //idnum = Int32.Parse(id);
            Regex reg = new Regex("Entity=(.+)tag=(.+)value=(.+)");
            Match match = reg.Match(line);
            id = match.Groups[1].Value.TrimEnd();
            String name = id;
            if (Int32.TryParse(id, out idnum))
            {
                currCard = entity[idnum];
            }
            else
            {
                if (id.Equals("GameEntity"))
                {
                    currCard = entity[1];
                }
                else if (id.Equals(entity[2].card_name))
                {
                    currCard = entity[2];
                }
                else if (id.Equals(entity[3].card_name))
                {
                    currCard = entity[3];
                }else
                {
                    int result, idnum2;
                    Regex reg2 = new Regex(@"(?<=id=)[0-9\.]*");
                    Match match2 = reg2.Match(strs[j]);
                    string id2 = match2.Groups[0].Value;
                    
                    if (Int32.TryParse(id2, out result))
                    {
                        idnum2 = result;
                    }
                    else
                    {
                        idnum2 = -1;
                    }
                    currCard = entity[idnum2];
                }

            }



            //i = line.IndexOf("tag=");
            //newstring = line.Substring(i + 4);
            //i = newstring.IndexOf(" ");
            //id = newstring.Substring(0, i);
            //tag = id;
            tag = match.Groups[2].Value.TrimEnd();
            //i = line.IndexOf("value=");
            //newstring = line.Substring(i + 6);
            //i = newstring.Length;
            //id = newstring.Substring(0, i);
            id = match.Groups[3].Value.TrimEnd();
            if( Int32.TryParse(id, out value)==false)
            {
                //
            }
            if (tag.Equals("ZONE_POSITION"))
            {
                if (currCard.zone == Zone.HAND && currCard.zPosition == 0)
                {
                    GameObject NewObj = new GameObject(); //Create the GameObject
                    Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
                    NewObj.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").transform);
                    NewObj.name = currCard.cardGameID.ToString();
                    NewImage=NewObj.GetComponent<Image>();

                    UnityEngine.Object obj = Resources.Load("card2");
                    GameObject instancedObj = GameObject.Instantiate(obj) as GameObject;
                    Material[] m = new Material[2];
                    UnityEngine.Object obj2 = Resources.Load("back");
                    Material myNewMaterial = new Material(Shader.Find("Standard"));
                    if (currCard.cardID != null && textureMap[currCard.cardID] != null)
                    {
                        myNewMaterial.mainTexture = textureMap[currCard.cardID];
                    }
                    else
                    {
                       
                    }
                    //myNewMaterial.SetFloat("_Mode", 1f);
                    SetMaterialRenderingMode(myNewMaterial, RenderingMode.Cutout);
                    m[1] = myNewMaterial;
                    m[0] = (Material)obj2;
                    instancedObj.GetComponent<Renderer>().materials = m;
                    instancedObj.name = "c"+currCard.cardGameID.ToString();

                    if (currCard.cardID!=null&&imageMap[currCard.cardID]!=null)
                    {
                        NewImage.sprite = imageMap[currCard.cardID];
                    }else
                    {
                        NewImage.sprite = defaultImage;
                    }

                    if (currCard.player == 1)
                    {
                        NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500 + value * 50, -400);
                        instancedObj.GetComponent<Transform>().position = new Vector3( 100+value * 20, 20,-50);
                    }
                    else
                    {
                        NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500 + value * 50, -500);
                        instancedObj.GetComponent<Transform>().position = new Vector3(100+value * 20,   20,40);
                        instancedObj.GetComponent<Transform>().Rotate(Vector3.right*180);

                    }
                    //new GameObject().AddComponent<MonoStub>().StartCoroutine(Load(currCard.cardGameID, NewImage));
                    NewObj.SetActive(true);
                }else if(currCard.zone == Zone.HAND)
                {
                    //float x = GameObject.Find(currCard.cardGameID.ToString()).GetComponent<RectTransform>().anchoredPosition.x;
                    //float y = GameObject.Find(currCard.cardGameID.ToString()).GetComponent<RectTransform>().anchoredPosition.y;
                    //float incX = (400 - x)/10;
                    //float incY = (20 - y)/10;
                    //while (GameObject.Find(currCard.cardGameID.ToString()).GetComponent<RectTransform>().anchoredPosition.x != 500 && GameObject.Find(currCard.cardGameID.ToString()).GetComponent<RectTransform>().anchoredPosition.y != 20)
                    //{
                    //    x += incX;
                    //    y += incY;
                    //    GameObject.Find(currCard.cardGameID.ToString()).GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                    //}
                    if (value == 0)
                    {
                        GameObject.Find(currCard.cardGameID.ToString()).GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 20);
                        GameObject.Find("c" + currCard.cardGameID.ToString()).GetComponent<Transform>().position = new Vector3(500, 20, -50);
                    }
                    else
                    {
                        if (currCard.player == 1)
                        {
                            GameObject.Find("c"+currCard.cardGameID.ToString()).GetComponent<Transform>().position = new Vector3(100 + value * 20, 20, -50);
                            GameObject.Find(currCard.cardGameID.ToString()).GetComponent<RectTransform>().anchoredPosition = new Vector2(-500 + value * 50, -400);
                        }
                        else
                        {
                            GameObject.Find("c"+currCard.cardGameID.ToString()).GetComponent<Transform>().position = new Vector3(100 + value * 20, 20, 40);
                            GameObject.Find( currCard.cardGameID.ToString()).GetComponent<RectTransform>().anchoredPosition = new Vector2(-500 + value * 50, -500);


                        }
                    }
                }
                currCard.zPosition = value;
            }
            if (tag.Equals("ZONE")) {
                currCard.zone = (Zone)Enum.Parse(typeof(Zone), id);
            }
            if (tag.Equals("NEXT_STEP"))
            {
                currCard.nextStep = id;
            }
            if (tag.Equals("RESOURCES"))
            {
                if(name== entity[2].card_name)
                {
                    if (Int32.Parse(id) > manap1.Count)
                    {
                        currCard.resources = Int32.Parse(id);
                        UnityEngine.Object obj = Resources.Load("crystal");
                        GameObject instancedObj = GameObject.Instantiate(obj) as GameObject;
                        Material[] m = new Material[1];
                        UnityEngine.Object obj2 = Resources.Load("crystal_material");
                        //Material myNewMaterial = new Material(Shader.Find("Standard"));
                        m[0] = (Material)obj2;
                        instancedObj.GetComponent<Renderer>().materials = m;
                        instancedObj.name = "1r" + id;
                        manap1.Add(instancedObj);
                    }else if (Int32.Parse(id) < manap1.Count)
                    {

                    }
                }else if (name == entity[3].card_name)
                {
                    if (Int32.Parse(id) > manap2.Count)
                    {
                        currCard.resources = Int32.Parse(id);
                        UnityEngine.Object obj = Resources.Load("crystal");
                        GameObject instancedObj = GameObject.Instantiate(obj) as GameObject;
                        Material[] m = new Material[1];
                        UnityEngine.Object obj2 = Resources.Load("crystal_material");
                        //Material myNewMaterial = new Material(Shader.Find("Standard"));
                        m[0] = (Material)obj2;
                        instancedObj.GetComponent<Renderer>().materials = m;
                        instancedObj.name = "2r" + id;
                        manap2.Add(instancedObj);
                    }
                    else if (Int32.Parse(id) < manap2.Count)
                    {

                    }
                }
               
                

            }
            if (tag.Equals("STEP"))
            {
                currCard.step = id;
            }
            if (tag.Equals("MULLIGAN_STATE"))
            {
                id.TrimEnd();
                if (id.Contains("INPUT"))
                {
                    //debug1 = -1;
                    if (currCard == entity[2]|| currCard == entity[3])
                    {
                        //debug2 = -1;
                        j++;
                        //debug1 = j;
                        //debugstring = strs[j];
                        j=ChoiceList(j);
                        return j;
                    }
                }
            }
            if (j + 1 >= strs.Length)
            {
                return -1;
            }
            else
            {
                j++;
            }
        }
        else if (line.Contains("PlayerID=1, PlayerName="))
        {
            //Regex reg = new Regex("PlayerName=(.+)$");
         
            //Match match = reg.Match(line);
            //string playerName = match.Groups[1].Value;
            string playerName;
            int i;
            i = line.IndexOf("PlayerName=");
            playerName = line.Substring(i + "PlayerName=".Length);
            entity[2].card_name = playerName;
            name2 = playerName;
            j++;
        }
        else if (line.Contains("PlayerID=2, PlayerName="))
        {

            string playerName;
            int i;
            i = line.IndexOf("PlayerName=");
            playerName = line.Substring(i + "PlayerName=".Length);
            entity[3].card_name = playerName;
            name1 = playerName;
            j++;
        }
        else
        {
            if (j + 1 >= strs.Length)
            {
                return -1;
            }
            else
            {
                j++;
            }
        }
        return j;
    }

    IEnumerator Load(int id, Image image)
    {
        string url=null;
        if (entity[id].cardID == null) {
            url = @"C:\Users\Public\TestFolder\back.png";
        }
        else
        {
            string urlbase = "https://art.hearthstonejson.com/v1/render/latest/enUS/256x/";
            string end = ".png";
            string gameID = entity[id].cardID;
            url = urlbase + gameID + end;
        }
        WWW www = new WWW(url);
        yield return www;

        Texture2D texture = www.texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }
    IEnumerator LoadDefault()
    {
       
        string url = @"C:\Users\Public\TestFolder\back.png";
       
        WWW www = new WWW(url);
        yield return www;

        Texture2D texture = www.texture;
        defaultText = texture;
        defaultImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    IEnumerator LoadAll(string id)
    {
        string url = null;


        string urlbase = "https://art.hearthstonejson.com/v1/render/latest/enUS/256x/";
        string end = ".png";
        string gameID = id;
        url = urlbase + gameID + end;

        WWW www = new WWW(url);
        yield return www;

        Texture2D texture = www.texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        if (!imageMap.ContainsKey(id))
        {
            imageMap.Add(id, sprite);
            textureMap.Add(id, texture);
        }
        
    }

    public Boolean IsSubLine(string line)
    {
        int start;
        string newstring;
        start = line.IndexOf("-");
        newstring = line.Substring(start + "-".Length);
        newstring=newstring.TrimStart();
        test = newstring;
        return newstring.StartsWith("tag=");

    }

    public int ChoiceList(int j)
    {


        int i = 0;
        while (strs[j].Contains("GameState.DebugPrintEntityChoices()"))
        {
            Regex reg = new Regex(@"(?<=id=)[0-9\.]*");
        Match match = reg.Match(strs[j]);
        string id = match.Groups[0].Value;
        int idnum = Int32.Parse(id);
        int[] currChocie = new int[5];
        choicesList.Add(currChocie);
        j++;
        
            if (strs[j].Contains("Entities"))
            {
                debug1++;
                Regex reg2 = new Regex(@"(?<=id=)[0-9\.]*");
                Match match2 = reg.Match(strs[j]);
                string id2 = match2.Groups[0].Value;
                int idnum2 = Int32.Parse(id2);
                currChocie[i]=idnum2;
                i++;
            }
            if (j + 1 >= strs.Length)
            {
                return -1;
            }
            else
            {
                j++;
            }
        }
        return j;
    }


    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                material.SetFloat("_Mode", 1f);
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
