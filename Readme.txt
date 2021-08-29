INTERTWINED BLADES
Press button to play the game

When not in combat:
When the line is up, it means that the game is telling you there is something you can
interact with, usually outlined in white
When these things are hovered over, a description box will appear, telling you it's
purpose and function
For most of these items, when the button is pressed, a circle will load, and upon
completion, will call an event

When in combat:
If not, the player is in combat when only the crosshair is visible.
The way combat works is that the player presses the button to draw his shield.
The enemy, if successfully blocked, will become stunned.
During this time, when the crosshair shrinks, it means the enemy is vulnerable,
and the next click of the button will thrust the sword into the enemy

Optimisations:
All scene environments are static
Shadow casting and receiving are disabled on scene objects

User Experience:
Loading tries to be seamless and loads asynchrously, fading in and out to ease the
player into the next scene rather than loading a new scene, causing a noticeable freeze

Also has spatial audio, but only on desktop. 3D audio available on cardboard.

Bonus:
Game has indoors and outdoors
Game tries to maximise framerate
Game tries to make up for frame drops through tricks such as fading during scene loading
