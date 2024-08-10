<p>
    The Component is an abstract class, whose main goal is to force components to register with the system.
    This is done in the constructor: the EntitySystem (being a Singleton) has a static method which is called to start the registration process.
</p>
<p>
    The constructor also takes a parent as parameter: this is a reference to the entity that instantiated the component.
</p>