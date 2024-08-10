<p>
    The RenderComponent (and its derived classes) provides support to render 3D models in the MonoGame framework.
    Its main method is the Draw method, which accepts 'projection' and 'view' matrices as parameters.
</p>
<p>
    <?php
        $link = isset($articles["PositionComponent"]) ? fixUrl($articles["PositionComponent"]->path) : fixUrl("PositionComponent");
    ?>
    A RenderComponent requires a <a href="<?php echo $link; ?>" target="_self">PositionComponent</a> to build its world matrix, so a PositionComponent is a obligatory parameter in its construction.
    A RenderComponent cannot exist without a PositionComponent. 
<p>
    The top level abstract class also provides a method to create a standard shader as a fallback in case of either a missing shader file, or a missing texture file.
</p>
<h4>Derived classes</h4>
<p>
    The most basic RenderComponent is the <b>SimpleModel</b>, which presumes an unanimated 3D model, with no textures and no shader.
    This is only used for quick tests on unfinished blender models.
</p>
<p>
    The <b>StaticModel</b> is a step up: it is still an unanimated model, but adds support for optional textures and a shader.
    If only a model name is given, it presumes the texture file has the same name in the AssetLoader.
    Finally, if it doesn't find the texture in question, it defaults back to the standard shader without textures.
</p>
<div class="small">
    <img src="<?php echo fixUrl("_images/SimpleModel.png"); ?>" alt="simple model without textures">
    <span>
        No texture was found, so the renderer defaulted to the standard shader without textures (essentially the same render method used in the SimpleModel component).
    </span>
</div>
<p>
    <?php
        $link = isset($articles["AnimationComponent"]) ? fixUrl($articles["AnimationComponent"]->path) : fixUrl("AnimationComponent");
    ?>
    Even more advanced is the <b>AnimatedModel</b>, which creates an <a href="<?php echo $link; ?>" target="_self">AnimationComponent</a> when its constructor is called.
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
