# Dead Letter Office
DOWNLOAD TO PLAY DEMO HERE: https://ducktran.itch.io/dead-letter-office 

# About Game
**Dead Letter Office** is a point-and-click documents-inspection style game about sorting through Republika's dead letters, reassigning names, and learning about the tales of people who are living under two nations at war. 
Drag-and-drop documents to move them around. Press [Tab] to open and read letter. Click on various objects around the screen in order to check the accuracy of the letters. <br>
<br>
![me](https://img.itch.zone/aW1hZ2UvMzY1ODMzNy8yMTc3MDA4Mi5naWY=/original/asj4d7.gif)
<br>
## Background
The game is made in Unity 2D as part of a solo personal project, where I set out to study documents-inspection games such as Papers, Please.<br>
Dead Letter Office implements procedural generation (to generate a unique mail, stamp, and letter contents every time). 
# Accessing Scripts & Art Assets
**Assets > Sprites** <br>
Everything is organized by the type of in-game object. For example, a letter asset will have its own folder that includes all its associated art, animations, and C# scripts. <br>
Names such as Republika, Greschnova, and Kastavye refer to the three major nations inside of the game's worldbuilding. The player works at a dead letter office in Republika.
## Procedural Generation Scripts
An important aspect of this game is its procedural generation system. To view and read them, go to: <br> <br>
[**Assets > Sprites > Letter > Scripts > Generate**](https://github.com/KimHaAnhTran/DeadLetterOffice/tree/main/Assets/Sprites/Letter/Scripts/Generate) <br>
_[MainDataset.cs](https://github.com/KimHaAnhTran/DeadLetterOffice/blob/main/Assets/Sprites/Letter/Scripts/Generate/MainDataset.cs)_ holds all data concerning letter contents, nations, provinces, names, and dialogues. <br>
_[MailGenerator.cs](https://github.com/KimHaAnhTran/DeadLetterOffice/blob/main/Assets/Sprites/Letter/Scripts/Generate/MailGenerator.cs)_ handles all procedural mail and stamp generation. <br><br>
[**Assets > Sprites > Letter > Scripts > Interact**](https://github.com/KimHaAnhTran/DeadLetterOffice/tree/main/Assets/Sprites/Letter/Scripts/Interact) <br>
_[LetterReader.cs](https://github.com/KimHaAnhTran/DeadLetterOffice/blob/main/Assets/Sprites/Letter/Scripts/Interact/LetterReader.cs)_ handles all randomized letter content generation based on the mail sender's nation.  <br>
_[MailOpener.cs](https://github.com/KimHaAnhTran/DeadLetterOffice/blob/main/Assets/Sprites/Letter/Scripts/Interact/MailOpener.cs)_ handles mail interaction behavior, including letter instantiation on [Tab] key press.
