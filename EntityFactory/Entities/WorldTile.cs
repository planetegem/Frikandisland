using EntityFactory.Components.Graphics;
using EntityFactory.Components.Positioning;
using EntityFactory.Components.State;
using Microsoft.Xna.Framework;

namespace EntityFactory.Entities
{
    internal class WorldTile : Entity
    {
        public WorldTile(float x, float y, bool tile, string texture) : base($"TileX{x}Y{y}ID")
        {
            // Position component
            Vector2 pos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this.id, pos);

            // Props component: tracks bounds and state
            TileProps props = new TileProps(this.id, positioner, !tile);

            // Render component
            if (tile)
            {
                TexturedModel renderer = new TexturedModel(this.id, "tile", texture);
                renderer.Positioner = positioner;
            }

        }
    }
}
