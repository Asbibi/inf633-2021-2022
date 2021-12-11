using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell : MonoBehaviour
{
    private GameObject emitter;
    private float decreaseStrenghtAmount;
    private float startStrenght;
    private float currentStrenght;
    private bool decreaseStrength;

    private bool debug = false;
    private Color debugColor;


    public void Init(GameObject emitter, float initialStrenght, float decreaseAmount, bool weakSmellOverTime, int debugColorIndex)
    {
        this.emitter = emitter;
        startStrenght = initialStrenght;
        currentStrenght = initialStrenght;
        decreaseStrenghtAmount = decreaseAmount;
        decreaseStrength = weakSmellOverTime;
        debug = debugColorIndex > -1;
        if (debug)
            debugColor = SmellFactory.GetColor(debugColorIndex);
    }
    public void ReInit(GameObject emitter, Vector3 position, float initialStrenght, float decreaseAmount, bool weakSmellOverTime, int debugColorIndex)
    {
        Init(emitter, initialStrenght, decreaseAmount, weakSmellOverTime, debugColorIndex);
        transform.position = position;// emitter.transform.position;
        gameObject.SetActive(true);
    }



    void FixedUpdate()
    {
        currentStrenght -= decreaseStrenghtAmount;
        if (currentStrenght < 0)
            SmellFactory.RemoveSmell(this);
        else if (debug)
        {
            Debug.DrawLine(transform.position, transform.position + (transform.forward * StrenghtToUse()),  debugColor, Time.fixedDeltaTime, false);
            Debug.DrawLine(transform.position, transform.position + (transform.forward * -StrenghtToUse()),debugColor, Time.fixedDeltaTime, false);
            Debug.DrawLine(transform.position, transform.position + (transform.right * StrenghtToUse()),    debugColor, Time.fixedDeltaTime, false);
            Debug.DrawLine(transform.position, transform.position + (transform.right * -StrenghtToUse()),  debugColor, Time.fixedDeltaTime, false);
        }
    }
    private float StrenghtToUse()
    {
        return decreaseStrength ? currentStrenght : startStrenght;
    }



    public float SmeltStrenght(Vector3 pos, float noseStrenght)
    {
        return StrenghtToUse() - (Vector3.Distance(pos, transform.position) / noseStrenght);
    }
    public GameObject GetEmitter()
    {
        return emitter;
    }
    public bool IsExcluded(string searchedTag, GameObject smeller)
    {
        if (emitter == null)
            return true;

        if (!emitter.CompareTag(searchedTag))
            return true;

        return smeller.Equals(emitter);    // ignores his own smell
    }
}
