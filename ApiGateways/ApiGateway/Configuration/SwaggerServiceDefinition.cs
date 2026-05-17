namespace ApiGateway.Configuration
{
    public class SwaggerServiceDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string SwaggerUrl { get; set; } = string.Empty; 
    }

    public class SwaggerAggregatorOptions
    {
        public const string Section = "SwaggerAggregator";
        public List<SwaggerServiceDefinition> Services { get; init; } = []; 
    }
}
