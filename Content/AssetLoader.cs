using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Frikandisland.Content
{
    // AssetLoader tracks models, textures & audio assets
    // Uses singleton pattern: when starting game, instance is created
    // All models and textures are loaded into their appropriate MonoGame classes
    internal sealed class AssetLoader
    {
        // SINGLETON
        // Instance property & padlock for thread safety
        private static AssetLoader instance = null;
        private static readonly object instanceLock = new object();

        // Static method to call or create instance: done lazily, with thread safety
        public static AssetLoader GetInstance(ContentManager cm)
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AssetLoader(cm);
                    }
                }
            }
            return instance;
        }

        // CONSTRUCTOR
        // Save ContentManager during construction
        private ContentManager cm;

        // Dictionaries with file locations and names
        private Dictionary<string, string> models = new Dictionary<string, string>();
        private Dictionary<string, string> textures= new Dictionary<string, string>();
        private Dictionary<string, string> effects = new Dictionary<string, string>();

        // Dictionaries with loaded assets
        private Dictionary<string, Model> loadedModels = new Dictionary<string, Model>();
        private Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();
        private Dictionary<string, Effect> loadedEffects = new Dictionary<string, Effect>();
        
        // Fill dictionaries during actual construction
        private AssetLoader(ContentManager cm)
        {
            this.cm = cm;

            // Models: name followed by location (relative to Content folder)
            models.Add("zombie", "zombol_rigged");
            models.Add("percolator", "models/percolator");
            models.Add("circle", "models/circle");
            models.Add("square", "models/square");
            models.Add("rossem", "entities/rossem/rossem");
            models.Add("tile", "models/tile");

            // Textures: name followed by location (relative to Content folder)
            textures.Add("zombie", "zombo-01");
            textures.Add("rossem", "entities/rossem/rossem_diffuse");
            textures.Add("error", "textures/error");
            textures.Add("tile", "textures/tile");

            // Effects: name followed by location (relative to Content folder)
            effects.Add("ambient", "effects/AmbientShader");
            effects.Add("flat", "effects/FlatShader");
            effects.Add("normal", "effects/NormalShader");

            // Afterwards: load real assets
            foreach (var item in models) { loadedModels.Add(item.Key, cm.Load<Model>(item.Value)); }
            foreach (var item in textures) { loadedTextures.Add(item.Key, cm.Load<Texture2D>(item.Value)); }
            foreach (var item in effects) { loadedEffects.Add(item.Key, cm.Load<Effect>(item.Value)); }
        }

        // RETURN ASSETS ON CALL
        public static Model GetModel(string name)
        {
            try
            {
                return instance.loadedModels[name];
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: couldn't find model named '{name}'");
                return null;
            }
        }
        public static Texture2D GetTexture(string name)
        {
            try
            {
                return instance.loadedTextures[name];
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: couldn't find texture named '{name}'");
                return null;
            }
        }
        public static Effect GetEffect(string name)
        {
            try
            {
                return instance.loadedEffects[name];
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: couldn't find shader named '{name}'");
                return null;
            }
        }

        // OTHER METHODS
        // Count amount of entities created (part of naming scheme)
        private int entityCount = 0;
        public static int EntityCount
        {
            get
            {
                if (instance == null)
                    throw new Exception("EntityLoader was not instantiated when creating entity");
                instance.entityCount++;
                return instance.entityCount;
            }
        }


  



    }
}
