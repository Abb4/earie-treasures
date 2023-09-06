Unlike rendering or AI, I'd go as far as considering the *audio system* as the heart of any good horror game.  While the worst AI or rendering technology may be a method to kill immersion, audio can make up all of these problems by inciting a feeling of dread and fear even tough the AI may be dumb as a brick and the character is only five pixels which barely resemble our player sprite. Therefore, *"earie-treasure"* has one!

## Barks

Normally, every stealth games with a great AI and game loop has one defining characteristic: Barks from the enemies. This can be callouts like *"Flashlight!"* in *F.E.A.R* which indicates that the AI is aware of the fact that you are in the same area as them or guards in *Thief* which states that he is a *"big meany guard person"* (*we love Benny*). Our game should do something very similar but with a small twist. Every now and then enemies will produce *fake barks* which are usually used to inform the player about their presence but aren't in this particular situation. One possible scenario could be as follows:

*You walk along a hallway which is dimly lit and try to open a door. It won't budge but you can hear someone ramble about and threaten to kill anyone entering it. "It" knows that you're in front of the room. And the only reason you didn't stop breathing is the fact that the door is between you and "it". However, you need to enter this room since it "may" contain a fuse which is required to open the shutters which are the only exit of the building.
So, after some searching in various other rooms you find a crowbar which might open the resistant door. Anxiously you breach the door. You hear the sound of an activated motion sensor. At the end of the room is a small cassette player. Did "it" put it there to trick you? No, that would be too easy.

Situations like these subvert the players expectation and are one of the hardest challenges of audio design. And this isn't a simple part of the game loop. While *fake barks* can be implemented by setting them as level events, it can also be used for deception. We *could* use audio items to distract the monster and stop chasing us ... or not. Still, each usage is **always** a double-edged sword:

- On the one hand, we can guide "it" to any destination we see fit
- On the other hand, "it" knows where we were ten or twenty seconds ago. While we may slip right under it, there is always the danger of getting caught on the way out

On the opposite side of the spectrum, *real barks* are **always** linked to an action from "it". If "it" growls come from a close location, **it is close**. If "it" bashes in the door, you are in trouble. Therefore each floor is an alternation of *real* and *fake barks*.