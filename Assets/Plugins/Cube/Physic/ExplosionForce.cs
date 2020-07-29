namespace Plugins.Cube.Physic
{
    public class ExplosionForce : BaseForce
    {
        public float explosionForce = 200;
        public float explosionRadius = 3;
        
        public override void AddForce()
        {
            base.AddForce();

            foreach (var target in Targets)
                target.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
    }
}