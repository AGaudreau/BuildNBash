



public abstract class BaseEvent {
  abstract public string GetTypeName();

}


public interface EventHandler {
  void HandleEvent(BaseEvent eventData);
}



// // Testing area
// public class TestEvent : BaseEvent {
//   public int TestData = 0;
// 
// 
//   public static string EventName = "TestEvent";
//   public override string GetTypeName() { return EventName; }
// }
// 
// 
// public delegate void HandleTestEvent(TestEvent testEvent);
// public class TestHandler: EventHandler {
//   HandleTestEvent mTestEventHandler;
// 
//   public TestHandler(HandleTestEvent handler) {
//     mTestEventHandler = handler;
//   }
// 
//   public void HandleEvent(BaseEvent eventData) {
//     mTestEventHandler.Invoke((TestEvent)eventData);
//   }
// }




// public class TestEvent2 : BaseEvent {
//   public string TestDataString = "This Is a Test";
// 
// 
// 
//   public static string EventName = "TestEvent2";
//   public override string GetTypeName() { return EventName; }
// }
// 
// public delegate void HandleTestEvent2(TestEvent2 testEvent);
// public class TestHandler2 : EventHandler {
//   HandleTestEvent2 mTestEventHandler;
// 
//   public TestHandler2(HandleTestEvent2 handler) {
//     mTestEventHandler = handler;
//   }
// 
//   public void HandleEvent(BaseEvent eventData) {
//     mTestEventHandler.Invoke((TestEvent2)eventData);
//   }
// }