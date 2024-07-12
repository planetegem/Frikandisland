(*) This project is the continuation of https://github.com/planetegem/MonoGame3DPlayground

The EntityFactory is a small tool I developed for Frikandisland. It provides an environment to test new entities, specifically:
- check if their models are rendered correctly (no glitches in coordinate system, textures mapped correctly, ...)
- check if their bounding boxes are correct (for collision detection, ...)
- finetune input systems (fluidity of movement, attack delays, ...)

Frikandisland makes use of the ECS (Entity - Component - System) design pattern. As the name implies, this design pattern splits your code into 3 parts:
1) Entities, which mainly provide a constructor for components to be combined
2) Components, which hold all of the main game logic. When a component is constructed, it automatically registers itself with the System
3) Systems, which track all currently active components and sorts them into categories. It can then execute all components of the same type in one go (for example: instruct all render components to draw their 3D models) without having to know anything about the entities that constructed them. 



<b><< CREATING ENTITIES >></b>

To create an entity, you can extend the abstract Entity class Entity. This provides your entity with some basic logic, such as an id field. The id field consists of the name of your entity (passed in the constructor) and an entity count (extracted from the EntityLoader system). The Entity class also holds a field to track entity state.
Other than that, you only need to write a constructor for your new entity. In this constructor you can create components à la carte. Some components require other components (for example: you can't have a render component without a position component).



<b><< TYPES OF COMPONENTS >></b>

Every component is an extension of the abstract Component class. As such, every component inherits the constructor of this class, which:
- registers the component with the EntitySystem
- and tracks the entity that constructed it (just as a safety, should rarely be used)

The <b>PositionComponent</b> is a basic component that stores coordinates, rotation and momentum. It also stores proposed changes to these values (to be reconciled after collision detection). Almost every entity should have a PositionComponent, but it's possible to imagine an entity that doesn't need to track its position (for example an abstract entity that doesn't need to be rendered).
The <b>InputComponent</b> suggests changes to the PositionComponent. Currently, there is only 1 concrete InputComponent: the SimpleKeyboard component, which takes simple keyboard commands to suggest changes to position. Later on, there will also be room for more complex player inputs, or AI inputs. The SimpleKeyboard component also holds a method to finetune movement mechanics (rate of acceleration, turning circle, etc).
The <b>BoundingComponent</b> tracks bounding boxes relative to a PositionComponent. It uses these bounding boxes to perform collision detection and amend proposed changes to the PositionComponent.
The BoundingComponent also holds a method to render the bounding boxes during debug. 
The <b>RenderComponent</b> holds all render logic. It offers support for 3D models, with or without textures. In case of shader failure, there are also some base lighting settings built into the component as a fallback. There are currently 3 RenderComponents to choose from:
- SimpleModel: very basic render component with no shader & no textures. Only used for testing.
- StaticModel: a model with no animations (pipeline builds it as a simple 3D model) 
- AnimatedModel: 3D model with support for animations. Automatically constructs an AnimationComponent as well.
The <b>AnimationComponent</b> updates bone positions in the 3D model of a RenderComponent.


<u>Understanding the EntitySystem</u>
The EntitySystem decides when a component can execute its logic. It makes use of the Singleton design pattern to make it accessible from anywhere in the program.
When a component is registered, the EntitySystem performs a type check on the component and then sorts the component into lists of similar components. Next, it executes the logic of the components in phases:
1) Execute InputComponents (who may or may not propose updates to PositionComponents)
2) Execute BoundingComponents (who check for collision detection and can then amend the proposed updates to the PositionComponents)
3) Resolve PositionComponents (i.e. make their proposed position their true position)
4) Update AnimationComponents (bone positions are updated based on game time & entity state)
5) Draw all RenderComponents

Currently, this is all one class, but it is not unlikely that I'll separate this logic into multiple systems later on.


<u>Understanding the EntityLoader</u>
The EntityLoader is another Singleton, making it available anywhere in the program. It is solely responsible for loading assets from the pipeline (i.e. models, textures, shaders, etc). Simply put, it's a way of preloading assets and making the Monogame ContentManager quickly available.


<u>Creating and loading models</u>
The game supports models (both static and animated) in fbx format. These can be made in Blender. The be able to compare sizes: the floor consists of tiles that are 1x1m in Blender. Models are recommended it fit on 1 tile at most. Models should be exported with the Z-axis upwards and point of origin at z = 0. If the origin point is not at z = 0), you can manually add a z-offset to a PositionComponent. 
Support for animated fbx models has been made possible with Aether.Extras (see the Aether.Extras folder for full credits).


<u>About the collision detection</u>
The game will be 3D, but all movement will take place on a 2D plane (isometric). As such, 2D collision detection is more than sufficient. All logic for this is contained within the BoundingArea class and its extensions. Currently supports collision detection with circles and axis-aligned rectangles, but will integrate SAT collision detection to allow a wide variety of polygons.
