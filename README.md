# Assignment-11---DavidSantos

Gave the coin and key a pickup sound as well as gave a sound that played upon death

Fixed an issue where enemies were not hurting player on the second level

There was an issue where the players stats would reset to zero when going to next level until a coin was picked up, 
could not find where the issue was in the code

The key object would not reset in the UI when entering the exit point, essentially allowing the player to immediately go straight
to level 3 without collecting the key from level 3. Had an idea to write a piece of code that would delete the key from the UI upon entering 
the exit point so that the player would have to collect the key in level 2 to advance level 3. 

While "Level" is displayed, it is not updating in level 2, so while the player is in level 2 the UI would say Level 1. Honestly had no idea
how to fix this since the Level did update going between Levels 2 and 3.

