﻿using EntityFactory.Components.Graphics;
using EntityFactory.Components.Positioning;
using EntityFactory.EntityFactory.Components;
using Microsoft.Xna.Framework;

namespace EntityFactory.Entities
{
    internal class WorldTile : Entity
    {
        public WorldTile(float x, float y, bool tile, string texture) : base($"TileX{x}Y{y}ID")
        {
            // Position component
            Vector2 pos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this, pos);

            // Props component: tracks bounds and state
            TilePropsComponent props = new TilePropsComponent(this, positioner, !tile);

            // Render component
            if (tile)
            {
                TexturedModel renderer = new TexturedModel(this, positioner, "tile", texture);
            }

        }
    }
}
