<p>
    The Entity is an abstract class which can be extended to create concrete entities.
    <br>
    It provides an id field which contains a unique string for every instantiated entity.
</p>
<p>
    Logic to create this unique string is provided in the constructor of the abstract class:
    a name is passed as parameter to the constructor, which is then combined with an entity counter from the AssetLoader.
    So "zombie2" is an entity of the zombie type and the second entity to be created in the game.
</p>
<p>
    When extending the abstract class into a concrete entity, the class constructor should contain the logic to set up its components.
    Other than that, a concrete class should not contain any logic.
</p>
<h4>Concrete entities</h4>
<table>
    <tr>
        <th>Percolator:</th>
        <td>a simple static model of a percolator which can be moved and rotated with the keyboard. For testing purposes.</td>
    </tr>
    <tr>
        <th>Zombie:</th>
        <td>a simple animated model which can be moved with the keyboard. Used while testing Aether.Extras animations.</td>
    </tr>
    <tr>
        <th>Rossem:</th>
        <td>the first entity with a real, animated model in the game. It gets keyboard input, and can cycle between multiple animation states.</td>
    </tr>
    <tr>
        <th>WorldTile:</th>
        <td>an entity to render world tiles on screen and detect collision detection (specifically if player entities go out of bounds). Currently very basic, but will become more important when I start work on world generation.</td>
    </tr>
    <tr>
        <th>Camera:</th>
        <td>
            A special entity that tracks where the camera is. 
            It has a specialized CameraPosition component and CameraController component.
            The system retrieves a view matrix from this entity during the render phase.
        </td>
    </tr>
</table>