using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRandomizer : MonoBehaviour
{
    public List<GameObject> spells = new List<GameObject>();
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;

    void Start()
    {
        SelectSpell(spawn1);
        SelectSpell(spawn2);
        SelectSpell(spawn3);
    }

    void SelectSpell(Transform pos)
    {
        int rand = Random.Range(0, spells.Count);
        Instantiate(spells[rand], pos.position, Quaternion.identity, gameObject.transform);
        spells.RemoveAt(rand);
    }
}