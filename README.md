# Dead Letter Office
DOWNLOAD TO PLAY DEMO HERE: https://ducktran.itch.io/dead-letter-office  

# About Game
**Dead Letter Office** is a point-and-click documents-inspection style game about sorting through Republika's dead letters, reassigning names, and learning about the tales of people who are living under two nations at war. Players have to read letters and cross-reference documents in order to fill in the name, nation, and province of the sender and receiver. Mails with invalid stamps are to be discarded. Submitting incorrect information will lead to player's salary being penalized.<br>
Drag-and-drop documents to move them around. Press [Tab] to open and read letter. Click on various objects around the screen in order to check the accuracy of the letters. <br>
<br>
![me](https://img.itch.zone/aW1hZ2UvMzY1ODMzNy8yMTc2OTg1NC5naWY=/original/MkuYex.gif)
<br>
## Background
The game is made in Unity 2D as part of a solo personal project, where I set out to study documents-inspection games such as Papers, Please.<br>
As of now, Dead Letter Office's gameplay consists of Day 1 out of 8 Days. 
## Features
* Drag and drop documents across the screen, placing them on top of the pile.
* Procedurally generated mails and letters that are unique in content, stamps, sender, and receiver each time.
* Accuracy-checking and penalization system.
* Dynamic textbox system, adjustable to length of text.
* Flipping pages in documents.
# Accessing Scripts & Art Assets
**Assets > Sprites** <br>
Everything is organized by the type of in-game object. For example, a letter asset will have its own folder that includes all its associated art, animations, and C# scripts. <br>
Names such as Republika, Greschnova, and Kastavye refer to the three major nations inside of the game's worldbuilding. The player works at a dead letter office in Republika.
## Procedural Generation Scripts
An important aspect of this game is its procedural generation system. <br>
* _MainDataset.cs_ stores all prefabs of mail types, stamps, and letters to be pieced together. Each nation has its own unique mail color, shape, and letter content.
* When the game loop starts, _Mailgenerator.cs_ randomzies the mail's nation (on Day 1, the nation is always Republika until the last mail, which is from Greschnova). Then the generator pulls a randomized prefab from the nation's mail prefab list.
* A random province of that nation will be chosen, then a random stamp will be pulled from that province's stamp prefab list. The mail is instantiated first, then the stamp is instantiated as a child gameobject within a predetermined x-y coordinate range.
* Once player presses [Tab] when cursor is over mail, _MailOpener.cs_ will instantiate a letter prefab with a randomized letter contents, sender name, and receiver name that pull from _MainDataset.cs_. <br>
[**Assets > Sprites > Letter > Scripts > Generate**](https://github.com/KimHaAnhTran/DeadLetterOffice/tree/main/Assets/Sprites/Letter/Scripts/Generate) <br>
_[MainDataset.cs](https://github.com/KimHaAnhTran/DeadLetterOffice/blob/main/Assets/Sprites/Letter/Scripts/Generate/MainDataset.cs)_ holds all data concerning letter contents, nations, provinces, names, and dialogues. <br>
_[MailGenerator.cs](https://github.com/KimHaAnhTran/DeadLetterOffice/blob/main/Assets/Sprites/Letter/Scripts/Generate/MailGenerator.cs)_ handles all procedural mail and stamp generation. <br><br>
[**Assets > Sprites > Letter > Scripts > Interact**](https://github.com/KimHaAnhTran/DeadLetterOffice/tree/main/Assets/Sprites/Letter/Scripts/Interact) <br>
_[LetterReader.cs](https://github.com/KimHaAnhTran/DeadLetterOffice/blob/main/Assets/Sprites/Letter/Scripts/Interact/LetterReader.cs)_ handles all randomized letter content generation based on the mail sender's nation.  <br>
_[MailOpener.cs](https://github.com/KimHaAnhTran/DeadLetterOffice/blob/main/Assets/Sprites/Letter/Scripts/Interact/MailOpener.cs)_ handles mail interaction behavior, including letter instantiation on [Tab] key press.
