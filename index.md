# Technical Decision on Assignment: Fantasy Snake

## Status
Finished. Waiting for evaluation.

## Context
I have been tasked to develop a game project with the provided game design document (GDD) by using Unity3D. This assignment is used to evaluate my programming skills, problem-solving, software design, and creativity in game development. By following the requirements and constraints strictly with this following.
* Using the latest Unity3D LTS version
* Using only legally usable assets
* No assistance from AI
* Restricted to use only Unity's packages
* Logging all technical decision

After carefully evaluating the requirements and constraints of the assignment and GDD, I have made some decisions in each of the following fields
* Unity LTS version
* Design Pattern choices
* Scriptable Object Vs JSON

## Decision
There are some decisions have been made, according to the provided requirements
1. As of now, the Unity3D 2023 is still not in an LTS release state. So the decision is made to go back to Unity3D version 2022.3 LTS version which may lack some features, but is still powerful enough to create a game in a short time.
2. In this project, I have decided to use the State Pattern to separate the current state of logic into a few classes. It can make each scene state to be flexible with each other and easy to be maintained.
3. In gameplay mechanic, hero decision. I have used the Finite-State machine (FSM) to decide the hero's action as it moves to the destination grid. The gameplay mechanic is not complex, and there are not many decisions for a hero to make. The FSM is a suitable choice for this role.
4. In some classes, I decided to separate its tasks into a few classes, by embracing the MVC principle, which separates the View which is the UI controller and logic controller. So it can be easily maintainable, and less complex.   
5. I have been using the Scriptable Object over JSON in terms of the config ability that is very simple and can be adjusted during runtime on an editor for testing. However, the JSON also has some advantages in that it can convert objects into a single string which is useful on built as for saving custom settings for this game. That's where the only place I have used JSON over the Scriptable Object.

## Consequences
With the decisions I made, I'm able to finish the game with a few additional adjustments that I hope will make the game more fun to play, and with some challenges that players can adjust as they see fit. However, If I have more time, I may add some more feature, such as a sound system and so on.