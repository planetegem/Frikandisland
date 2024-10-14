<p>
    The EntityFactory will be one of the core components of Frikandisland.
    As its name suggests, it handles construction and subsquent tracking of entities in the game. 
    It's what forms the bridge between player input, entity states (such as positioning) and the final rendering of entities on screen.
</p>
<p>
    A seperate, clean solution which only contains the EntityFactory files and a small pregenerated world can be used for extensive testing of new entities (for example collision detectio, look and feel, etc).
</p>
<h4>Design pattern</h4>
<p>
    The EntityFactory is implemented using the <a href="https://en.wikipedia.org/wiki/Entity_component_system#:~:text=Entity%E2%80%93component%E2%80%93system%20(ECS,Entity%E2%80%93Component%E2%80%93System%20layout." target="_blank">ECS design pattern</a>,
    or less cryptically: the Entity-Component-System design pattern. 
    What does this mean? Well, simply put, all code is spread out over 3 categories:
</p>
<ol>
    <li>
        <b>Entities</b> act as containers for different components: 
        they construct components and apply custom parameters/settings to them if necessary. 
        This can be any number or variation of components.
        Consider entities to be a restaurant menu: they tell you which type of component is the starter, which one is the main dish, and which is dessert.
    </li>
    <li>
        <b>Components</b> contain the brunt of all game logic. 
        Depending on the type of component, it can do different things.
        There are components specialized in rendering objects to the screen, components specialized in handling keyboard input, components that track positions, and so on.
        During construction, every component stops by the system to announce its creation.
    </li>
    <li>
       During construction, every component stops by the <b>System</b> to announce its existence. 
       The system then groups these components by type and determines the order it will execute their logic.
       The system does not need to know anything about entities:
       it only looks at the components and does not need to know how one relates to the other.
    </li>
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
    Lets have a look at a concrete example with 2 entities: 
    the first is a zombie controlled by the player, another is a zombie controlled by an AI.
    <br>
    The player zombie consists of a component that tracks its position, a component that renders a model of a zombie, and a component that tracks input from the keyboard.
    The AI zombie is exactly the same, except that it takes an AI component instead of a keyboard component for input.
</p>
<p>
    When these entities are created, the construction of their components is ordered.
    Every one of these components tells the system of its existence. 
    The render components say "hello, I'm a render component" and the position components say "hello, I'm a position component".
    The keyboard component and AI component are different, but they're both input components, so they go "hello, I'm an input component".
    The system remembers all of the components and groups them together by type.
</p>
<p>
    During runtime, the system starts executing the internal of the components.
    First, it looks at the input components and asks them for an update.
    The keyboard component registers an arrow key event, which prompts it to update the position component of its parent entity.
    The AI component runs a pathfinding algorithm, also prompting an update to the position component of its parent entity.
</p>
<p>
    Next, the system asks the position components to check for collision detection. 
    If a collision with another position component is detected, the position component resets its position.
</p>
<p>
    Finally, the system tells all render component to draw their models on the screen.
    They get the position of the model from the position component of their parent entity.
</p>
<p>
    All of this is done to make it easy to create new entities à la carte: 
    while some components require each other (for example: you can't draw something without knowing where to draw it),
    every component is essentially optional. 
    Want an invisible zombie? Simply drop the render component, and nothing will be drawn on screen.
</p>
<h4>Dive deeper into ECS</h4>
<p>
    Looking for more info about this design pattern? I can always recommend this lovely example in C# by <a href="https://matthall.codes/blog/ecs/" target="_blank">Matt Hall</a> or this very informative article at <a href="https://cowboyprogramming.com/2007/01/05/evolve-your-heirachy/" target="_blank">Cowboy Programming</a>.
    Or of course, you can always dive deeper into the EntityFactory entities, components and system as well!
</p> 