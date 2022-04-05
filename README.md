# Milestone-Plugin-Samples-Communications

# What does it do?

Broadcast Smart Client information with a screenshot to other Smart Clients
Send C# object types that are not usually [Serializable]
Popup a camera on all Smart Clients
Send a message to the Event Server wait for “Ok” or “Error” return message.
Restart all Smart Clients

# What does it solve?

When developing plugins for Milestone, it may often be required to be able to exchange data between the Event Server, the Management Client, the Smart Clients and even to a standalone service using the Milestone SDK. Our sample demonstrates how to do this using the Message Communication class.

The alternative to this is to implement a custom data pipe that uses TCP networking. This is difficult to implement and usually requires an extra settings page somewhere to set the destination IP and port. Why re-invent the wheel when Milestone has already done an awesome job with their internal messaging system?

When creating plugins that must communicate with external servers or hardware devices, it will not always be possible to connect to them directly from the Smart Client due to networking. All commands can be routed through the Event Server or to a standalone service in a DMZ using Milestone’s default communication port 22331.
Same goes for the Management Client, you may need to pull data from a server, and network access may only be available 

![image](https://user-images.githubusercontent.com/65533203/161709556-5adc0c43-c11e-4d44-9826-57608a12c5d6.png)

# How is it done?

Based on the “Chat” plugin sample included in the MIP SDK.

We use a popular Nuget package, Newtonsoft, to serialize data into a string that can be easily passed in the Message Communications. We used to use .NET xml serialization, which did not require any external libraries but this was quite limited.  

Thread safe queues are used to handle the messages. This sample can be used as a base for a high-performance data pipes between Milestone applications/services.

Messages that can be send must implement our abstract PluginMessage class.

![image](https://user-images.githubusercontent.com/65533203/161710341-6459c50c-b28f-4f5c-a7db-2408a89e059b.png)

Each new message must be explicitly coded into the Deserialize method of the abstract class.

![image](https://user-images.githubusercontent.com/65533203/161710022-357871a6-a2df-4c44-8d5d-fea9ed73002e.png)


