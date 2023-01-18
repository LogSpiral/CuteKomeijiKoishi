using CuteKomeijiKoishi.Contents.HereIsKoishies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace CuteKomeijiKoishi.Contents.MrHats
{
    public class Origin_MrHat : MrHat<Origin_HereIsKoishi>
    {
    }
    public class Satori_MrHat : MrHat<Satori_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.BrightPinkDye).AddTile(TileID.Loom).Register();
        }
    }
    public class SilverCyanDark_MrHat : MrHat<SilverCyanDark_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.CyanandBlackDye).AddTile(TileID.Loom).Register();
        }
    }
    public class DarkGreen_MrHat : MrHat<DarkGreen_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.GreenandBlackDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Yuki_MrHat : MrHat<Yuki_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.SilverDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Mai_MrHat : MrHat<Mai_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.BrightCyanDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Pink_MrHat : MrHat<Pink_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.PinkDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Yumemi_MrHat : MrHat<Yumemi_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.RedandSilverDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Flandre_MrHat : MrHat<Flandre_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.FlameDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Golden_MrHat : MrHat<Golden_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.ReflectiveGoldDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Diamond_MrHat : MrHat<Diamond_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.CyanDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Silver_MrHat : MrHat<Silver_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.ReflectiveSilverDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Copper_MrHat : MrHat<Copper_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.ReflectiveCopperDye).AddTile(TileID.Loom).Register();
        }
    }
    public class Crimson_MrHat : MrHat<Crimson_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.RedandBlackDye).AddTile(TileID.Loom).Register();
        }
    }
    public class NikoColor_MrHat : MrHat<NikoColor_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.BrownDye).AddTile(TileID.Loom).Register();
        }
    }
    public class MadelineColor_MrHat : MrHat<MadelineColor_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Origin_MrHat>().AddIngredient(ItemID.BlueDye).AddTile(TileID.Loom).Register();
        }
    }
    public class PurpleSatori_MrHat : MrHat<PurpleSatori_HereIsKoishi>
    {
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Satori_MrHat>().AddIngredient(ItemID.PurpleDye).AddTile(TileID.Loom).Register();
        }
    }

}
