using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EntityFactory.Systems
{
    // EntityLoader tracks models, textures & audio assets
    // Uses singleton pattern: when starting game, instance is created
    // All models and textures are loaded into their appropriate MonoGame classes
    internal sealed class EntityLoader
    {
        // Save ContentManager during construction
        private ContentManager cm;

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

        // Constructor is private to hide it
        private EntityLoader(ContentManager cm)
        {
            this.cm = cm;

            // instantiate dictionaries
            models = new Dictionary<string, string>();
            textures = new Dictionary<string, string>();
            effects = new Dictionary<string, string>();

            // Models: name followed by location (relative to Content folder)
            models.Add("zombie", "zombol_rigged");
            models.Add("percolator", "models/percolator");
            models.Add("circle", "models/circle");
            models.Add("square", "models/square");

            // Textures: name followed by location (relative to Content folder)
            textures.Add("zombie", "zombo-01");

            // Effects: name followed by location (relative to Content folder)
            effects.Add("ambient", "effects/AmbientShader");
            effects.Add("main", "effects/MainShader");

            // Afterwards: load real assets
            loadedModels = new Dictionary<string, Model>();
            foreach (var item in models){ loadedModels.Add(item.Key, cm.Load<Model>(item.Value)); }
            loadedTextures = new Dictionary<string, Texture2D>();
            foreach (var item in textures) { loadedTextures.Add(item.Key, cm.Load<Texture2D>(item.Value)); }
            loadedEffects = new Dictionary<string, Effect>();
            foreach (var item in effects) { loadedEffects.Add(item.Key, cm.Load<Effect>(item.Value)); }
        }

        // Instance property & padlock for thread safety
        private static EntityLoader instance = null;
        private static readonly object instanceLock = new object();

        // Static method to call or create instance: done lazily, with thread safety
        public static EntityLoader GetInstance(ContentManager cm)
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new EntityLoader(cm);
                    }
                }
            }
            return instance;
        }

        // Dictionaries with file locations and names
        private Dictionary<string, string> models;
        private Dictionary<string, string> textures;
        private Dictionary<string, string> effects;

        // Dictionaries with loaded assets
        private Dictionary<string, Model> loadedModels;
        private Dictionary<string, Texture2D> loadedTextures;
        private Dictionary<string, Effect> loadedEffects;

        public static Model GetModel(string name)
        {
            return instance.loadedModels[name];
        }
        public static Texture2D GetTexture(string name)
        {
            return instance.loadedTextures[name];
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



    }
}
