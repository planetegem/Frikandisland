using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EntityFactory.Components;

using System;
using System.Collections.Generic;
using EntityFactory.Components.Bounding;
using EntityFactory.Components.Graphics;

namespace EntityFactory.Entities
{
    internal class Percolator : Entity
    {

        public Percolator(float x = 0f, float y = 0f): base("percolator")
        {
            // Step 1: set position component
            Vector2 startPos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this, startPos);

            // Step 2: bounding & input
            BoundingArea[] bBoxes = new BoundingArea[]{
                new BoundingCircle(new Vector2(0, 0), 0.25f)
            };
            BoundingComponent bounder = new BoundingComponent(this, positioner, bBoxes);
            SimpleKeyboard inputer = new SimpleKeyboard(this, positioner);

            // Step 3: renderer
            StaticModel renderer = new StaticModel(this, positioner, "percolator");

        }

        public void Load(ContentManager cm)
        {
            try
            {
                Model model = cm.Load<Model>("models/percolator");
                //this.render = new SimpleModel(this, model);
            }
            catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message);}
        }
        public  void Update(GameTime gt, List<BoundingArea> neighbours)
        {
           

        }

    }
}
