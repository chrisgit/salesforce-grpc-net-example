# GrpcSalesforce

## Pre-requisites
- Latest long term support version of [Nodejs](https://nodejs.org)  
- [Salesforce DX CLI](https://developer.salesforce.com/tools/sfdxcli) - recommend installing as a Node module   
- IDE or Editor such as [Visual Studio Code](https://code.visualstudio.com/)  
- Access to a Salesforce developer instance that will act as a DevHub or sign up to a [Salesforce Developer Edition Org](https://developer.salesforce.com/signup)  
- [Connect and authorise your Salesforce DX](https://developer.salesforce.com/docs/atlas.en-us.sfdx_dev.meta/sfdx_dev/sfdx_dev_auth_web_flow.htm) to your DevHub org  

## Creating a scratch org
After setting up your SFDX environment use the create-scratch-org.cmd helper in the scripts folder

## Push the code
Use sf or sfdx to push the code to the scratch org

## This project
Contains 
- Platform event named Simple_Event__e with a single custom field Action__c
- Class named SimpleEvent with 
  - static method named helloWorld which creates and publishes Simple_Event__e setting Action__c to the text 'Hello World'
  - static method names withAction with a single string parameter, creates and publishes Simple_Event__e, the value of Action__c is the value of the parameter

Using anonymous Apex create a platform event by calling methods on the helper class
```Apex
SimpleEvent.helloWorld();
```

or
```Apex
SimpleEvent.withAction('Goodbye World');
```

