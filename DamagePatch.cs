using NLog;
using NLog.Fluent;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using Torch.API.Managers;
using Torch.API.ModAPI;
using Torch.Mod;
using Torch.Mod.Messages;
using VRage.Game;
using VRage.Game.ModAPI;

namespace DeathCounterRecounted
{
    public static class DamagePatch
    {

        private static readonly Random Random = new Random();

        private static bool _init;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static ConcurrentDictionary<ulong, int> DeathCounterRecounted = new ConcurrentDictionary<ulong, int>();
        public static ConcurrentDictionary<ulong, int> KillStreak = new ConcurrentDictionary<ulong, int>();
        public static ulong _lastKiller;
        public static ConcurrentDictionary<ulong, int> DeathStreak = new ConcurrentDictionary<ulong, int>();

        private static readonly Config Config = Core.Instance.Config;

        public static void Init()
        {
            if (_init) return;

            _init = true;

            MyAPIGateway.Session.DamageSystem.RegisterBeforeDamageHandler(1, ProcessDamage);
        }

        private static void ProcessDamage(object target, ref MyDamageInformation info)
        {
            var character = target as MyCharacter;
            if (character == null)
                return;
            var identity = character.GetIdentity();

            if (identity == null)
            {
                Log.Warn($"Character has no identity: {character.DisplayName}");
                return;
            }

            var steamId = MySession.Static.Players.TryGetSteamId(identity.IdentityId);
            string victim = "";
            string causeOfDeath = "";
            if (character.Integrity - info.Amount > 0) return;
            victim = character?.DisplayName;
            if (info.Type != null) causeOfDeath = info.Type.String;
            var attackingIdentity = MySession.Static.Players.TryGetIdentity(info.AttackerId);

            // now removing suicide from death streak means pressing backspace wont increase ur death streak.

            bool isSuicide = causeOfDeath.Equals("suicide", StringComparison.OrdinalIgnoreCase);

            var playerId = new MyPlayer.PlayerId(steamId);
            MyPlayer player = MySession.Static.Players.GetPlayerById(playerId);

            bool victim_is_real_player = (player != null) && (player.IsRealPlayer);
            if (victim_is_real_player && !isSuicide)
            {
                DeathCounterRecounted.AddOrUpdate(steamId, 1, (l, i) => i + 1);

                DeathStreak.AddOrUpdate(steamId, 1, (l, i) => i + 1);

                var currentDeathStreak = DeathStreak[steamId];

                if (Config.AnnounceDeathStreak &&
                    currentDeathStreak >= 5 &&
                    currentDeathStreak % 5 == 0)
                {
                    var msg = Config.DeathStreakMessages[
                        Random.Next(Config.DeathStreakMessages.Count)];

                    msg = msg.Replace("{PLAYER}", character.DisplayName)
                             .Replace("{COUNT}", currentDeathStreak.ToString());

                    MyVisualScriptLogicProvider.SendChatMessage(msg);
                }
            }
            if (MyEntities.TryGetEntityById(info.AttackerId, out var attacker, true))
            {
                switch (attacker)
                {
                    case MyCubeBlock block:
                        attackingIdentity = MySession.Static.Players.TryGetIdentity(block.CubeGrid.BigOwners.FirstOrDefault());
                        break;
                    case MyVoxelBase _:

                        // exceptoion damages causes.
                        if (causeOfDeath.Equals("Explosion", StringComparison.OrdinalIgnoreCase))
                            break;

                        if (!Config.AnnounceCollisionDeath)
                            return;

                        causeOfDeath = Config.VoxelMessages[Random.Next(Config.VoxelMessages.Count)];
                        victim = string.Empty;
                        attackingIdentity = character.GetIdentity();
                        break;
                    case MyCubeGrid grid:
                        if (!Config.AnnounceCollisionDeath) return;
                        causeOfDeath = Config.GridMessages[Random.Next(Config.GridMessages.Count)];
                        attackingIdentity = MySession.Static.Players.TryGetIdentity(grid.BigOwners.FirstOrDefault());
                        break;
                    case IMyHandheldGunObject<MyGunBase> rifle:
                        attackingIdentity = MySession.Static.Players.TryGetIdentity(rifle.OwnerIdentityId);
                        break;
                    case IMyHandheldGunObject<MyToolBase> handTool:
                        attackingIdentity = MySession.Static.Players.TryGetIdentity(handTool.OwnerIdentityId);
                        break;
                }

            }
            if (causeOfDeath.Equals("suicide", StringComparison.OrdinalIgnoreCase) && Config.AnnounceSuicide)
            {
                causeOfDeath = Config.SuicideMessages[Random.Next(Config.SuicideMessages.Count)];
                victim = string.Empty;
                attackingIdentity = character.GetIdentity();
            }

            if (causeOfDeath.Equals("lowpressure", StringComparison.OrdinalIgnoreCase))
            {
                if (!Config.AnnounceSuffocationDeath) return;
                causeOfDeath = Config.SuffocationMessages[Random.Next(Config.SuffocationMessages.Count)];
                victim = string.Empty;
                attackingIdentity = character.GetIdentity();
            }

            if (causeOfDeath.Equals("Hunger", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.HungerMessages[Random.Next(Config.HungerMessages.Count)];
                victim = string.Empty;
                attackingIdentity = character.GetIdentity();
            }

            if (causeOfDeath.Equals("Radioactivity", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.RadioactivityMessages[Random.Next(Config.RadioactivityMessages.Count)];
                victim = string.Empty;
                attackingIdentity = character.GetIdentity();
            }
            if (causeOfDeath.Equals("Environment", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.EnvironmentMessages[Random.Next(Config.EnvironmentMessages.Count)];
                victim = string.Empty;
                attackingIdentity = character.GetIdentity();
            }

            if (causeOfDeath.Equals("Weather", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.WeatherMessages[Random.Next(Config.WeatherMessages.Count)];
                victim = string.Empty;
                attackingIdentity = character.GetIdentity();
            }
            if (causeOfDeath.Equals("Spider", StringComparison.OrdinalIgnoreCase) ||
                causeOfDeath.Equals("Wolf", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.CreatureDeathMessages[
                    Random.Next(Config.CreatureDeathMessages.Count)];

                victim = string.Empty;

                // Make victim appear as the "actor" in the message
                attackingIdentity = character.GetIdentity();
            }
            if (attackingIdentity == null) return;

            if (causeOfDeath.Equals("Bullet", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.ProjectileMessages[Random.Next(Config.ProjectileMessages.Count)]; ;
            }

            if (causeOfDeath.Equals("Weld", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.WeldMessages[Random.Next(Config.WeldMessages.Count)]; ;
            }

            if (causeOfDeath.Equals("Drill", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.DrillMessages[Random.Next(Config.DrillMessages.Count)]; ;
            }

            if (causeOfDeath.Equals("Explosion", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.ExplosionMessages[Random.Next(Config.ExplosionMessages.Count)]; ;
            }

            if (causeOfDeath.Equals("Grind", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.GrindMessages[Random.Next(Config.GrindMessages.Count)]; ;
            }

            if (causeOfDeath.Equals("Thruster", StringComparison.OrdinalIgnoreCase))
            {
                causeOfDeath = Config.ThrusterMessages[Random.Next(Config.ThrusterMessages.Count)]; ;
                victim = string.Empty;
                attackingIdentity = character.GetIdentity();
            }

            var attackerSteamId = MySession.Static.Players.TryGetSteamId(attackingIdentity.IdentityId);

            Log.Info($"DeathCounterRecounted DEBUG | FinalMessage={attackingIdentity.DisplayName} {causeOfDeath} {victim}");

            MyVisualScriptLogicProvider.SendChatMessage($"{attackingIdentity.DisplayName} {causeOfDeath} {victim}");
            if (DeathCounterRecounted.TryGetValue(steamId, out var deaths))
            {
                Log.Info($"{character.DisplayName} died = {deaths} total death");
            }
            if (((steamId <= 0 || attackerSteamId <= 0) && string.IsNullOrEmpty(victim)) ||
                steamId == attackerSteamId) return;
            DeathStreak.TryRemove(attackerSteamId, out _);
            if (_lastKiller == attackerSteamId)
            {
                KillStreak.AddOrUpdate(attackerSteamId, 2, (l, i) => i + 1);

                MyVisualScriptLogicProvider.SendChatMessage($"{attackingIdentity.DisplayName} on {KillStreak[attackerSteamId]} kill streak");


            }
            else
            {
                KillStreak.Clear();
                KillStreak[attackerSteamId] = 1;
            }

            _lastKiller = attackerSteamId;
        }
    }

}