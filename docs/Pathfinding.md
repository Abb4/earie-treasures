## Approaches
### Classic stealth AI

In the beginning, the most viable approach was the classic concept of a stealth AI which is known from games like *Thief* or *Dishonored*. Essentially, it's the cycle of patrolling, finding the player, chasing /attacking him and returning to the patrol route whenever the player has evaded its viewing field long enough/died due to the attacks of the AI. While functional and easy to implement, this approach has some problems. First, the game becomes rather predictable *if* one uses fixed routes which hurts replayability. Second, this approach would require the manual placement of patrol points per level. Granted, one could place the waypoints dynamically **but** this would require to parse the entire tilemap beforehand and only **then** place each patrol route *pseudo-randomly*. 

### Stalker

Like the name implies, the monster will be actively stalk the player. Besides being harder to implement, this way of pathfinding gives the designer a higher degree of freedom.  Let me highlight the benefits with a example scenario: Due to a blackout inside the entire floor, the player has to find three fuses which a randomly placed in three rooms. After the second fuse is acquired, the monster will start to either spawn in front of him (e.g. peeking in a hallway) or behind him (e.g. behind a crate). Eventually, it will start to chase the player whenever either a certain time has expired or a *heat* threshold is passed. *Heat* in this context is something like an *aggression* meter which is incremented by a player's action and the expired time.

### Nonstop chase

Last but not least, we can also use an AI which is permanently chase the player without any pause. However, this requires a particular set of items and situations which have to be rigorously tested. Furthermore, using randomness in this scenario is quite hard because each test would mean that the player would a.) start from a random location and b.) the items would be randomly distributed across the floor. On the other hand it would create a never-ending feeling of terror within the player. 