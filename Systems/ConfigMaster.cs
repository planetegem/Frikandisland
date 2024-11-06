

using System;

namespace Frikandisland.Systems
{
    public static class ConfigMaster
    {

        // Shader configs
        public static int shaderLevel = 0;
        public static int ShaderLevel 
        { 
            get { return shaderLevel; }
            set 
            { 
                shaderLevel = Math.Clamp(value, 0, 4);
                shaderNeedsUpdate = true;


            }

        }

        public static bool isTextured = false;
        public static bool IsTextured {
            set
            {
                isTextured = value;
                shaderNeedsUpdate = true;
            }
        }

        public static bool shaderNeedsUpdate = false;
        
     



    }
}
