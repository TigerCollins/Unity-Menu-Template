using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScript : MonoBehaviour
{
    //CODE BY TIGER COLLINS https://tigercollins.myportfolio.com/ , https://twitter.com/_TigerCollins
    //Feel free to modify or expand upon this tween script. Please credit myself if you choose to do so.

    public bool moveTowardsEnd;
    [SerializeField]
    private bool previousDirectionTowardsEnd;
    [Tooltip("You can still use Inverse mode through script, but it will still function as an inverse")]
    public TweenMode tweenMode;
    //ATTACH THIS SCRIPT TO THE OBJECT YOU WANT TO TWEEN
    public UIManager uiScript;
    public TweenLibrary tweenLibrary;
    [Tooltip("Time for tween animation to complete. 1 is instant.")]
    [Range(1f, 5f)]
    public float tweenDuration;
    [Tooltip("This is used if the Animation ID is wrong when called or on Awake.")]
    public string defaultTween;

    [Header("Menu Exclusive")]
    [SerializeField]
    private bool isMenuPanel;
   // [HideInInspector]
    public bool canTrigger = true;
    [SerializeField]
    private bool triggerOnAwake;

    [Header("Position Settings")]
    public AdjustPosition adjustPosition;
    [SerializeField]
    [Tooltip("Leave empty to use Vector3 co-ords beginning.")]
    private Transform startPositionTransform; //Place an object to use it's transform instead of a Vector3 x/y/z.
    [SerializeField]
    [Tooltip("Leave empty to use Vector3 co-ords for destination.")]
    private Transform destinationPositionTransform; //Place an object to use it's transform instead of a Vector3 x/y/z.
    private Vector3 newPos; //Position during tweening.

    [Header("Scale Settings")]
    public AdjustScale adjustScale;
    [Tooltip("Beginning scale. Used if start transform is empty.")]
    [SerializeField]
    private Vector3 startScale;
    [SerializeField]
    [Tooltip("Destination scale. Used if end transform is empty.")]
    private Vector3 destinationScale;
    private Vector3 newScale; //Scale during tweening.

    //Tween Progression
    private float currentTime;
    private float animationCompletion;
    private bool midTransition;
    private Coroutine coroutine;
    private float curveProgression;
    [HideInInspector]
    public bool scaleCurrentlyMoving;
    private bool scaleBoolChanged;
    //[HideInInspector]
    public bool positionCurrentlyMoving;
    private bool positionBoolChanged;
    public bool finishAnimation;


    //New input system related.
    [HideInInspector]
    public int inputTriggered = 0;

    public enum TweenMode
    {
        [InspectorName("Inverse - Use Unity Event")]
        inverse,
        [InspectorName("On Command - Via Code")]
        onCommand
    }

    public void Awake()
    {
        transform.position = startPositionTransform.transform.position;

        if(tweenLibrary == null)
        {
            Debug.LogWarning("Couldn't find the Tween Library script, attempting to attach...");
            tweenLibrary = FindObjectOfType<TweenLibrary>();
        }
        //-----Trigger-----
        if (triggerOnAwake == true)
        {
            TriggerPositionTween(defaultTween);
        }

    }

    public void Start()
    {
       
    }



    public void CanTriggerState(bool trigger)
    {
        canTrigger = trigger;
    }


    //Triggered by coroutine (while loop). Actual animation.
    public void AdjustScale(int animationID)
    {
      
        //-------TRACKS TWEEN PROGRESSION---------
        if (!midTransition)
        {
            //Tracks the animation progress
            animationCompletion = currentTime / tweenDuration;
        }

        else
        {
            //Tracks the animation progress and adds the time already done to the tween progression
            //Done so the tween doesn't skip to the start position (similar to not having exit time on the Unity animator).
            animationCompletion = (currentTime + animationCompletion) / tweenDuration;
        }

       
        //--------TWEEN MAGIC-------
        //Tween moves towards the destination position from the start.
        if (!adjustScale.inverse)
        {
            currentTime += Time.unscaledDeltaTime;
            transform.localScale = Vector3.LerpUnclamped(startScale, destinationScale, tweenLibrary.animationCurve[animationID].animationCurve.Evaluate(animationCompletion));
        }

        //Tween moves towards the start position from the destination.
        else
        {
            currentTime += Time.unscaledDeltaTime;
            transform.localScale = Vector3.LerpUnclamped(destinationScale, startScale, tweenLibrary.animationCurve[animationID].animationCurve.Evaluate(animationCompletion));
        }


        curveProgression = animationCompletion;
        if (animationCompletion == 1 || animationCompletion == 0)
        {
            midTransition = false;
        }

    }

    //Triggered by coroutine (while loop). Actual animation.
    public void AdjustPosition(int animationID)
    {
        //-------TRACKS TWEEN PROGRESSION---------
        if (!midTransition)
        {
            //Tracks the animation progress
            animationCompletion = currentTime / tweenDuration;
        }

        else
        {
            //Tracks the animation progress and adds the time already done to the tween progression
            //Done so the tween doesn't skip to the start position (similar to not having exit time on the Unity animator).
            animationCompletion = (currentTime + animationCompletion) / tweenDuration;
        }
        curveProgression = animationCompletion;

        //--------TWEEN MAGIC-------
        //Tween moves towards the destination position from the start.
        if(tweenMode == TweenMode.inverse)
        {
            if (!adjustPosition.inverse)
            {
                previousDirectionTowardsEnd = true;
                currentTime += Time.unscaledDeltaTime;

                transform.position = Vector3.LerpUnclamped(startPositionTransform.position, destinationPositionTransform.position, tweenLibrary.animationCurve[animationID].animationCurve.Evaluate(animationCompletion));
            }

            //Tween moves towards the start position from the destination.
            else
            {
                currentTime += Time.unscaledDeltaTime;
                previousDirectionTowardsEnd = false;

                curveProgression = animationCompletion;
                transform.position = Vector3.LerpUnclamped(destinationPositionTransform.position, startPositionTransform.position, tweenLibrary.animationCurve[animationID].animationCurve.Evaluate(animationCompletion));
            }
        }

        else
        {
            if (moveTowardsEnd && transform.position != destinationPositionTransform.position)
            {
                previousDirectionTowardsEnd = !moveTowardsEnd;
                currentTime += Time.unscaledDeltaTime;
 
                transform.position = Vector3.LerpUnclamped(startPositionTransform.position, destinationPositionTransform.position, tweenLibrary.animationCurve[animationID].animationCurve.Evaluate(animationCompletion));
            }

            //Tween moves towards the start position from the destination.
            else if(!moveTowardsEnd && transform.position != startPositionTransform.position)
            {
                previousDirectionTowardsEnd = moveTowardsEnd;
                currentTime += Time.unscaledDeltaTime;
                curveProgression = animationCompletion;

                transform.position = Vector3.LerpUnclamped(destinationPositionTransform.position, startPositionTransform.position, tweenLibrary.animationCurve[animationID].animationCurve.Evaluate(animationCompletion));
            }
        }

   
     
    }

    public IEnumerator StartScaleTween(float time, int animationID)
    {
        var t = 0.0;
        while (t <= 1.0) //If the timer is still going
        {
            newPos = transform.position;
            t += Time.deltaTime / time;

            AdjustScale(animationID);


            yield return null;
        }

        while (t > 1) // If the timer is finished
        {
            midTransition = false;
            // if(scaleBoolChanged==false)
            //{
            scaleCurrentlyMoving = false;
            scaleBoolChanged = true;
            positionBoolChanged = false;
            yield return null;
            //  }

        }
    }
    public IEnumerator StartPositionTween(float time, int animationID)
    {
        var t = 0.0;

        while (t <= 1.0)
        {
            // myBool = true;
            newPos = transform.position;
            t += Time.deltaTime / time;

            if (positionBoolChanged == false)
            {
                positionCurrentlyMoving = true;
                positionBoolChanged = true;

                //yield return null;
            }
            AdjustPosition(animationID);

            yield return null;
        }
        if (t >= 1)
        { // If the timer is finished
            midTransition = false;
            if (positionBoolChanged == true)
            {
              
                positionCurrentlyMoving = false;
                positionBoolChanged = false;
               
            }

            yield return null;

        }

  
    }


    public void Update()
    {
        //THE FOLLOWING CODE IS ONLY FOR IF YOU EXPECT ASPECT RATIOS (rotation/resolution) TO CHANGE AT RUNTIME. CODE NOT NEEDED OTHERWISE.

        //---Position----
      

    }


    public void WhenResolutionChanges()
    {

        /*    if (previousDirectionTowardsEnd)
            {
                transform.position = destinationPositionTransform.position;
            }
            else
            {*/

    if(previousDirectionTowardsEnd)
        {
            transform.position = destinationPositionTransform.position;
        }
        else
        {

        }
            
        //}
    }

    //Triggered by input system player component.
    public void TriggerPositionTween(string animationName)
    {
        if(gameObject.activeInHierarchy)
        {
            int animationID = 0;
            for (int i = 0; i < tweenLibrary.animationCurve.Length; i++)
            {
                if (tweenLibrary.animationCurve[i].name == animationName)
                {
                    animationID = i;
                }
                else
                {
                    animationID = 0;
                }
            }

            //Following 2 if statements act a check if a menu panel is able to be triggered. ie; If it's a pause menu not to be opened when in options
            if (isMenuPanel == true)
            {
                if (canTrigger == true)
                {
                    uiScript.currentTween = GetComponent<TweenScript>();

                    //Resets time. Effectively allows tween to... well, tween.
                    currentTime = 0;
                    if (adjustPosition.adjustPositionBool)
                    {

                        if (coroutine != null)
                        {
                            StopCoroutine(coroutine);
                        }

                        //if the curve progression isn't previously finished when the coroutine stopped...
                        if (curveProgression < 1 && curveProgression > 0)
                        {
                            midTransition = true;
                        }

                        //Sets the coroutine variable and starts a coroutine.
                        coroutine = StartCoroutine(StartPositionTween(tweenDuration, animationID));

                    }

                    adjustPosition.inverse = !adjustPosition.inverse;
                }
            }

            else
            {
                uiScript.currentTween = GetComponent<TweenScript>();

                //Resets time. Effectively allows tween to... well, tween.
                currentTime = 0;
                if (adjustPosition.adjustPositionBool)
                {

                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }

                    //if the curve progression isn't previously finished when the coroutine stopped...
                      if (curveProgression < 1 && curveProgression > 0)
                        {
                            midTransition = true;
                        }

                    //Sets the coroutine variable and starts a coroutine.
                    coroutine = StartCoroutine(StartPositionTween(tweenDuration, animationID));

                }

                Debug.Log("Inverse should be change");
                adjustPosition.inverse = !adjustPosition.inverse;
            }
        }
      
       
    }

   //Triggered by other scripts
    public void TriggerScaleTween(string animationName)
    {
        if(gameObject.activeInHierarchy)
        {
            int animationID = 0;
            for (int i = 0; i < tweenLibrary.animationCurve.Length; i++)
            {
                if (tweenLibrary.animationCurve[i].name == animationName)
                {
                    animationID = i;
                    return;
                }
                else
                {
                    animationID = 0;
                }
            }


            //Resets time. Effectively allows tween to... well, tween.
            currentTime = 0;
            if (adjustScale.adjustScaleBool)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }

                //if the curve progression isn't previously finished when the coroutine stopped...
                if (curveProgression != 0)
                {
                    midTransition = true;

                }
                //Sets the coroutine variable and starts a coroutine.
                coroutine = StartCoroutine(StartScaleTween(tweenDuration, animationID));
            }
            if (!scaleCurrentlyMoving)
            {
                Debug.Log("Inverse should be change");
                adjustScale.inverse = !adjustScale.inverse;
            }


        }

    }

    //Triggered by input system player component.
    public void TriggerPositionTweenInput(string animationID)
{
        //-DUE TO A BUG WITH THE NEW INPUT SYSTEM, THIS IS A CHECK SO THE FOLLOWING IS ONLY CALLED ONCE. 
        //--INPUT IS TRIGGERED ON PRESS, HOLD AND RELEASE, NO MATTER THE SETTINGS.
        //--3 IS USED AS IT MEANS IT TRIGGERS ON THE RELEASE.

        //If statement continues on input release
        inputTriggered++;
    if (inputTriggered >= 3)
    {
            TriggerPositionTween(animationID);
            //Resets trigger count so more inputs can be made. Needs to be last thing done.
            inputTriggered = 0;
        }
       
    }

    //Triggered by input system player component.
    public void TriggerScaleTweenInput(string animationID)
    {
        //-DUE TO A BUG WITH THE NEW INPUT SYSTEM, THIS IS A CHECK SO THE FOLLOWING IS ONLY CALLED ONCE. 
        //--INPUT IS TRIGGERED ON PRESS, HOLD AND RELEASE, NO MATTER THE SETTINGS.
        //--3 IS USED AS IT MEANS IT TRIGGERS ON THE RELEASE.

        //If statement continues on input release
        inputTriggered++;
        if (inputTriggered >= 3)
        {
            TriggerScaleTween(animationID);
            //Resets trigger count so more inputs can be made. Needs to be last thing done.
            inputTriggered = 0;
        }
    }
}


//PURELY JUST A HOLDER FOR THE INSPECTOR
[System.Serializable]
public class AdjustScale
{
    [Tooltip("Is the tween going from start -> end or start <- end")]
    public bool inverse;
    [Tooltip("Allows the scale to be adjusted")]
    public bool adjustScaleBool = false;

}

//PURELY JUST A HOLDER FOR THE INSPECTOR
[System.Serializable]
public class AdjustPosition
{
    [Tooltip("Is the tween going from start -> end or start <- end")]
    public bool inverse;
    [Tooltip("Allows the position to be adjusted")]
    public bool adjustPositionBool = true;
}






