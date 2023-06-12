# Battleships
Battleships-2-player-local-with-replay  

### Description:
A two-player, turn-based game played locally on a 10x10 grid. The game features randomly generated fleets of seven ships in various sizes: two 1x1, two 2x1, one 3x1, one 4x1, and one 5x1. Players take turns selecting coordinates to launch attacks, aiming to sink their opponent's fleet. The first player to sink all of their opponent's ships wins.

The game records replays after each match, allowing players to review and playback their moves.
### Info
Created with Unity 2021.3.13f1

### Installation
Clone/download repository and open up the project with Unity.

### Replay module features (WIP)
#### Recording:  
It is currently recording the entire state of the board  
Replays are saved in `.json` format in `Application.persistentDataPath` directory
i. e. (`Appdata/LocalLow/DefaultCompany/Battleships`)

#### Playback:  
It is only injecting the player actions and not reading the entire state.
Replay module will play the most recent recorded replay. It can be accessed from the main menu.

#### Partial modularity (WIP)
Although not fully featured, it is designed with flexibility in mind and could potentially support usage across similar projects in the future

#### Screenshots
![image](https://github.com/pzoghbi/battleships/assets/10575726/807c0786-e882-4452-8a0e-dbef17a3b178)
![image](https://github.com/pzoghbi/battleships/assets/10575726/ebad391b-259e-4de4-b304-72617381e666)
