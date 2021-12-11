using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellFactory
{
    static private List<Smell> activeSmells = new List<Smell>();
    static private List<Smell> inactiveSmells = new List<Smell>();
    static private GameObject smellPrefab;
    static private Color[] colors = { Color.green, Color.blue, Color.red };


    static public void SetSmellPrefab(GameObject prefab)
    {
        smellPrefab = prefab;
    }
    static public void AddSmell(GameObject emitter, float initialStrenght, float decreaseAmount, bool weakOverTime, int colorIndex = -1)
    {
        AddSmell(emitter, emitter.transform.position, initialStrenght, decreaseAmount, weakOverTime, colorIndex);
    }
    static public void AddSmell(GameObject emitter, Vector3 position, float initialStrenght, float decreaseAmount, bool weakOverTime, int colorIndex = -1)
    {
        if (inactiveSmells.Count == 0)
        {
            Smell smell = GameObject.Instantiate(smellPrefab, position, Quaternion.identity).GetComponent<Smell>();
            smell.Init(emitter, initialStrenght, decreaseAmount, weakOverTime, colorIndex);
        }
        else
        {
            Smell smell = inactiveSmells[inactiveSmells.Count - 1];
            inactiveSmells.RemoveAt(inactiveSmells.Count - 1);
            activeSmells.Add(smell);
            smell.ReInit(emitter, position, initialStrenght, decreaseAmount, weakOverTime, colorIndex);
        }
    }
    static public void RemoveSmell(Smell smell)
    {
        smell.gameObject.SetActive(false);
        activeSmells.Remove(smell);
        inactiveSmells.Add(smell);
    }

    static public Color GetColor(int colorIndex)
    {
        return colors[colorIndex];
    }


    static public void Smelling(string tagToSearch, GameObject smeller, float noseStrenght, ref float strongest, ref float strongestAngle)
    {
        Vector3 smellerPosition = smeller.transform.position;
        strongest = 0;
        float st = 0; // variable strenght, defined here to avoid cost of defining it in the loop since it will be a costly operation
        foreach (var smell in activeSmells)
        {
            if (smell.IsExcluded(tagToSearch, smeller))
                continue;

            st = smell.SmeltStrenght(smellerPosition, noseStrenght);
            if (st < strongest)
                continue;

            strongestAngle = Vector3.Angle(smell.transform.position - smellerPosition, Vector3.forward)/360;
            strongest = st;
        }
    }
    static public void Smelling_StrongestNCumulate(string tagToSearch, GameObject smeller, float noseStrenght, ref float strongestSmell, ref float cumulateSmells)
    {
        strongestSmell = 0;
        cumulateSmells = 0;
        float st = 0; // variable strenght, defined here to avoid cost of defining it in the loop since it will be a costly operation
        foreach (var smell in activeSmells)
        {
            if (smell.IsExcluded(tagToSearch, smeller))
                continue;

            st = smell.SmeltStrenght(smeller.transform.position, noseStrenght);
            if (st > 0)
            {
                cumulateSmells += st;
                if (st > strongestSmell)
                    strongestSmell = st;
            }
        }
    }
    static public void Smelling_CumulateNAngle(string tagToSearch, GameObject smeller, float noseStrenght, ref float cumulateSmells, ref float averageAngle)
    {
        Vector3 averagePosition = Vector3.zero;
        Vector3 smellerPosition = smeller.transform.position;
        cumulateSmells = 0;
        float st = 0; // variable strenght, defined here to avoid cost of defining it in the loop since it will be a costly operation
        foreach (var smell in activeSmells)
        {
            if (smell.IsExcluded(tagToSearch, smeller))
                continue;

            st = smell.SmeltStrenght(smellerPosition, noseStrenght);
            if (st < 0)
                continue;

            averagePosition += (smell.transform.position - smellerPosition) * st;
            cumulateSmells += st;            
        }

        averageAngle = Vector3.Angle(averagePosition, Vector3.forward) / 360;
    }
}
