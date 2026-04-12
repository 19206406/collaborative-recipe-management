namespace BuildingBlocks.Preferences
{
    public static class RecipeTagMapper
    {
        private static readonly Dictionary<string, HashSet<string>> _restrictionKeywords = new()
        {
            [PreferenceTypes.GlutenFree] = ["harina", "trigo", "cebada", "centeno", "gluten", "wheat", "flour"],
            [PreferenceTypes.LactoseFree] = ["leche", "queso", "mantequilla", "crema", "yogur", "milk", "cheese", "butter", "cream"],
            [PreferenceTypes.NutAllergy] = ["nuez", "almendra", "maní", "cacahuete", "pistacho", "nut", "almond", "peanut"],
            [PreferenceTypes.SeafoodAllergy] = ["camarón", "langosta", "cangrejo", "atún", "salmón", "shrimp", "lobster", "crab"],
            [PreferenceTypes.EggFree] = ["huevo", "egg"],
        };

        private static readonly Dictionary<string, HashSet<string>> _dietKeywords = new()
        {
            [PreferenceTypes.Vegan] = ["pollo", "carne", "cerdo", "res", "leche", "huevo", "chicken", "beef", "pork", "milk", "egg"],
            [PreferenceTypes.Vegetarian] = ["pollo", "carne", "cerdo", "res", "chicken", "beef", "pork"],
            [PreferenceTypes.Keto] = ["arroz", "pasta", "pan", "papa", "maíz", "rice", "pasta", "bread", "potato", "corn"],
            [PreferenceTypes.LowCarb] = ["arroz", "pasta", "pan", "papa", "rice", "pasta", "bread", "potato"],
            [PreferenceTypes.Paleo] = ["arroz", "pasta", "pan", "legumbres", "rice", "pasta", "bread", "legumes"],
        };

        public static bool ViolatesRestriction(string preferenceType, IEnumerable<string> ingredientNames)
        {
            if (!_restrictionKeywords.TryGetValue(preferenceType, out var keywords))
                return false;

            return ingredientNames.Any(i =>
                keywords.Any(k => i.Contains(k, StringComparison.OrdinalIgnoreCase)));
        }

        public static bool ViolatesDiet(string preferenceType, IEnumerable<string> ingredientNames)
        {
            if (!_dietKeywords.TryGetValue(preferenceType, out var keywords))
                return false;

            return ingredientNames.Any(i =>
                keywords.Any(k => i.Contains(k, StringComparison.OrdinalIgnoreCase)));
        }

        public static bool TagMatchesPreference(string tag, string preferenceType)
        {
            return string.Equals(tag.Trim(), preferenceType.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
