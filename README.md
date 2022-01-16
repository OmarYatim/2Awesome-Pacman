# 2Awesome-technical-Test
 
This project is my implementation of pacman.

This project includes 3 different manager classes under Scripts/Managers : 

- GameManager : controls all game states and changing between states
- InputManager : Manages Player Input
- SoundManager : Manages ingame music and sound effects

all these classes follow the singleton Design pattern

Ghost behaviour is manged through classes in Scripts/Ghosts, GhostChase, GhostFrightened, GhostScatter, GhostHome inherit from GhostBehaviour and each is responsible of a single state for the Ghosts

Movement Behaviour is shared between Ghosts and Pacman using classes inside Scripts/Movement 

- Nodes defines points in grid where the characters can change movement direction
- Passage defines the two points in the grid where charachters can teleport to the other side
- Pacman class contains logic for controlling pacman thanks to player input received from inputManager

These classes follow the Observer Pattern either by using unity's built in OnCollision or OnTrigger methods C# Actions to invoke events in game.

We use Raycasting to determine if the path is occupied so that a character can move in a direction or not (Movement.cs)

pellets, pacman and ghosts are prefabs in Prefabs folder.

Ghosts use a Particle system to create an effect when eaten by pacman character.

All animations are created using unity's built in animation system and can be found in Animations folder while Animation Controlller for each prefab is in Animators folder.
