using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {


  Dictionary<string, List<EventHandler>> mEventHandlers = new Dictionary<string, List<EventHandler>>();


  //public void HandleTestEvent1(TestEvent testEvent) {
  //  print(testEvent.TestData);
  //}
  //
  //public void HandleTestEvent2(TestEvent2 testEvent) {
  //  print(testEvent.TestDataString);
  //}

  // Use this for initialization
  void Start() {
    //RegisterEventHandler(TestEvent.EventName, new TestHandler(HandleTestEvent1));
    //RegisterEventHandler(TestEvent2.EventName, new TestHandler2(HandleTestEvent2));
  }

  // Update is called once per frame
  void Update() {
    //TestEvent newTestEvent1 = new TestEvent();
    //newTestEvent1.TestData = 99;
    //
    //
    //TestEvent2 newTestEvent2 = new TestEvent2();
    //newTestEvent2.TestDataString = "It totally worked BRO";
    //
    //TriggerEvent(newTestEvent1);
    //TriggerEvent(newTestEvent2);
  }


  void RegisterEventHandler(string type, EventHandler handler) {
    if (!mEventHandlers.ContainsKey(type)) {
      mEventHandlers.Add(type, new List<EventHandler>());
    }
    mEventHandlers[type].Add(handler);
  }

  void TriggerEvent(BaseEvent newEvent) {
    string name = newEvent.GetTypeName();
    foreach(EventHandler handler in mEventHandlers[name]) {
      handler.HandleEvent(newEvent);
    }
  }

}