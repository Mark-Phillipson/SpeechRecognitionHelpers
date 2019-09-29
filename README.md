# Various Speech Recognition Helper Projects

Fast dictation box for normal dictation and converting to variable names and camel case.

A fairly simple Windows Forms C# application that includes:

* A fast dictation box
* With find and replace built-in
* Ability to do a web search directly from the box
* Ability to set the transparency to 50% by clicking the window button
* Convert the current dictation to a variable i.e. capitalising each word and remove spaces
* Convert the current dictation to camel case i.e. similar to Variable except the first character is always lowercase
* Maximise the dictation box and the controls will resize accordingly

For speed best to call from a DVC Script for example:

`SendSystemKeys "{Ctrl+c}"`

`AppBringUp "C:\Program Files (x86)\MSP Systems\Speech Recognition Helpers\DictationBoxMSP.exe"`  

`SendSystemKeys "{Ctrl+v}"`

`SendSystemKeys "{Ctrl+a}"`

