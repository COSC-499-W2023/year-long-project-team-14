[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-718a45dd9cf7e7f842a935f5ebbe5719a5e09af4491e668f4dbf3b35d5cca122.svg)](https://classroom.github.com/online_ide?assignment_repo_id=12113751&assignment_repo_type=AssignmentRepo)
# ![alt text](docs/design/mageMadnessLogoNoBorder.png "Mage Madness")
## Project Option 4 - Team 14
### By Gabriel M, Jesse L, Justin M, Kibele S, Darion P

A 2D top-down shooter game designed for 1-2 players in local multiplayer mode, where they team up to battle against AI-controlled bots.

Assets acknowledgement:
* Characters: https://merchant-shade.itch.io/16x16-puny-characters
* Level design: https://pixel-poem.itch.io/dungeon-assetpuck
* Menu Background: https://assetstore.unity.com/packages/2d/textures-materials/tiles/parallax-dusk-mountain-background-53403
* Enemy movement AI: https://www.arongranberg.com/astar
* Portal: https://elthen.itch.io/2d-pixel-art-portal-sprites
* Font: https://fonts.google.com/specimen/Press+Start+2P?query=press
* Leaderboards: https://lootlocker.com/
* Fireball explosion: https://xthekendrick.itch.io/2d-explosions-pack
* Lightning bolt: https://ansimuz.itch.io/gothicvania-magic-pack-9
* Bullet effects: https://bdragon1727.itch.io/free-effect-and-bullet-16x16
* Ice cube: https://nemorium.itch.io/ice-asset-pack
* Spell prompt banner: https://bdragon1727.itch.io/border-and-panels-menu-part-2
* Prompt button icons: https://greatdocbrown.itch.io/gamepad-ui
* Main menu song: https://www.youtube.com/watch?v=ihreDHxtNxE&list=PLyDurh0QXZaNyHh7MIZVf04qRXfTZSsiL&index=29
* In game music: https://www.youtube.com/watch?v=PPOnlNBrHug
* Madness win song: https://www.youtube.com/watch?v=zR6fECxF44I
* Mini boss and win songs: https://xdeviruchi.itch.io/8-bit-fantasy-adventure-music-pack
* Final boss song: https://www.youtube.com/watch?v=uIfD2BKaD2k
* Final boss art: https://darkpixel-kronovi.itch.io/mecha-golem-free
* Sound effect for picking up spells: https://pixabay.com/sound-effects/cute-level-up-3-189853/
* Sound effect for fireball explosions: https://pixabay.com/sound-effects/fire-spell-100276/
* Sound effect for seeking orb explosions: https://pixabay.com/sound-effects/explosion-95176/
* Freeze spell freeze and unfreeze
  * Freeze: https://www.fesliyanstudios.com/royalty-free-sound-effects-download/magic-spell-222
  * Unfreeze: https://www.soundjay.com/nature/sounds/breaking-ice-02.mp3
* Shield spell activate and deactivate
  * Activate: https://pixabay.com/sound-effects/retro-gaming-fx-i-28793/
  * Deactivate: https://mixkit.co/free-sound-effects/lose/
* Mage rage activate and deactivate
  * Activate: https://pixabay.com/sound-effects/062708-laser-charging-81968/
  * Deactivate:  https://pixabay.com/sound-effects/sucked-into-classroom-103774/
* Final boss/mini boss sounds: https://ef9.itch.io/ultimate-8bit-sfx-library-vol-1
* Sound Effect Pack: https://opengameart.org/content/512-sound-effects-8-bit-style
* Chest Effects: https://bdragon1727.itch.io/pixel-holy-spell-effect-32x32-pack-3

## Installation and Setup

### To Run and Install the Game for Yourself:
1. Install Unity Hub (https://unity.com/download)
1. Install Unity Version 2022.3.10f1 from Unity Hub or from the release archive (https://unity.com/releases/editor/archive) 
    - Visual Studio Code is not required but strongly recommended for any C# coding done with Unity (https://marketplace.visualstudio.com/items?itemName=visualstudiotoolsforunity.vstuc)
3. Clone the repository using your version control software of choice (GitHub Desktop, Git Bash, Terminal)
4. From Unity Hub, open “499 Project” folder within the “year-long-project-team-14” root folder (year-long-project-team-14 -> app -> 499 Project)
5. Once the project is open, open the “Menu” Scene and hit the play icon to run the game.
6. The Game is hosted on itch.io to play on web browser, to update or upload the web browser build follow these steps:
    - First return to the open project in unity. Then Select the drop down menu “File” from the top left of the screen. Then select the “Build Settings” option.
    - Once in the “Build Settings” menu select the platform “WebGL” and deselect the “Scenes/Test” from the “Scenes In Build” submenu as the unit tests are not needed to actually play the game. Then select the “Switch Platform” button on the button right of the “Build Settings” menu.
    - Once the platform has been switched you will want to have an empty folder on your computer that you can save your build to. Once you have that folder select “Build” on the bottom right of the Build Settings menu and select the folder you made to store the build. Then zip the folder.
    - Once you have the zipped folder open your internet browser and head to https://itch.io/. From here log into the “Mage Madness” account. If you are not automatically taken to the “Creator Dashboard” screen then select “Dashboard” from the menu bar. Next select the “Edit” button underneath the title for the “Mage Madness” project.
    - If the prior steps were successfully completed you should now be on the “Edit Game” page. From here navigate down the page until you see a section called “Uploads”. Then simply delete the current file that is under this section. Once the file has been completed you may select the “Upload files” button and simply select the zipped file that contains the build of the game. Once that file has been uploaded select the “This file will be played in the browser” option underneath the location of the zipped file that you uploaded.
    - Finally navigate to the bottom of the “Edit game” page and select the “Save” button. If all of the steps were correctly completed then the new build will successfully be uploaded.
    - To test this simply select the “MageMadness” tag on the menu bar, select the MageMadness project that is present and play the game.

7. Please use the provided folder structure for your docs (project plan, design documentation, communications log, weekly logs, and final documentation), source code, testing, etc.    You are free to organize any additional internal folder structure as required by the project.  Please use a branching workflow and once an item is ready, do remember to issue a PR, code review, and merge it into the develop branch and then the master branch.
```
.
├── docs                    # Documentation files (alternatively `doc`)
│   ├── project plan        # Project plan document
│   ├── design              # Getting started guide
│   ├── final               # Getting started guide
│   ├── logs                # Team Logs
│   └── ...          
├── app                     # Source files
├── tests                   # Automated tests 
├── utils                   # Tools and utilities
└── README.md
```
Also, update your README.md file with the team and client/project information.  You can find details on writing GitHub Markdown [here](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax) as well as a [handy cheatsheet](https://enterprise.github.com/downloads/en/markdown-cheatsheet.pdf).   
