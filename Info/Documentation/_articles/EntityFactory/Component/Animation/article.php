<?php
    $link = isset($articles["Graphics"]) ? fixUrl($articles["Graphics"]->path) : fixUrl("Graphics");
?>
<p>
    Animations are mainly handled by the AnimationComponent, which is constructed automatically when an <a href="<?php echo $link; ?>#animated-model-chapter" target="_self">AnimatedModel</a> is created.
    The component has 2 main jobs:
</p>
<ol class="centered-list">
    <li>update bone positions of the model in the animation phase</li>
    <li>apply bone transformations during the render phase</li>
</ol>
<p>
    The animations themselves come from an fbx with the animations baked into it.
    Multiple animations can be baked into the same fbx.
</p>
<h4>Switching Animations</h4>
<p>
    During its update phase, the AnimationComponent needs to be select the correct animation and then move the animation forward along the time axis.
    It does this by making use of an AnimationDictionary, which is a custom struct.
    The AnimationDictionary is basically the bridge between all possible states an entity can have, and the animations available in the fbx.
</p>
<p>
    The AnimationDictionary is made versatile by overloading its constructor.
    For example: do you have a model with only one animation (let's say an animation called "walk")?
    Only pass one parameter to the constructor (i.e. "walk"). 
    Every animation state will be matched to the same walk animation: whether the model is running or walking, the walk animation is played.
</p>
<div>
    <img src="<?php echo fixUrl("_images/rossem.webp");?>"></img>
    <span>
        The Rossem model cycling through its 4 animations: idle, backtracking, walking and finally running.
        <br>
        Warning: terrible shaders!
    </span>
</div>
<p>
    When constructing an AnimatedModel, an AnimationDictionary needs to be set, linking the names of the animations to relevant states.
    This is done in the entity constructor.
</p>
<h4>Aether Library</h4>
<p>
    The Monogame content pipeline normally does not support animated fbx files.
    Support for fbx files has been made possible with the <a href="https://github.com/tainicom/Aether.Extras" target="_blank">Aether.Extras library</a>.
</p>
<p>
    Trying to add the Aether.Extras library to your own project?
    See this <a href="https://community.monogame.net/t/i-made-a-guide-to-setting-up-3d-animations-with-aether-extras/12242" target="_blank">guide</a> on the Monogame community forums.
</p>