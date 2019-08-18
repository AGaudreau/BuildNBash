using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TestingInteractionScript : MonoBehaviour
{

  int callMe(bool yesNo) {
    return 0;
  }

  int callMe2(bool yesNo) {
    return 0;
  }

  delegate int TestDelegate(bool test);






  public SteamVR_Action_Boolean Triggered;
  SteamVR_Input_Sources leftHand = SteamVR_Input_Sources.LeftHand;

  // Start is called before the first frame update
  void Start()
  {
    
    TestDelegate myDelegate = new TestDelegate(callMe);
    TestDelegate myDelegate2 = callMe2;
    


    if (Triggered != null) {

      //grabPinch.AddOnChangeListener(OnTriggerPressedOrReleased, leftHand);
      Triggered.AddOnStateDownListener(TriggerDown, leftHand);

    }
  }

  void TriggerDown(ISteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
    Debug.Log("LeftHandWasTriggered!");
  }

  //void OnTriggerPressedOrReleased(SteamVR_Action_In action_In) {
  //  Debug.Log("Trigger was pressed or released");
  //}

  // Update is called once per frame
  void Update()
  {
        
  }


}
