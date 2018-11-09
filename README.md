# Enzo SharePoint Explorer
A simple project allowing SharePoint administrators and developers to view, and to a limited extend, manage SharePoint Online resources and securables. 

The Enzo SharePoint Explorer project is written in C# 2015, but can very easily be ported to other versions of Visual Studio since there is very little code in it. Because this project uses Enzo as the backend service to access SharePoint, no special SDK is needed which makes this code very easy to read and port to other languages. In order to run this application, you will need to either create an Enzo Online account, or download and install Enzo Unified on a server (see more below). 

## Project Overview
The project is structured in three main sections:  UI Events, SharePoint calls, and HTTP/REST processing.  UI Events are just that: reacting to click events, and calling the appropriate method in the EnzoSharePointOperations class. The SharePoint calls are made using REST calls; REST processing is performed by the EnzoHttpSP class, which is a simple HTTP wrapper. 

The Enzo service is the layer that performs more complex processing against SharePoint; for example, Enzo will automatically cache the SharePoint List Fields for performance; and Enzo will also handle paging automatically for lists that have more than 4,000 items. Last but not least, it is possible to pass a "WHERE" parameter to the HTTP command to filter results. While not implemented in version 1, Enzo performs the complex task of turning a natural where clause into the proper XAML command that SharePoint understands. Last but not least, the connection secrets for SharePoint are stored by Enzo; the application uses a proxy account, which makes the application inherently more secure. 

## SharePoint Methods Used
The methods available through Enzo are available in the online documentation: https://portal.enzounified.com/Docs/Documentation.html#docSharePoint

This application uses a subset of these calls. For example, one of the calls being made by Enzo SharePoint Explorer is __GetFoldersWithRoles__. The required parameters for this call are the __\_config__ and __title__ parameters. Since the \_config parameter is automatically added by the HTTP logic, all we need here is to add the title parameter in this format:  title:mylist.  The HTTP handler will automatically create a title header, and setting its value to mylist.

The call returns the folder name, the relative URL of the folder, and its assigned roles (see online documentation). This is essentially done using two lines of code:

```c#
public List<dynamic> GetFoldersWithRoles(string listName)
{
  string param = "title:" + listName;
  return _enzoSP.ExecuteAsDynamic("getfolderswithroles", param);
}
```

The actual HTTP request is performed by the enzoHttpSP class, which is a simple HTTP call wrapper class. As mentioned previously, this class adds the \_config parameter and sets the HTTP headers as needed. It also adds the required __authToken__ HTTP header automatically to authenticate the request. The code that performs the actual HTTP calls is found in the FetchInternal method. Note that while this method is primarily used to fetch records, it call also execute operations such as adding a new list item, or deleting a list. 

## How to display JSON results
Enzo returns data in XML or JSON (default) format. In order to simplify the consumption of JSON data, Enzo SharePoint Explorer contains extension methods that dynamically read and format ListView objects. The ListView.Fill() method is used for that purpose. For example, the code that displays the available SharePoint lists is written as such:

```c#
var data1 = EnzoSharePointOp.GetLists();
listViewLists.Fill(data1, "Id");
```

Behind the scenes, the Fill method dynamically inspects the data1 result set, which is a dynamic JSON object returned by the Enzo GetLists method, and formats the ListView object accordingly. The EnzoHelper class contains the helper methods to display JSON items in a ListView object. 

## Enzo Online
The easiest way to use Enzo SharePoint Explorer is to signup on the Enzo Online portal (https://portal.enzounified.com); there is a freemium level allowing you to configure a single SharePoint site. Once you have done that, you will be assigned an Authentication Token, and an Enzo Online URL that you will use to connect with Enzo SharePoint Explorer. 

Note that Enzo Online can only communicate with SharePoint Online. If you have SharePoint Server, you will need to download and use Enzo Unified instead. Enzo Unified is a more advanced version of Enzo, and needs to be installed in your infrastructure. Enzo Unified comes with a 30-day trial. You will need to download version 1.7 or higher (http://www.enzounified.com/download/).  

You will also need to create a configuration setting for SharePoint within the portal, so that Enzo can call SharePoint Online on your behalf. Click on the SharePoint tab on the left of the portal (https://portal.enzounified.com/SharePoint) to access your configuration settings. 

One of the benefits of using Enzo Online is to view the recent calls made through Enzo; for example the Access Log tab on the Enzo portal (https://portal.enzounified.com/EnzoAccounting) can be used to see which methods were called, how long each call took, how many bytes were returned, and how many items were returned to the client. 


