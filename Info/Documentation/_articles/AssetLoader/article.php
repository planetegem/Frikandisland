<p>
    The AssetLoader functions as a dictionary for the 3D models, textures, sound effects, fonts etc. after they have been pulled through the MonoGame ContentManager.
    Its main goal is to make already loaded forms of these assets available during gameplay and accessible from any class.
</p>
<p>
    Other than that, it also carries some static utility methods to be used during entity and map generation.
</p>
<h4>Accessible anywhere, everywhere</h4>
<p>
    The AssetLoader implements the <a href="https://refactoring.guru/design-patterns/singleton#:~:text=The%20Singleton%20pattern%20disables%20all,stricter%20control%20over%20global%20variables." target="_blank">Singleton design pattern</a>.
    Its constructor is private, with static method pointing to it. 
    If this static method is called while there is no instance of the class yet, the constructor is called.
    Else a reference to the single instance of the class is returned.
    All of this is done lazily, with a lock to provide thread safety.
</p>
<p>
    The advantage of this design pattern is that the assets loaded by the AssetLoader are accessible from anywhere in the game.
    At the same, the class still needs to be instantiated, meaning we have control over when it begins its loading process (preferably hidden behind a loading screen).
</p>
<p>
    Once instantiated, the class uses the MonoGame Content Manager to build dictionaries of its resources, which can then be accessed through static methods.
</p>
<h4>Utility methods</h4>
<p>
    The AssetLoader provides some simple logic to keep track of the amount of entities created in the game.
    This is used when giving entities their unique id: the name of the entity is combined with the enty count from the AssetLoader.
</p>

