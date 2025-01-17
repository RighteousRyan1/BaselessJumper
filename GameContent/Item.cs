﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaselessJumping.GameContent.Behaviour;
using BaselessJumping.GameContent.Physics;
using BaselessJumping.Internals;
using BaselessJumping.Internals.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaselessJumping.GameContent
{
    public class Item : Entity
    {
        // to be completed

        internal static Item[] items = new Item[50];

        public const int TOTAL_ITEMS = 50;

        #region Shared Metadata

        public static float grabRange = 50f;

        #endregion

        #region Metadata

        public int id;
        public int worldId;
        public Player owner; // null if no player owns it
        public ISource spawnSource;
        public bool inInventory;

        #endregion

        #region Stats

        public int width;
        public int height;

        public double damage;

        public float scale;
        public float rotation;

        public string Description { get; set; }
        public string Name { get; set; }

        public Color Rarity { get; set; }

        public Rectangle hitbox;
        // position is (-1, -1) if owned by a player

        #endregion

        private Item(int type)
        {
            int _len = 0;
            foreach (var item in items)
                if (item != null)
                    _len++;

            id = type;
            worldId = _len;
            var tex = GameAssets.ItemTexture[type];
            width = tex.Width;
            height = tex.Height;
            spawnSource = GameSources.Create();

            items[_len] = this;
        }

        public void Draw()
        {
            OnPreDraw?.Invoke(this, new());

            Base.spriteBatch.Draw(GameAssets.ItemTexture[id], position, null, Color.White, rotation, GameAssets.ItemTexture[id].Size() / 2, scale, default, 0f);
            GameUtils.DrawStringQuick(this, position + new Vector2(0, 25));
            // DrawUtils.DrawDebugBox(hitbox);

            OnPostDraw?.Invoke(this, new());
        }

        public void UpdateWorld()
        {
            if (!inInventory)
            {
                velocity *= 0.9f;

                Update_PlayerGrab();

                hitbox = new((int)position.X - width / 2, (int)position.Y - height / 2, width, height);

                position += velocity;

                inInventory = false;

                owner = null;
            }
        }

        public void Update_PlayerGrab()
        {
            Player.AllPlayers.ForEach(player =>
            {
                if (player.pickupCooldowns[id].ElapsedGameTicks > Player.PICKUP_RESET_SATISFACTION)
                {
                    var dist = player.Distance(this);

                    if (dist < grabRange)
                    {
                        player.GrabItem(this, out var pickedUp);

                        if (pickedUp)
                        {
                            items[worldId] = null;
                        }
                    }
                }
            });
        }

        public void PlaceInPlayerInventory(ref Player player)
        {
            position = new(-1, -1);
            owner = player;
            inInventory = true;
        }

        public static Item CreateNew(int texId, Vector2 position)
        {
            var item = new Item(texId);

            item.position = position;
            item.scale = 1f;
            return item;
        }

        /// <summary>
        /// Copy the data of item1 to item2.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public static void CopyDataTo(ref Item item1, ref Item item2)
        {
            item2.damage = item1.damage;
            item2.Description = item1.Description;
            item2.Rarity = item1.Rarity;
            item2.scale = item1.scale;
            item2.Name = item1.Name;
            item2.id = item1.id;
        }

        public override string ToString()
        {
            return $"id: {id} | worldId: {worldId} | name: {Name}";
        }

        /// <summary>
        /// <c>SpriteBatch.Begin</c> is not called.
        /// </summary>
        public event EventHandler OnPreDraw;
        /// <summary>
        /// <c>SpriteBatch.End</c> is not called yet.
        /// </summary>
        public event EventHandler OnPostDraw;
    }
}
