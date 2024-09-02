<p>
    The RenderComponent and its derived classes are the main graphics components in Frikandisland.
    They provide the logic to render 3D models in the MonoGame framework, logic which is called whenever the system reaches the render phase of its runtime.
</p>
<p>
    The create animated models, a RenderComponent can also be augmented with an AnimationComponent. 
    These update bone positions in 3D models whenever the system reaches its animation phase, which should come just before the render phase.
</p>
<h4 id="RenderComponent">RenderCompononents</h4>
<p> 
    Rendering 3D models typically requires 3 transformation matrices, which are applied (i.e. multiplied) one after the other: 
    a world matrix to define the position of the model in the world, 
    a view matrix to define the position and angle of the camera,
    and a projection matrix to handle perspective (the actual projection on the 2D surface of the screen).
</p>
<div>
    <img src="https://rbwhitaker.wdfiles.com/local--files/monogame-basic-matrices/transformations.png" alt="transformation matrices being applied to a model">
    <span>
        As you can see in this image from RB Whitaker's Wiki, the coordinates of a model are multiplied with the 3 transformation matrices to get their true positions on the screen.
        As per usual, he gives a very clear and concise explanation <a href="http://rbwhitaker.wikidot.com/monogame-basic-matrices" target="_blank">on his Wiki</a>.
    </span>   
</div>
<p>
    The main method of a RenderComponent is the Draw method, which takes view and projection matrices as parameters.
    <?php
        $link = isset($articles["Positioning"]) ? fixUrl($articles["Positioning"]->path) : fixUrl("Positioning");
    ?>
    The world matrix is derived from a <a href="<?php echo $link; ?>#PositionComponent" target="_self">PositionComponent</a>,
    giving us our 3 transformation matrices.
    The consequence of this is that any entity with a RenderComponent is boliged to have a PositionComponent as well.
    This is enforced by including the PositionComponent as an obligatory parameter in the constructor.
    which is why a PositionComponent is included as obligatory parameter in the constructor.
</p>
<h4>Mechanics</h4>
<p>
    A RenderComponent abstract class can be extended to make new, more advanced RenderComponents.
    The abstract class provides fields for a Model and a 2DTexture. 
    Typically, these are set suring construction.
</p>
<p> 
    Besides that, the abstract class also provides a field for a shader.
    This shader is a ShaderComponent, which forms the bridge between MonoGame and a custom shader written in hlsl.
    For ease of access and testing, ShaderComponents can easily be swapped out after construction.
    Finally, the abstract class also provides a method to set up the standard MonoGame effect configuration as fallback in case of shader failure.
</p>
<h4>Derived classes</h4>
<p>
    The most basic RenderComponent is the <b>SimpleModel</b>, which corresponds to an unanimated 3D model with no textures.
    This corresponds to what is often the first step in making a 3D asset: sculpting a mesh for your object.
    It defaults to a simple shader which only supports ambient lighting.
</p>
<p>
    The <b>TexturedModel</b> is the next step and adds support for textures. It also uses a more advanced shader with support for diffuse lighting.
    Its constructor takes either a model name (in which case it presumes the model and texture have the same name),
    or it takes both a model and texture name.
    If the texture is not found, it defaults to an error texture.
</p>
<div class="small">
    <img src="<?php echo fixUrl("_images/SimpleModel.png"); ?>" alt="simple model without textures">
    <span>
        No texture was found, so the renderer defaulted to the standard shader without textures (essentially the same render method used in the SimpleModel component).
    </span>
</div>
<p>
    Even more advanced is the <b>AnimatedModel</b>, which creates an AnimationComponent when its constructor is called.
    The AnimationComponent updates bone positions in the model (based on the state of the entity and certain timing factors), 
    which was made possible with the <a href="https://github.com/tainicom/Aether.Extras" target="_blank">Aether.Extras library</a>.
</p>
<h4>Resource requirements</h4>
<p>
    Models in fbx format (created in Blender) can be pushed through the Monogame Content pipeline.
    For animated models, use the CPU Animated Model processor.
    <br>
    It is recommended that the origin of the model be put at ground level.
    Save the file with the Z-axis as up.
</p>
