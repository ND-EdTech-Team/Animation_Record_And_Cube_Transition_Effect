namespace Plugins.Cube.Physic
{
    public class NormalForce : BaseForce
    {
        public float forceStrength = 200f;
        
        public override void AddForce()
        {
            base.AddForce();

            foreach (var target in Targets)
                target.AddForce(transform.forward * forceStrength);
        }
    }
}