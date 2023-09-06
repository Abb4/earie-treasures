## BaseActor

Each *"Base Actor"* has the following attributes which can be accessed at runtime from any other script:

- A **velocity** which will depend on the current state of its finite state machine as well as other variables
- A **item** which is currently held in either held in one (Left or Right) or both hands. For instance, a crowbar would be held in one hand while a pistol would only be held with both hands.
- The current **state** which is any state which is also contained inside the FSM of the corresponding actor. Thus, a player may not have the state *"PATROL"* while a monster is capable of entering it
- An **inventory** which the player can leverage do remain undetected by the monster
- A **physics collider** which ensures that the base actor can't clip through obstacles or walls

## Player -> BaseActor

Each *"Player"* has **all** of the attributes of the **BaseActor** and additionally

- a **control mapping** which determines which how the player can be controlled via keyboard+mouse/controller etc.
- a **health bar** which has a minimum value of zero and a maximum one of one hundred
- one or more **missions** which are essentially triggers which are activated by either satisfying a match of item to static object or a waypoint which has to be reached by the player
- a **sprint meter?** since the player shouldn't be able to run around indefinetly

## Enemy -> BaseActor

Each *"Enemy"* has **all** of the attributes of the **BaseActor** and additionally

- an **attack** with a certain amount of damage points (player HP will be substracted by it)
- **LNPP** (Last Known Player Position)
- **PBC** (Player's breadcrumbs)