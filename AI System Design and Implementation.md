

AI System Design and Implementation
The AI system implemented for this project is a bat enemy designed to create a
stealth-based interaction scenario with the player. The player’s objective is to pick up
and carry an item while avoiding detection. The bat AI reacts dynamically to the player’s
actions using a finite state machine (FSM) and sensory systems, forcing the player to
make strategic decisions such as when to move, when to hide, and when to risk
carrying the item. If the player is caught while holding the item, they are forced to drop it
and lose progress, reinforcing the importance of careful movement and awareness. The
bat AI is structured using a finite state machine consisting of five states: Idle, Alert,
Chase, Search, and Respawn. In the Idle state, the bat remains stationary at its spawn
location and passively monitors for player input through its sensory systems. If the
player generates noise or enters the bat’s field of view, the AI transitions to the Alert
state. In the Alert state, the bat becomes aware of the player’s presence and begins
moving toward the player’s last known position. If the player becomes clearly visible, the
AI transitions into the Chase state. If visual confirmation is lost, the AI transitions into
the Search state instead. In the Chase state, the bat actively pursues the player using
Unity’s NavMesh system. This state represents the highest level of threat to the player.
If the player escapes the bat’s field of view, the AI stores the last known player position
and transitions into the Search state.
In the Search state, the bat navigates to the player’s last known location and attempts to
reacquire the target. If the player is detected again, the AI returns to the Chase state. If
the player is not found after a short duration, the AI returns to the Idle state.
In the Respawn state, triggered when the bat successfully collides with the player, the
bat immediately returns to its original spawn position and resets to the Idle state. This
ensures consistent gameplay flow and prevents the AI from remaining too far from its
intended area.
The transitions between states are driven by sensory input and gameplay events. The
transition from Idle to Alert occurs when the player is detected either through vision or
noise. From Alert, the AI transitions to Chase if the player is visible, or to Search if the
player cannot be confirmed visually.
During Chase, if the player escapes detection, the AI transitions to Search using the
player’s last known position. From Search, the AI either transitions back to Chase if the
player is found again, or returns to Idle after a timeout period. A separate transition
occurs when the bat collides with the player, forcing the AI into the Respawn state

regardless of its current behavior. After respawning, the AI returns to Idle, completing
the loop. The AI uses two main sensory systems: vision and hearing. The vision system
is implemented using a combination of distance checks, angle checks, and raycasting.
This ensures that the bat can only detect the player when they are within a defined field
of view and not obstructed by obstacles. This prevents unrealistic detection through
walls and encourages the player to use positioning strategically. The hearing system is
event-based and is triggered when the player performs actions such as moving or
interacting with the item. These actions generate noise, which causes the AI to enter the
Alert state even if the player is not directly visible. This adds another layer of interaction,
as the player must consider both visibility and sound when navigating the environment.
A key gameplay interaction occurs when the player is holding the item. If the bat collides
with the player in this state, the player is forced to drop the item, and the bat resets to its
spawn location. This creates a clear consequence for being detected and introduces a
risk-reward system where the player must balance speed and stealth. One of the main
challenges during development was ensuring that the AI did not continuously chase the
player without losing interest. Initially, the bat would follow the player indefinitely due to
overly permissive detection logic. This was resolved by refining the vision system with
stricter distance and angle checks, and by introducing the Search state to handle loss of
visual contact more realistically. Another challenge was integrating the NavMeshAgent
correctly. Early issues included errors where the agent was not properly placed on the
NavMesh, preventing movement. This was solved by baking the NavMesh correctly and
ensuring that the bat’s position and collider aligned with the walkable surface. Finally,
implementing item interaction required careful handling of object parenting and physics.
The item needed to attach to the player when picked up and detach cleanly when
dropped. This was resolved by toggling the Rigidbody’s kinematic state and managing
transform parenting to ensure smooth transitions. Overall, the bat AI system
successfully demonstrates responsive and adaptive behavior through the use of a finite
state machine and sensory systems. The interaction between the player and the AI
creates meaningful gameplay decisions, particularly through the risk of losing the item
when detected. The system meets the assignment objectives by combining technical
implementation with player-focused design, resulting in an engaging and functional
AI-driven scenario.
