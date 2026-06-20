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
        
        // este campo nos ayuda que el swagger del microservicio 
        // apunte a la direccion del api-gateway y no al servicio en concreto
        public string? GatewayPublicUrl { get; set; }

        public List<SwaggerServiceDefinition> Services { get; init; } = []; 
    }
}
