Each monster's state machine consist of four states. **IDLE**,**INVESTIGATE**,**CHASE** and **GAME_OVER**. 

**IDLE**
Using the way-points which were assigned by the "Orchestrator", the monster will patrol the current room and search for the player. If the player enters the view of the monster, a transition to the **INVESTIGATE** state will be fired. 

**INVESTIGATE**
As a reaction the monster will navigate to the position where it has seen the player. If the player is then still visible to it, a transition to the **CHASE** state will be fired. Otherwise a transition to the state **IDLE** will be fired and the monster will continue the previously aborted patrol from the nearest patrol point.

**CHASE**
By navigating alongside the breadcrumbs which the player will now drop periodically, the monster tries to take out the player.  However, now the monster is significantly faster and evading it becomes harder. Still, if the player manages to flee from the monster's view cone for a duration of *x* seconds, the state machine will transition to the **IDLE** state. **BUT**, if the player is killed and failed to escape, a transition to the **GAME_OVER** state.

**GAME_OVER**
If the player dies and the transition to this state fires, the monster will try to devour the player remains and loop a corresponding animation.
