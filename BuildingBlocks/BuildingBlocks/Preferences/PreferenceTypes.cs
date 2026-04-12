namespace BuildingBlocks.Preferences
{
    public static class PreferenceTypes
    {
        // ── Dietas (bonus) ──────────────────────────────────────
        public const string Vegan = "vegan";
        public const string Vegetarian = "vegetarian";
        public const string Keto = "keto";
        public const string LowCarb = "low-carb";
        public const string Paleo = "paleo";

        // ── Restricciones / alergias (filtro duro) ──────────────
        public const string GlutenFree = "gluten-free";
        public const string LactoseFree = "lactose-free";
        public const string NutAllergy = "nut-allergy";
        public const string SeafoodAllergy = "seafood-allergy";
        public const string EggFree = "egg-free";


        public static readonly IReadOnlySet<string> Restrictions = new HashSet<string>
        {
            GlutenFree, LactoseFree, NutAllergy, SeafoodAllergy, EggFree
        };

        public static readonly IReadOnlySet<string> Diets = new HashSet<string>
        {
            Vegan, Vegetarian, Keto, LowCarb, Paleo
        }; 
    }
}
