using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using ReLogic.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace CuteKomeijiKoishi.Contents
{
    public class KoishiGlobalNPC : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Clothier) 
            {
                shop.Add(new NPCShop.Entry(ModContent.ItemType<YellowThread>()));
            }
            base.ModifyShop(shop);
        }
    }
    public class YellowThread : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GreenThread);
            Item.shopCustomPrice = Item.buyPrice(0, 1);
        }
    }
    public abstract class MrHat<T> : ModItem where T : HereIsKoishi
    {
        public override void SetStaticDefaults()
        {
            //var path = "Mods.CuteKomeijiKoishi.";
            //DisplayName.SetDefault(Language.GetTextValue(path + "ItemName.MrHat"));
            //Tooltip.SetDefault(path + "ItemTooltip.MrHat");
            //DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.English), "");
            //DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese), "");
            //Tooltip.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.English), "");
            //Tooltip.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese), "");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }
        public override void SetDefaults()
        {
            //Item.DefaultToVanitypet(ModContent.ProjectileType<Koishi>(), ModContent.BuffType<HereIsKoishi>());
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<Koishi>();
            Item.buffType = ModContent.BuffType<T>();
            //Item.buffTime = 3600;
            Item.value = Item.sellPrice(0, 5, 14);
            Item.rare = ItemRarityID.Cyan;
        }
        public virtual Koishi.KoishiStyle KoishiStyle => Koishi.KoishiStyle.Origin;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0);
            var koishi = proj.ModProjectile as Koishi;
            koishi.style = KoishiStyle;
            proj.netUpdate = true;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<YellowThread>(5).AddIngredient(ItemID.Silk, 20).AddIngredient(ItemID.LifeCrystal).AddIngredient(ItemID.NaturesGift).AddTile(TileID.Loom).Register();
            base.AddRecipes();
        }
    }
    public abstract class HereIsKoishi : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //var path = "Mods.CuteKomeijiKoishi.";
            //DisplayName.SetDefault(Language.GetTextValue(path + "BuffName.HereIsKoishi"));
            //Description.SetDefault(Language.GetTextValue(path + "BuffDescription.HereIsKoishi"));
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
        public virtual Koishi.KoishiStyle KoishiStyle => Koishi.KoishiStyle.Origin;
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            //player.fishingSkill += 5140;
            int projType = ModContent.ProjectileType<Koishi>();
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0)
            {
                var entitySource = player.GetSource_Buff(buffIndex);
                var proj = Projectile.NewProjectileDirect(entitySource, player.Center, default, projType, 0, 0f, player.whoAmI);
                var koishi = proj.ModProjectile as Koishi;
                koishi.style = KoishiStyle;
                proj.netUpdate = true;
            }
        }
    }
    public class Koishi : ModProjectile
    {
        //public float[] oldAcc = new float[60];
        //public float[] oldVel = new float[60];
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)state);
            writer.Write((byte)style);
            writer.Write(sleepCounter);
            writer.Write(helpCounter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            state = (KoishiState)reader.ReadByte();
            sleepCounter = reader.ReadInt32();
            helpCounter = reader.ReadInt32();
        }
        public enum KoishiState
        {
            Idle,
            IdleA,
            IdleB,
            IdleC,
            Walk,
            Run,
            Flip,//
            Jump,
            Launch,
            Recover,
            Swim,
            Sleep
        }
        public KoishiState state;
        public enum KoishiStyle
        {
            Origin,
            Satori,
            //Madeline,
            SilverCyanDark,
            DarkGreen,
            Yuki,
            Mai,
            Pink,
            Yumemi,
            Flandre,
            Golden,
            Diamond,
            Silver,
            Copper,
            Crimson,
            //Niko,
            NikoColor,
            MadelineColor,
            PurpleSatori,
            Ash,
            BlackWhite,
            Bright,
            Cold,
            CopperOld,
            Crystal,
            CyanBlack,
            GoldenOld,
            Luminite,
            Meteor,
            Nebula,
            Sky,
            Solar,
            StarDust,
            Vortex,

        }
        public KoishiStyle style;
        public int sleepCounter;
        public int helpCounter;
        public int MaxFrame => state switch
        {
            KoishiState.Idle => 9,
            KoishiState.IdleA => 12,
            KoishiState.IdleB => 24,
            KoishiState.IdleC => 15,
            KoishiState.Walk => 12,
            KoishiState.Run => 12,
            KoishiState.Flip => 9,
            KoishiState.Jump => 4,
            KoishiState.Launch => 8,
            KoishiState.Recover => 11,
            KoishiState.Swim => 18,
            KoishiState.Sleep or _ => 24
        };
        public int FrameCounter
        {
            get => Projectile.frameCounter;
            set => Projectile.frameCounter = value;
        }
        public int Frame
        {
            get => Projectile.frame;
            set => Projectile.frame = value;
        }
        public Texture2D CurrentTex => ModContent.Request<Texture2D>($"CuteKomeijiKoishi/Contents/Textures/{style}/{state}", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        public void ResetFrameData()
        {
            FrameCounter = 0;
            Frame = 0;
            helpCounter = 0;
        }
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            sleepCounter--;
            #region 玩家检测
            var player = Main.player[Projectile.owner];
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (!player.dead && player.HasBuff(ModContent.Find<ModBuff>("CuteKomeijiKoishi", $"{style}_HereIsKoishi").Type))
            {
                Projectile.timeLeft = 2;
            }
            #endregion
            #region 基本量与初始化
            bool goRight = false;
            bool goLeft = false;
            bool jump = false;
            int range = 12;
            #endregion
            #region 位置检测
            float offset = -32 * player.direction;
            float targetX = player.Center.X + offset;
            var cenX = Projectile.Center.X;
            if (targetX < cenX - range)
                goRight = true;
            else if (targetX > cenX + range)
                goLeft = true;
            #endregion
            #region 起飞
            if (player.rocketDelay2 > 0 && state != KoishiState.Recover)
                Projectile.ai[0] = 1f;
            if (Projectile.ai[0] != 0f && state != KoishiState.Recover)
            {
                #region 声明&初始化变量
                float modifyStep = 0.2f;
                int length = 200;
                Projectile.tileCollide = false;
                Vector2 target = player.Center - Projectile.Center;
                float distance = target.Length();
                #endregion
                if (distance > 1440)
                {
                    Projectile.Center = player.Center;
                }
                #region 检测并切换状态
                if (distance < length &&//小于最大距离
                    player.velocity.Y == 0f &&//玩家站在地面上 
                    Projectile.Center.Y <= player.Center.Y && //在玩家中心以上
                    !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)//我不到啊，物块碰撞检测？
                    )
                {
                    //切换至走路状态！
                    state = KoishiState.Recover;
                    Projectile.ai[0] = 0f;
                    Projectile.tileCollide = true;
                    ResetFrameData();
                    Projectile.localAI[0] = player.Center.Y + (player.mount.Active ? player.mount.HeightBoost - 8 : 8);
                    Projectile.localAI[1] = Projectile.Center.Y;
                    int k = 8;
                    while (k > -1)
                    {
                        helpCounter = (int)player.Center.X + k * 16 * player.direction;
                        int i = helpCounter / 16;
                        int n = 0;
                        int j = (int)Projectile.localAI[0] - 128;
                        j /= 16;
                        while (n < 12)
                        {
                            var tile = Main.tile[i, j];
                            if (tile.HasUnactuatedTile && Main.tileSolid[tile.TileType])
                            {
                                //Main.NewText(tile);
                                break;
                            }
                            j++;
                            n++;
                        }
                        if (n != 12 | k == 0)
                        {
                            Projectile.localAI[0] = j * 16 - 12;
                            break;
                        }
                        k--;
                    }

                    if (Projectile.velocity.Y < -6f)
                        Projectile.velocity.Y = -6f;
                    return;
                }
                #endregion
                #region 典中典之速度渐进
                if (distance < 60f)
                {
                    target = Projectile.velocity;
                }
                else
                {
                    target = target.SafeNormalize(default) * 10f;
                }
                if (Projectile.wet)
                {
                    target *= new Vector2(.25f, .1f);
                    state = KoishiState.Swim;
                }
                else
                {
                    state = KoishiState.Launch;
                }

                if (Projectile.velocity.X < target.X)
                {
                    Projectile.velocity.X += modifyStep;
                    if (Projectile.velocity.X < 0f)
                        Projectile.velocity.X += modifyStep * 1.5f;
                }
                if (Projectile.velocity.X > target.X)
                {
                    Projectile.velocity.X -= modifyStep;
                    if (Projectile.velocity.X > 0f)
                        Projectile.velocity.X -= modifyStep * 1.5f;
                }
                if (Projectile.velocity.Y < target.Y)
                {
                    Projectile.velocity.Y += modifyStep;
                    if (Projectile.velocity.Y < 0f)
                        Projectile.velocity.Y += modifyStep * 1.5f;
                }
                if (Projectile.velocity.Y > target.Y)
                {
                    Projectile.velocity.Y -= modifyStep;
                    if (Projectile.velocity.Y > 0f)
                        Projectile.velocity.Y -= modifyStep * 1.5f;
                }
                #endregion
                #region 动画处理
                //state = KoishiState.Launch;
                if (!Projectile.wet)
                    for (int k = 0; k < 10; k++)
                    {
                        float fac = (k + 1) / 10f;
                        var unit = ((FrameCounter - 1 + fac) / 32f * MathHelper.TwoPi * -Projectile.spriteDirection).ToRotationVector2() * new Vector2(16, 4);
                        unit = unit.RotatedBy(Projectile.rotation);
                        for (int n = -1; n < 2; n += 2)
                        {
                            var dust = Dust.NewDustPerfect(Projectile.Center + unit * n + Projectile.velocity + Main.rand.NextVector2Unit(), n == 1 ? DustID.Clentaminator_Cyan : DustID.TheDestroyer, null, 0, Color.White, (1 + fac) * .5f + Main.rand.NextFloat(0, .25f));// 
                            dust.noGravity = true;
                            dust.velocity *= .625f;
                            dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[0].type);
                        }
                    }

                FrameCounter++;
                if (FrameCounter % 4 == 0)
                    Frame++;
                Frame %= MaxFrame;

                //if (Projectile.velocity.X > 0.5)
                //    Projectile.spriteDirection = -1;
                //else if (Projectile.velocity.X < -0.5)
                //    Projectile.spriteDirection = 1;
                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                Projectile.rotation = MathHelper.PiOver2 * Projectile.direction * (1 - 1 / (Math.Abs(Projectile.velocity.X) / 24f + 1));
                //Vector2 velocity3 = Projectile.velocity;
                //velocity3.Normalize();
                //Projectile.rotation = velocity3.ToRotation() + (float)Math.PI / 2f;
                #endregion
            }
            #endregion
            #region 平地
            else //走路ai
            {
                if (state == KoishiState.Recover)
                {
                    //落地
                    if (FrameCounter <= 16f)
                    {
                        float t = FrameCounter / 16f;
                        //模式切换前记录玩家高度0和弹幕高度1
                        if (Projectile.localAI[1] < Projectile.localAI[0]) //高了
                        {
                            Projectile.Center = Vector2.Lerp(new Vector2(Projectile.Center.X, Projectile.localAI[1]), new Vector2(helpCounter, Projectile.localAI[0]), t * t);
                        }
                        else
                        {
                            Projectile.Center = Vector2.Lerp(new Vector2(Projectile.Center.X, Projectile.localAI[1]), new Vector2(helpCounter, Projectile.localAI[0]), -2 * t * (t - 3 / 2f));
                        }
                        Projectile.velocity.X *= .975f;
                        Projectile.velocity.Y = 0;
                        for (int k = 0; k < 10; k++)
                        {
                            float fac = (k + 1) / 10f;
                            var unit = ((FrameCounter - 1 + fac) / 16f * MathHelper.TwoPi).ToRotationVector2() * new Vector2(48, 24);
                            unit = unit.RotatedBy((Projectile.position - Projectile.oldPosition).ToRotation() + MathHelper.PiOver2);

                            for (int n = 0; n < 2; n++)
                            {
                                var dust = Dust.NewDustPerfect(Projectile.Center + unit * (n == 0 ? 1 : -.5f), DustID.Clentaminator_Blue, null, 0, Color.White, 1 + fac * .5f);
                                dust.noGravity = true;
                                dust.velocity *= .75f;
                                dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[0].type);

                            }
                        }

                    }
                    else
                    {
                        Projectile.velocity.X *= .925f;
                    }
                    if (FrameCounter == 16)
                    {
                        for (int n = 0; n < 30; n++)
                        {
                            Dust.NewDustPerfect(Projectile.Center, DustID.Clentaminator_Blue, -Main.rand.NextFloat(0, MathHelper.Pi).ToRotationVector2() * Main.rand.NextFloat(0, 8f), 0, default, Main.rand.NextFloat(.5f, 1.5f)).shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[0].type);
                        }
                        SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);//整活就听62
                    }
                    Projectile.velocity.Y += 0.4f;
                    if (Projectile.velocity.Y > 10f)
                        Projectile.velocity.Y = 10f;
                    FrameCounter++;
                    if (FrameCounter % 4 == 0)
                        Frame++;

                    Projectile.rotation *= .25f;
                    if (Frame == MaxFrame)
                    {
                        Projectile.rotation = 0;
                        state = KoishiState.Idle;
                        ResetFrameData();
                    }
                }
                else
                {
                    var dis = Vector2.Distance(Projectile.Center, player.Center);
                    if (dis > 1440)
                    {
                        state = KoishiState.Recover;
                        ResetFrameData();
                        Projectile.localAI[0] = player.Center.Y + (player.mount.Active ? player.mount.HeightBoost - 8 : 8);
                        Projectile.localAI[1] = Projectile.Center.Y - 64;
                    }
                    else if (dis > 256) Projectile.ai[0] = 1;
                    #region 走路/跑步/转向
                    float acc = 0.2f;
                    float speedMax = 6f;
                    if (speedMax < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                    {
                        speedMax = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                        acc = 0.3f;
                    }
                    if (goRight)
                    {
                        if (Projectile.velocity.X > -3.5)
                            Projectile.velocity.X -= acc;
                        else
                            Projectile.velocity.X -= acc * 0.25f;
                    }
                    else if (goLeft)
                    {
                        if (Projectile.velocity.X < 3.5)
                            Projectile.velocity.X += acc;
                        else
                            Projectile.velocity.X += acc * 0.25f;
                    }
                    else
                    {
                        Projectile.velocity.X *= 0.9f;
                        if (Projectile.velocity.X >= 0f - acc && Projectile.velocity.X <= acc)
                            Projectile.velocity.X = 0f;
                    }
                    #endregion
                    #region 跳跃判定
                    if (goRight || goLeft)
                    {
                        Point point = (Projectile.Center / 16).ToPoint();
                        if (goRight)
                            point -= new Point(1, 0);

                        if (goLeft)
                            point += new Point(1, 0);

                        point += new Point((int)Projectile.velocity.X, 0);
                        if (WorldGen.SolidTile(point))
                            jump = true;
                    }
                    #endregion
                    Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
                    if (Projectile.velocity.Y == 0f) //站在地面上
                    {
                        //if (!flag4 && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
                        //{
                        //    int num163 = (int)(Projectile.position.X + Projectile.width / 2) / 16;
                        //    int j3 = (int)(Projectile.position.Y + Projectile.height / 2) / 16 + 1;
                        //    if (goRight)
                        //        num163--;

                        //    if (goLeft)
                        //        num163++;

                        //    WorldGen.SolidTile(num163, j3);
                        //}

                        if (jump && state != KoishiState.Flip)
                        {
                            //TODO 加入jump状态
                            int num164 = (int)(Projectile.position.X + Projectile.width / 2) / 16;
                            int num165 = (int)(Projectile.position.Y + Projectile.height) / 16;
                            if (WorldGen.SolidTileAllowBottomSlope(num164, num165) || Main.tile[num164, num165].IsHalfBlock || Main.tile[num164, num165].Slope > 0)
                            {
                                try
                                {
                                    num164 = (int)(Projectile.position.X + Projectile.width / 2) / 16;
                                    num165 = (int)(Projectile.position.Y + Projectile.height / 2) / 16;
                                    if (goRight)
                                        num164--;

                                    if (goLeft)
                                        num164++;

                                    num164 += (int)Projectile.velocity.X;
                                    if (!WorldGen.SolidTile(num164, num165 - 1) && !WorldGen.SolidTile(num164, num165 - 2))
                                        Projectile.velocity.Y = -5.1f;
                                    else if (!WorldGen.SolidTile(num164, num165 - 2))
                                        Projectile.velocity.Y = -7.1f;
                                    else if (WorldGen.SolidTile(num164, num165 - 5))
                                        Projectile.velocity.Y = -11.1f;
                                    else if (WorldGen.SolidTile(num164, num165 - 4))
                                        Projectile.velocity.Y = -10.1f;
                                    else
                                        Projectile.velocity.Y = -9.1f;
                                }
                                catch
                                {
                                    Projectile.velocity.Y = -9.1f;
                                }
                            }
                            state = KoishiState.Jump;
                            ResetFrameData();
                        }
                    }
                    #region 速度与朝向处理
                    if (Projectile.velocity.X > speedMax)
                        Projectile.velocity.X = speedMax;

                    if (Projectile.velocity.X < -speedMax)
                        Projectile.velocity.X = -speedMax;

                    //if (Projectile.velocity.X < 0f)
                    //    Projectile.direction = -1;

                    //if (Projectile.velocity.X > 0f)
                    //    Projectile.direction = 1;

                    //if (Projectile.velocity.X > acc && goLeft)
                    //    Projectile.direction = 1;

                    //if (Projectile.velocity.X < 0f - acc && goRight)
                    //    Projectile.direction = -1;

                    //if (Projectile.direction == -1)
                    //    Projectile.spriteDirection = 1;

                    //if (Projectile.direction == 1)
                    //    Projectile.spriteDirection = -1;
                    #endregion
                    #region 动画处理
                    //Projectile.spriteDirection = Projectile.direction;
                    #region 翻转
                    #region 放弃
                    //var dVeloX = Projectile.velocity.X - Projectile.oldVelocity.X;
                    //Main.NewText(dVeloX);
                    //var accDir = Math.Sign(dVeloX);
                    //if (accDir * Projectile.spriteDirection < 0 && state != KoishiState.Flip && Math.Abs(dVeloX) > 1f)
                    //{
                    //    state = KoishiState.Flip;
                    //    ResetFrameData();
                    //    Projectile.direction = Projectile.spriteDirection = accDir;
                    //    helpCounter = accDir;
                    //    Main.NewText((accDir, Projectile.spriteDirection, Projectile.velocity.X - Projectile.oldVelocity.X));
                    //}
                    //if (state == KoishiState.Flip)
                    //{
                    //    FrameCounter += accDir * helpCounter;
                    //    FrameCounter++;
                    //    Frame = FrameCounter / 4;
                    //    if (FrameCounter < 0 || FrameCounter >= 36)
                    //    {
                    //        if (FrameCounter < 0) Projectile.direction = Projectile.spriteDirection = -helpCounter;
                    //        state = KoishiState.Walk;
                    //        ResetFrameData();
                    //    }
                    //}
                    #endregion
                    var dVx = Projectile.velocity.X - Projectile.oldVelocity.X;//计算速度变化量
                    var tarX = player.Center.X - Projectile.Center.X;
                    //Main.NewText((tarX * Projectile.velocity.X < 0, dVx * tarX > 0, state != KoishiState.Flip));
                    if (tarX * Projectile.velocity.X < 0 && dVx * tarX > 0 && state != KoishiState.Flip) //
                    {
                        state = KoishiState.Flip;
                        ResetFrameData();
                        helpCounter = (int)(Projectile.velocity.X * 128);
                    }
                    if (state == KoishiState.Flip)
                    {
                        //if (dVx * helpCounter > 0) //转身被打断
                        //{
                        //    Main.NewText(dVx);
                        //    state = KoishiState.Walk;
                        //    ResetFrameData();
                        //}
                        var t = Projectile.velocity.X * 128 / helpCounter;//应该是 逐渐从1降低至0的
                        Projectile.spriteDirection = -Math.Sign(helpCounter);
                        if (t <= 0) //已经减小至0，退出转身状态
                        {
                            //Projectile.spriteDirection = Math.Sign(helpCounter);
                            state = KoishiState.Walk;
                            ResetFrameData();

                        }
                        else if (t > 1) //转回去    ////应该不会发生
                        {
                            //state = KoishiState.Walk;
                            //ResetFrameData();
                            t = 1;
                        }
                        //if (helpCounter != 0)
                        Frame = (int)((1 - t) * 9);
                        if (Frame > MaxFrame) { Frame = MaxFrame - 1; }

                    }
                    #endregion
                    #region 跳跃
                    else if (state == KoishiState.Jump)
                    {
                        Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                        if (Projectile.velocity.Y != 0)
                        {
                            if (helpCounter < 15)
                                helpCounter++;
                            Frame = helpCounter / 4;
                        }
                        else
                        {
                            //if (helpCounter > 0)
                            //{
                            //    helpCounter--;
                            //}
                            //Frame = 3 - helpCounter / 3;
                            //if (helpCounter == 0) 
                            //{
                            state = KoishiState.Walk;
                            ResetFrameData();
                            //}
                        }
                    }
                    #endregion
                    #region 站立
                    else if (Projectile.oldPosition == Projectile.position)
                    {
                        sleepCounter += 2;
                        //Projectile.direction = player.direction;
                        //Projectile.spriteDirection = Projectile.direction;

                        if (sleepCounter >= 3600)
                        {
                            if (state != KoishiState.Sleep)
                            {
                                state = KoishiState.Sleep;
                                ResetFrameData();
                            }
                            FrameCounter++;
                            if (FrameCounter % 4 == 0) Frame++;
                            if (Frame == MaxFrame)
                            {
                                Frame = MaxFrame - 1;
                            }
                        }
                        else
                        {
                            if ((int)state > 3) //进入直立状态，初始化
                            {
                                state = KoishiState.Idle;
                                ResetFrameData();
                            }
                            FrameCounter++;
                            if (FrameCounter % 6 == 0) Frame++;
                            if (Frame == MaxFrame)
                            {
                                Frame = 0;
                                helpCounter++;
                                if (helpCounter > 3 && Main.rand.NextBool(3))
                                {
                                    state = (KoishiState)Main.rand.Next(1, 4);
                                }
                                else
                                {
                                    state = KoishiState.Idle;
                                }
                            }
                        }

                    }
                    #endregion
                    #region 移动
                    else
                    {
                        //Main.NewText((Projectile.spriteDirection, Math.Sign(Projectile.velocity.X - Projectile.oldVelocity.X)));
                        //if (state != KoishiState.Flip && ((Projectile.velocity.X - Projectile.oldVelocity.X) * Projectile.spriteDirection < 0))//Projectile.velocity.X * Projectile.oldVelocity.X < 0 ||
                        //{
                        //    state = KoishiState.Flip;
                        //    ResetFrameData();
                        //}
                        //if (state == KoishiState.Flip)
                        //{
                        //    FrameCounter++;
                        //    if (FrameCounter % 4 == 0) Frame++;
                        //    if (Frame == MaxFrame)
                        //    {
                        //        state = KoishiState.Run;
                        //        //Projectile.spriteDirection *= -1;
                        //        //if (Projectile.velocity.X * Projectile.spriteDirection < 0)
                        //        //{
                        //        //    Projectile.velocity.X *= -1;
                        //        //}
                        //        ResetFrameData();
                        //    }
                        //}
                        //else
                        //{
                        if (Projectile.velocity.Y != 0) { state = KoishiState.Jump; ResetFrameData(); }
                        int step = 1 + (int)Math.Abs(Projectile.velocity.X * .2f);
                        if (step > 2) step = 2;
                        FrameCounter += step;
                        while (FrameCounter > 3)
                        {
                            FrameCounter -= 4;
                            Frame++;
                        }
                        if (Frame >= MaxFrame) Frame = 0;
                        if (Math.Abs(Projectile.velocity.X) < 4) state = KoishiState.Walk;
                        else state = KoishiState.Run;
                        if (Math.Sign(Projectile.velocity.X) != 0)
                            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                        if (state == KoishiState.Run || Main.rand.NextBool(3))
                        {
                            var dust = Dust.NewDustPerfect(Projectile.Center + new Vector2(Projectile.velocity.X, 12), DustID.Clentaminator_Blue, default, 0, default, Main.rand.NextFloat(.5f, 1.5f));
                            dust.velocity *= .25f;
                            dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[0].type);
                        }

                        //}
                    }
                    #endregion


                    #endregion
                    #region 迫真重力
                    Projectile.velocity.Y += 0.4f;
                    if (Projectile.velocity.Y > 10f)
                        Projectile.velocity.Y = 10f;
                    #endregion
                }
            }
            #endregion
            if (sleepCounter < 0) sleepCounter = 0;
            if (sleepCounter > 3600) sleepCounter = 3600;
            if ((int)Main.GlobalTimeWrappedHourly % 30 == 0) Projectile.netUpdate = true;
            //Projectile.oldSpriteDirection[0] = Projectile.spriteDirection;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, TextureAssets.Projectile[Type].Frame(1, 15, 0, Projectile.frame), lightColor, Projectile.rotation, new Vector2(23, 40), 1f, Projectile.spriteDirection == 1 ? 0 : SpriteEffects.FlipHorizontally, 0);
            //Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CuteKomeijiKoishi/icon").Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(40), 1f, 0, 0);
            ////Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 1, 1), Color.Red, 0, new Vector2(.5f), 4f, 0, 0);
            ////Main.spriteBatch.DrawString(FontAssets.MouseText.Value, (Projectile.ai[0], Projectile.ai[1], Projectile.localAI[0], Projectile.localAI[1], Projectile.tileCollide).ToString(), Projectile.Center - Main.screenPosition + new Vector2(0, 32), Main.DiscoColor);
            ////foreach (var proj in Main.projectile)
            ////{
            ////    if (proj.type == 891)
            ////    {
            ////        Main.spriteBatch.DrawString(FontAssets.MouseText.Value, (proj.ai[0], proj.ai[1], proj.localAI[0], proj.localAI[1], proj.tileCollide).ToString(), proj.Center - Main.screenPosition + new Vector2(0, 64), Main.DiscoColor * .5f);
            ////        break;
            ////    }
            ////}
            //if (Frame >= MaxFrame || Frame < 0)
            //{
            //    var wth = (Frame, MaxFrame, state);
            //    _ = wth;
            //    Main.NewText("动画出问题了，请联系阿汪！", Color.Blue);
            //    Main.NewText("The Animation has something wrong, please contact LogSpiral in time.", Color.Blue);

            //}
            if (Frame >= MaxFrame) Frame = MaxFrame - 1;
            if (Frame < 0) Frame = 0;
            //Main.NewText((Frame, MaxFrame, state));
            //_ = 0;
            //var p = ModContent.Request<Texture2D>($"CuteKomeijiKoishi/Contents/Textures/{style}/{state}",ReLogic.Content.AssetRequestMode.ImmediateLoad);
            //var set = (false, false, false, false);
            //if (p != null)
            //{
            //    set = (p.IsLoaded, p.IsDisposed, p.Value.IsDisposed, false);
            //}
            //else set.Item4 = true;
            //Main.NewText(set);
            //Main.NewText((Frame, MaxFrame));
            Main.EntitySpriteDraw(CurrentTex, Projectile.Center - Main.screenPosition, new Rectangle(0, 32 * Projectile.frame, 32, 32), lightColor, Projectile.rotation, new Vector2(16, 26), 2f, Projectile.spriteDirection == 1 ? 0 : SpriteEffects.FlipHorizontally, 0);
            //_ = 0;
            //Main.spriteBatch.DrawString(FontAssets.MouseText.Value, (Projectile.ai[0], Projectile.ai[1], Projectile.localAI[0], Projectile.localAI[1], Projectile.tileCollide).ToString(), Projectile.Center - Main.screenPosition + new Vector2(0, 32), Main.DiscoColor);
            //Main.spriteBatch.DrawString(FontAssets.MouseText.Value, (Frame, MaxFrame, state, Projectile.spriteDirection, Main.player[Projectile.owner].velocity).ToString(), Projectile.Center - Main.screenPosition + new Vector2(0, 48), Main.DiscoColor);
            return false;
        }
    }
}
