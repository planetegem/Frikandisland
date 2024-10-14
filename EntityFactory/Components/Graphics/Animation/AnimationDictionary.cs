
namespace EntityFactory.Components.Graphics
{
    internal struct AnimationDictionary
    {
        public readonly string idle;
        public readonly string walking;
        public readonly string backtracking;
        public readonly string running;

        // Simplest version: model only has 1 animation
        public AnimationDictionary(string animation)
        {
            idle = animation;
            walking = animation;
            backtracking = animation;
            running = animation;
        }

        // Model with idle and simple movement
        public AnimationDictionary(string idle, string walking)
        {
            this.idle = idle;
            this.walking = walking;
            backtracking = walking;
            running = walking;
        }

        // Expanded movement
        public AnimationDictionary(string idle, string walking, string backtracking, string running)
        {
            this.idle = idle;
            this.walking = walking;
            this.backtracking = backtracking;
            this.running = running;
        }

    }
}
