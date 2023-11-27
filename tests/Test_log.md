# Test Log
## Tests passing
![](Test_screenshots/Milestone2/test_passing_m2.png)

## Test for bullet collisions
![](Test_screenshots/Milestone2/bulletCTestmS2.png)

How this test works:
  * Spawn both a player and an enemy
  * Make the player move left
  * Wait then shoot
  * Make the player move left again
  * Store the bullet in an array and then ensure its within the boundarys of the level

![](Test_screenshots/Milestone2/bulletCTestmS2.png)
How this test works:
  * Spawn a player and an enemy
  * Have them wait to adjust the orcs bullet path
  * Have them shoot at the same time
  * Wait untill bullets collide
  * Store the bullets of player and enemy in an array
  * Check if the arrays in ^ are empty (which they should be as both bullets would delete from the scene when they collide and thus nothing would be stored)
  * If there empty the test pass
  * (This test is for bullets colliding with bullets) 

## Test for player animation
![](Test_screenshots/Milestone2/PAT_p1_ms2.png)
![](Test_screenshots/Milestone2/PAT_p2_ms2.png)


How this test works:
  * Spawn a player
  * Set playerController to move joystick in a specific direction
  * Assert that Animator Tree is storing the corresponding x,y position and playing correct animation

## Test for player attack
![](Test_screenshots/Milestone2/PattackT_ms2.png)

How this test works:
  * Spawn a player and initialize some attributes.
  * Attempt to shoot 2 bullets and check that only 1 bullet spawns to ensure that the players cannot shoot when they have no ammo.
  * Check the bullets position to make sure it moves in the correct direction.
  * Wait some time and shoot another bullet to test that ammo recharges over time.

## Test for player Collision
![](Test_screenshots/Milestone1/test_playerCollision.png)

How this test works:
  * Instantiate a player
  * Make that player walk in a direction for 5 seconds (long enough to hit the level template wall/ collider)
  * Check and make sure that player is within the level template still

## Test for player controls 
![](Test_screenshots/Milestone1/test_playerController.png)

How this test works:
  * Spawn a player
  * set the player's move direction to up
  * Assert that the player is moving up (only need to test one direction as the input system rather knows all directions or none)
  * set the player's aim direction to the right
  * Assert that the player's bullet is facing right (only need to test one direction as the input system rather knows all directions or none)

## Test for main menu
![](Test_screenshots/Milestone1/testmainmenu.png)

How this test works:
* loads the menu scene from Unity
* checks if the menu scene is loaded
* checks if the play button is existed in the menu scene
* checks if the ser clicking the "playButton" and the game scene is loaded
* cleans up the scene

## Test for pause menu
![](Test_screenshots/Milestone1/testpausemenu.png)
![](Test_screenshots/Milestone1/testpausemenu1.png)

How this test works:
* PauseMenu_ResumeGame:
  * checks if the game scene is loaded
  * checks if the pause menu is existed in the game scene and is active
  * checks if the resume button is existed in the pause menu
  * checks if the user clicking the "resumeButton" and the game scene is loaded

* PauseMenu_LoadMenu:
    * checks if the menu scene is loaded
    * checks if the pause menu is existed in the game scene and is active
    * checks if the load menu button is existed in the pause menu
    * checks if the user clicking the "loadMenuButton" and the menu scene is loaded

* PauseMenu_Restart:
    * creates a new instance of the PauseMenu
    * yields control to complete the Unity testing

## Test for COOP
![](Test_screenshots/Milestone2/coopTest1P.png)
![](Test_screenshots/Milestone2/coopTest2p.png)

How this test works:
*

## Test for Game Over Menu
![](Test_screenshots/Milestone2/GOMTMs2.png)

## Test for Orc Attack's
![](Test_screenshots/Milestone2/OAT_p1_ms2.png)
![](Test_screenshots/Milestone2/OAT_p2_ms2.png)
How OrcMoveToPlayerTest works:
* Spawn both a player and an orc
* Have the orc move to the player
* Check that the player's health has decreased, if so the test pass's

![](Test_screenshots/Milestone2/OAT_p3_ms2.png)


## Test for Orc Bullet Collision
![](Test_screenshots/Milestone2/OBCT_p1_ms2.png)
![](Test_screenshots/Milestone2/OBCT_p2_ms2.png)



How this test works:
* Spawn an orc
* Have the orc shoot
* Wait and then check if the bullet is within the boundary's of the level (if so it pass's)

## Test for Orc Collision with walls
![](Test_screenshots/Milestone2/OCWT_p1_ms2.png)
![](Test_screenshots/Milestone2/OCWT_p2_ms2.png)
![](Test_screenshots/Milestone2/OCWT_p3_ms2.png)

How this test works:
* Spawn an orc
* Have it move towards a wall
* Wait and then ensure it is within the boundary's of the level
* If so then it pass's

## Test for Orc Movement
![](Test_screenshots/Milestone2/OMT_p1_ms2.png)
![](Test_screenshots/Milestone2/OMT_p2_ms2.png)



How this test works:
*

## Test for Pause menu
![](Test_screenshots/Milestone2/PMT_p1_ms2.png)
![](Test_screenshots/Milestone2/PMT_p2_ms2.png)



How this test works:
*

## Test for the ui
![](Test_screenshots/Milestone2/uiT_p1_ms2.png)
![](Test_screenshots/Milestone2/uiT_p2_ms2.png)



How this test works:
*



