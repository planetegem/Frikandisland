
using EntityFactory.Entities;

namespace EntityFactory
{
    internal class World
    {
        public World(int width, int height, Entity player)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float startX = 0.5f - width * 0.5f;
                    float startY = 0.5f - height * 0.5f;
                    float xPos = startX + x;
                    float yPos = startY + y;
                    bool tile = !(x == 0 || y == 0 || x == width - 1 || y == height - 1);

                    WorldTile newCell = new WorldTile(xPos, yPos, tile, "tile");
                }
            }
        }
    }
}
