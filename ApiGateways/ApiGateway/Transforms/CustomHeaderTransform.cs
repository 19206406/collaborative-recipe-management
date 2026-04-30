using Yarp.ReverseProxy.Transforms.Builder;

namespace ApiGateway.Transforms
{
    public class CustomHeaderTransform : ITransformProvider
    {
        public void Apply(TransformBuilderContext context)
        {
            throw new NotImplementedException();
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        {
        }

        public void ValidateRoute(TransformRouteValidationContext context)
        {
        }
    }
}
