using Frikandisland.Systems;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace EntityFactory.Systems
{
    // AssetLoader tracks all sorts of assets coming through the content pipeline
    // When adding new assets to the pipeline, add a reference to Assets.xml

    // Uses singleton pattern: when starting game, instance is created
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

        // Dictionaries with loaded assets
        private Dictionary<string, Model> models = new Dictionary<string, Model>();
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private Dictionary<string, Effect> effects = new Dictionary<string, Effect>();

        // Fill dictionaries during actual construction
        private AssetLoader(ContentManager cm)
        {
            // Keep reference to content manager for later use
            this.cm = cm;

            // Start filling dictionaries from assets.xml
            var filename = "Content/Assets.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var filepath = Path.Combine(currentDirectory, filename);

            if (!File.Exists(filepath))
                throw new FileNotFoundException($"Assetloaded failed: {filepath} was not found");
            
            XmlDocument xml = new XmlDocument();
            xml.Load(filepath);

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                try
                {
                    string alias = ""; string location = "";

                    switch (node.Name)
                    {
                        case "Effect":
                            alias = node.Attributes["alias"].Value; location = node.Attributes["location"].Value;
                            effects.Add(alias, cm.Load<Effect>(location));
                            FrikanLogger.Write($"Assetloader: loaded effect {alias} from {location}");
                            break;
                        case "Model":
                            alias = node.Attributes["alias"].Value; location = node.Attributes["location"].Value;
                            models.Add(alias, cm.Load<Model>(location));
                            FrikanLogger.Write($"Assetloader: loaded effect {alias} from {location}");
                            break;
                        case "Texture":
                            alias = node.Attributes["alias"].Value; location = node.Attributes["location"].Value;
                            textures.Add(alias, cm.Load<Texture2D>(location));
                            FrikanLogger.Write($"Assetloader: loaded effect {alias} from {location}");
                            break;
                        case "#comment":
                            FrikanLogger.Write($"Assetloader: started on (assets for) {node.Value}");
                            break;
                        default:
                            FrikanLogger.Write($"Assetloader: node {node.Name} wasn't recognized");
                            break;
                    }
                }
                catch (Exception e) { FrikanLogger.Write($"Exception reading {node.Name} in AssetLoader: {e}"); }
            }
        }

        // RETURN ASSETS ON CALL
        public static Model GetModel(string name)
        {
            try
            {
                return instance.models[name];
            }
            catch (Exception e)
            {
                FrikanLogger.Write($"Error: couldn't find model named '{name}'");
                return null;
            }
        }
        public static Texture2D GetTexture(string name)
        {
            try
            {
                return instance.textures[name];
            }
            catch (Exception e)
            {
                FrikanLogger.Write($"Error: couldn't find texture named '{name}'");
                return null;
            }
        }
        public static Effect GetEffect(string name)
        {
            try
            {
                return instance.effects[name];
            }
            catch (Exception e)
            {
                FrikanLogger.Write($"Error: couldn't find shader named '{name}'");
                return null;
            }
        }
    }
}
