<p>
    The Component is an abstract class which can be extended to create specialized components.
    The main logic of this abstract class is in its constructor: 
    on construction, it calls a static method in the EntitySystem to start registration process.
    Every component inherits this logic, forcing it to also register itself with the system. 
</p>
<p>
    The constructor also takes a parent as parameter: this is a reference to the entity that instantiated the component.
</p>
