<p>
    The EntityFactory is the part of Frikandisland that handles the construction & update logic of entities, such as players, NPCs or items. 
    <br>
    It can also be run as a separate, clean solution which comes with a small patch of pregenerated terrain and a camera.
    This allows for thorough testing of entity collision detection, model rendering & animations. 
</p>
<h4>Design pattern</h4>
<p>
    In order to be able to handle a wide range of entities, all of them with different combinations of logic, the EntityFactory is coded around the <a href="https://en.wikipedia.org/wiki/Entity_component_system#:~:text=Entity%E2%80%93component%E2%80%93system%20(ECS,Entity%E2%80%93Component%E2%80%93System%20layout." target="_blank">ECS design pattern</a>: Entity - Component - System.
    <br>
    Simply put, any entity is simply a combination of different components, which contain all of the logic. 
    Once constructed, the game only needs to track the individual components: the entities themselves are no longer important.
    Tracking of the components (and execution of their logic) is handled by the system.
</p>
<div>
    <img src="<?php echo fixUrl("_images/frikandisland-schema.png");?>"></img>
    <span>
        This flowchart represents an early incarnation of the EntityFactory code and is a good example of the ECS design pattern.
        Read from bottom to top: an entity is initialised, prompting the construction of a range of components.
        When constructed, the components register themselves with the system.
        The system then groups similar components together and calls their logic at the appropriate phase of the game.
    </span>
</div>
<p>
    Imagine for example that you have 2 entities in your game: one is a zombie controlled by the player, another is a zombie controlled by the AI.
    <br>
    They are both zombies, so they use the same models and animations, meaning the same render components with the same logic.
    However, they process input differently: 
    the player controlled zombie needs to listen for input from the keyboard, while the AI controlled zombie requires an AI component.
</p>
<p>
    The input components are different, but still of the same type, so the system groups them together.
    They execute at the same time to update the player and AI positions (based on their input).
    With their positions updated, the system then calls both render components to draw them in the correct position.
    <br>
    Similar logic is executed in group, making maintenance easier and adding to repeatability.
</p>
<p>
    Furthermore, while some components require each other, every component are essentially optional: you can have a zombie with a bounding component for collision detection, or without if you don't care about it colliding with anything.
    Or you can have the same zombie without a render component to get an invisible zombie.
</p>
<h4>Dive deeper into ECS</h4>
<p>
    Looking for more info about this design pattern? I can always recommend this lovely example in C# by <a href="https://matthall.codes/blog/ecs/" target="_blank">Matt Hall</a> or this very informative article at <a href="https://cowboyprogramming.com/2007/01/05/evolve-your-heirachy/" target="_blank">Cowboy Programming</a>.
    Of course, you can always dive deeper into the EntityFactory entities, components and system as well!
</p> 