using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card {

    public Zone zone;
    public int cardGameID;
    public string cardID;
    public int player;
    public int zPosition;
    public string step;
    public string type;
    public string card_name;
    internal string nextStep;
}
public enum Zone { HAND, DECK, SECRET, GRAVEYARD, PLAY, SETASIDE};