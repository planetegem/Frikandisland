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
</table>