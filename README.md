# Various Speech Recognition Helper Projects

# Custom Dictation Box

Fast dictation box for normal dictation and converting to variable names and camel case.

A fairly simple Windows Forms C# application that includes:

* A fast dictation box
* With find and replace built-in
* Ability to do a web search directly from the box
* Ability to set the transparency to 50% by clicking the window button
* Convert the current dictation to a variable i.e. capitalising each word and remove spaces
* Convert the current dictation to camel case i.e. similar to Variable except the first character is always lowercase
* Maximise the dictation box and the controls will resize accordingly
* Has the ability to toggle between two different font sizes, depending on what you are dictating.

For speed best to call from a DVC Script for example:

`SendSystemKeys "{Ctrl+c}"`

`AppBringUp "C:\Program Files (x86)\MSP Systems\Speech Recognition Helpers\DictationBoxMSP.exe"`  

`SendSystemKeys "{Ctrl+v}"`

`SendSystemKeys "{Ctrl+a}"`

# Browse Scripts

This Windows forms application will load the KnowBrainer XML script file and give you the ability to filter by a search term, thus showing a particular script, including the list values and even the code. Please see the following link for more info.

https://github.com/Mark-Phillipson/DragonScripts/wiki/Browse-KnowBrainer-Scripts  

# Custom IntelliSense

https://github.com/Mark-Phillipson/DragonScripts/wiki/Custom-IntelliSense

The idea with the Custom IntelliSense is is to provide the following functionality:

* Be available everywhere by voice, not just in an IDE

* Call up in a variety of ways:

* Language Category - will show a list matching the language and category.

* Language Category Name - if this matches an existing record in the Custom IntelliSense, it will run the command.


* Show List (SearchTerm) - will show a list of all records that contain the search term, regardless of the language or category.


* When a list is shown on the screen, have the ability to say edit list, thereby providing the ability to edit on-the-fly.

Each record can have different types as follows:


* Send Keys

* Copy and Paste

* Copy Only

* Run as Dragon Advanced Script command

Mainly the first two are used.


# Video Demos

To view some of the functionality in action on Twitch or YouTube please click the links:

https://www.twitch.tv/marcusvoiceprogrammer/videos
https://www.youtube.com/channel/UCagNNvjqAX003KNIANSkGXA
