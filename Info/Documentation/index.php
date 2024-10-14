<?php
// Deconstruct URL
$urlParts = explode("/", $_SERVER["REQUEST_URI"]);
if ($urlParts[sizeof($urlParts) - 1] == ""){
    array_pop($urlParts);
}
if (sizeOf($urlParts) > 0){
    $urlArticle = ucfirst($urlParts[sizeof($urlParts) - 1]);
}

// Fetch documentation tree
$json = file_get_contents('documentation_tree.json');
$obj = json_decode($json);

// Class used to decode documentation tree
class CodeArticle {
    public $name;
    public $path;
    public $depth;
    public $children;
    public $parent;

    function __construct($name, $path, $depth, $children, $parent){
        $this->name = $name;
        $this->path = $path;
        $this->depth = $depth;
        $this->children = $children;
        $this->parent = $parent;
    }

}

// Decode documentation tree
function buildArticleRegistry() {

    function loopThroughChildren($parent, $item, $depth){
        global $articles;

        $name = $item->name;
        $path = $depth > 1 ? $articles[$parent]->path . "/" . $name : $name;

        $children = array();
        if (property_exists($item, "children")){
            foreach($item->children as $child){
                $children[] = $child->name;
            }
        }

        $articles[$name] = new CodeArticle($name, $path, $depth, $children, $parent);
        if (property_exists($item, "children")){
            foreach($item->children as $child){
                loopThroughChildren($name, $child, $depth + 1);
            }
        }
    }

    global $obj;
    foreach ($obj->tree as $child){
        loopThroughChildren("", $child, 1);
    }
}
$articles = array();
buildArticleRegistry();

// Compare against article selected in url
$currentArticle = isset($articles[$urlArticle]) ?  $articles[$urlArticle] : null;

// Prepare all article related variables
if ($currentArticle == null){
    // Fallback
    $path = "_articles/Intro/article.php";
    $children = array();
    foreach($obj->tree as $item){
        $children[] = $item->name;
    }
    $parent = "";
    $header = "";

} else {
    // Real article
    $path = "_articles/" . $currentArticle->path . "/article.php";
    $children = $currentArticle->children;
    $parent = $currentArticle->parent;
    $header = $currentArticle->name;
}

// Correct parent
if ($parent == ""){
    $parentUrl = fixUrl("");
    $parent = "start";
} else {
    $parentUrl = fixUrl($articles[$parent]->path);
}

// Format url's as fixed instead of relative
function fixUrl($url){
    // Fix to localhost structure during testing
    if ($_SERVER['SERVER_NAME'] == "localhost"){
        return "/Info/Documentation/" . $url;
    } else {
        return "https://www.planetegem.be/eb/frikandisland/documentation/" . $url;
    }
}
?>

