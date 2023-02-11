// Call using anonymous Apex with SimpleEvent.helloWorld();
public with sharing class SimpleEvent {
    public static void helloWorld() {
        withAction('Hello World');
    }

    public static void withAction(String action) {
        Simple_Event__e evt = new Simple_Event__e(Action__c = action);
        List<Simple_Event__e> allEvents = new List<Simple_Event__e> { evt };
        List<Database.SaveResult> eventSaveResults = EventBus.publish(allEvents);
        reportErros(eventSaveResults);
    }

    private static void reportErros(List<Database.SaveResult> eventSaveResults) {
        for (Database.SaveResult result : eventSaveResults) {
            if (result.success == false) {
                String errorMessage = String.format('An error occured saving event {0}', new object[] { result.getErrors() });
                System.debug(LoggingLevel.DEBUG, errorMessage);
            }
        }
    }
}
