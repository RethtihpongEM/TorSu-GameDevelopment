# To run this project:

## Hierarchy
- Plane
    - Click on NavMesh and Bake

- Canvas

- Player GameObject
    - Scripts:
        - Player.cs
        - PlayerController.cs
            - Drag Player GameObject into Rigidbody
            - Drag Left_Joystick from Canvas into Left Joystick
            - Drag Right_Joystick from Canvas into Right Joystick
            - Drag Player GameObject into Animator
            - Drag SM_Bullet_05 from Prefabs folder into Bullet Prefab
            - Drag ShootPoint (Child of Player) into Shoot Point
            - Drag Player GameObject into Ammo Manager
            - Drag Player GameObject into Audio Source
            - Drag bullet_sound from Sounds folder into Shoot Sound
        - AmmoManager.cs
        
- Spawner GameObject
    - Scripts:
        - Spawner.cs
            - Click (+) to add 3 field for Spawn Points
            - Drag each SpawnPoint (Child of Spawner) into each field
            - Drag Sekeleton from Prefabs folder into Zombie Prefab field

- BulletPoolManager
    - Scripts:
        - BulletPool.cs
        - Drag SM_Bullet_05 into Bullet Prefab

## Project
- Prefabs
    - HealthBar
        - Scripts:
            - HealthBar.cs
                - Choose Foreground for Healthbar Sprite