<!DOCTYPE html>
    <html lang="en">
    <head>
        <meta http-equiv='content-type' content='text/html; charset=UTF-8'>        
        <meta name="keywords" content="documentation, ECS, design, pattern, c#, monogame, game, development, singleton, progress, blog">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <meta name="robots" content="index, follow">
        <meta name="googlebot" content="notranslate">
        <meta name="author" content="Niels Van Damme">

        <title>Fikandisland - Code Documentation</title>
        <meta property="og:title" content="Fikandisland - Code Documentation">
        <meta name="twitter:title" content="Fikandisland - Code Documentation">
        <meta name="description" content="Looking for an example of the ECS design pattern in C#? Frikandisland is a game that's being developed with the MonoGame framework.">
        <meta property="og:description" content="Looking for an example of the ECS design pattern in C#? Frikandisland is a game that's being developed with the MonoGame framework.">
        <meta name="twitter:description" content="Looking for an example of the ECS design pattern in C#? Frikandisland is a game that's being developed with the MonoGame framework.">
        
        <?php
            // Canonical URL is constructed from article path
            $addon = $currentArticle != null ? "/" . $currentArticle->path : "";
            $canonical = "https://www.planetegem.be/eb/frikandisland/documentation" . $addon;
            echo "<link rel='canonical' content='{$canonical}'>";
        ?>
        
        <meta property="og:url" content="https://www.planetegem.be/eb/frikandisland/documentation">
        <meta property="og:site_name" content="planetegem.be">
        <meta name="twitter:url" content="https://www.planetegem.be/eb/frikandisland/documentation">

        <meta property="og:image" content="https://wwww.planetegem.be/eb/mangerie_online/info/banner.png">
        <meta property="og:image:secure_url" content="https://wwww.planetegem.be/eb/mangerie_online/info/banner.png" />
        <meta property="og:image:type" content="image/png" />
        <meta property="og:image:width" content="630" />
        <meta property="og:image:height" content="500" />
        <meta property="og:image:alt" content="The banner for Mangerie Online: a bodybuilder looking into a microscope" />
        <meta name="twitter:image" content="https://wwww.planetegem.be/eb/mangerie_online/info/banner.png">

        <link rel="stylesheet" href="<?php echo fixUrl("style-base.css");?>">
        <link rel="stylesheet" href="<?php echo fixUrl("style-special.css");?>">
    </head>
    <body>
        <header>  
            <h1>Code Documentation</h1>
            <h2 id="frikandisland-header">FRIKANDISLAND</h2>
            <nav>
                <form>
                    <select id="selector">
                        <option>Select a topic</option>
                        <?php
                            foreach ($articles as $article){
                                $option = "";
                                for ($i = 1; $i < $article->depth; $i++){
                                    $option = $option . "&nbsp;&nbsp;";
                                    if ($i == $article->depth - 1){
                                        $option = $option . "&#10551; ";
                                    }
                                }
                                $option = $option . $article->name;
                                echo "<option value='{$article->path}'>{$option}</option>";
                            }
                        ?>
                    </select>
                </form>
            </nav>
        </header>
        
        <main>
            <?php
                // MAIN ARTICLE
                if (file_exists($path)){
                    // First create header
                    echo "<h3>{$header}</h3>";
    
                    // Load article (with assigned id for custom styling if necessary)
                    echo "<article id='{$header}'>";
                    include $path;
                    echo "</article>";
                } else {
                    include "_articles/Error/article.php";
                    $children = array();
                }
            ?>
            <nav <?php if($currentArticle == null || sizeOf($children) == 0){echo "class='centered'";} ?>>
                <!-- NAVIGATION -->
                <ul class="nav-to-parent">
                    <?php
                        if ($currentArticle != null){                           
                            echo "<li>";
                            echo "<a href='{$parentUrl}'>";
                            echo "return to {$parent}";
                            $imageUrl = fixUrl("_images/arrow.svg");
                            echo "<img src='{$imageUrl}'>";
                            echo "</a>";
                            echo "</li>";
                        }
                    ?>
                </ul>
                <ul class="nav-to-children">
                <?php
                    // CREATE NAVIGATION
                    if (sizeof($children) > 0){
                        foreach ($children as $child){
                            $path = fixUrl($articles[$child]->path);
                            echo "<li>";
                            echo "<a href='{$path}'>";
                            $imageUrl = fixUrl("_images/arrow.svg");
                            echo "<img src='{$imageUrl}'>";
                            echo "go to {$child}";
                            echo "</a>";
                            echo "</li>";
                        }
                    }
                ?>
                </ul>
            </nav>
            <h4 id="github-header">Hungry for more?</h4> 
            <p>
                Visit <a href="https://github.com/planetegem/Frikandisland" target="_blank">github</a> for the full source code.
            </p>
        </main>
        
        <footer>
            <button id="returnbutton" onclick="location.href='https://www.planetegem.be'" type="button">
                <div id="return"></div>
                <p>return&nbsp;to<br>planetegem</p>
            </button>
            <p>
                &#169; 2024 Niels Van Damme | info@planetegem.be | 
                <a href="https://www.instagram.com/planetegem/" style="text-decoration:none;color:black;">www.instagram.com/planetegem</a> 
                <br> 
                &#9825; 
                <a href="https://www.pecten.be" target="_blank" style="text-decoration:none;color:black;">www.pecten.be</a> 
                &#9825;
            </p>
        </footer>

        <script src="<?php echo fixUrl("script.js");?>"></script>
    </body>