using Sandbox.Game.Screens.Models;
using System.Collections.Generic;
using System.Web;
using Torch;
using Torch.Views;

namespace DeathCounterRecounted
{
    public class Config : ViewModel
    {
        private List<string> _voxelMessages;
        [Display(Order = 6, Name = "Voxel Messages", Description = " Random messages for Death caused by Hiting the voxels")]
        public List<string> VoxelMessages
        {
            get => _voxelMessages;
            set => SetValue(ref _voxelMessages, value); }

        private List<string> _drillMessages;
        [Display(Order = 7,Name = "Drill Messages", Description = "Random messages for Death caused by Drill")]
        public List<string> DrillMessages
        {
            get => _drillMessages;
            set => SetValue(ref _drillMessages, value);
        }

        private List<string> _explosionMessages;
        [Display(Order = 8,Name = "Explosion Messages", Description = "Random messages for Death caused by Explosion")]
        public List<string> ExplosionMessages
        {
            get => _explosionMessages;
            set => SetValue(ref _explosionMessages, value);
        }
        
        private List<string> _grindMessages;
        [Display(Order = 9,Name = "Grind Messages", Description = "Random messages for Death caused by Grinder")]
        public List<string> GrindMessages
        {
            get => _grindMessages;
            set => SetValue(ref _grindMessages, value);
        }

        private List<string> _weldMessages;
        [Display(Order = 10,Name = "Weld Messages", Description = "Random messages for Death caused by Welder")]
        public List<string> WeldMessages
        {
            get => _weldMessages;
            set => SetValue(ref _weldMessages, value);
        }

        private List<string> _thrusterMessages;
        [Display(Order = 11,Name = "Thruster Messages", Description = "Random messages for Death caused by Thruster")]
        public List<string> ThrusterMessages
        {
            get => _thrusterMessages;
            set => SetValue(ref _thrusterMessages, value);
        }

        private List<string> _suicideMessages;
        [Display(Order = 12,Name = "Suicide Messages", Description = "Random Messages for Death caused by Suicide")]
        public List<string> SuicideMessages
        {
            get => _suicideMessages;
            set => SetValue(ref _suicideMessages, value);
        }

        private List<string> _gridmessages;
        [Display(Order = 13,Name = "Grid Messages", Description ="Random Messages for Death caused by coliding to a grid")]
        public List<string> GridMessages
        {
            get => _gridmessages;
            set => SetValue(ref _gridmessages, value);
        }

        private List<string> _suffocationmessages;
        [Display(Order = 14,Name = "Suffocation Messages", Description = "Random Messages for Death caused by Suffocation ")]
        public List<string> SuffocationMessages
        {
            get => _suffocationmessages;
            set => SetValue(ref _suffocationmessages, value);
        }

        private List<string> _projectilemessages;
        [Display(Order = 15,Name = "Projectile Messages", Description = "Random Messages for Death caused by bullets")]
        public List<string> ProjectileMessages
        {
            get => _projectilemessages;
            set => SetValue(ref _projectilemessages, value);
        }

        private List<string> _hungermessages;
        [Display(Order = 16,Name = "Hunger Messages", Description ="Random Messages for Death caused by Starvation")]
        public List<string> HungerMessages
        {
            get => _hungermessages;
            set => SetValue(ref _hungermessages, value);
        }

        private List<string> _radioactivitymessages;
        [Display(Order = 17,Name = "Raditaion Messages", Description ="Random Messages for Death caused by Radition exposure")]
        public List<string> RadioactivityMessages
        {
            get => _radioactivitymessages;
            set => SetValue(ref _radioactivitymessages, value);
        }

        private List<string> _weathermessages;
        [Display(Order = 18,Name = "Weather Messages", Description ="Random Messages for Death caused by Weather")]
        public List<string> WeatherMessages
        {
            get => _weathermessages;
            set => SetValue(ref _weathermessages, value);
        }

        private List<string> _environmentmessages;
        [Display(Order = 19,Name = "Environmental Messages", Description ="Random Messages for Death caused by Environmental")]
        public List<string> EnvironmentMessages
        {
            get => _environmentmessages;
            set => SetValue(ref _environmentmessages, value);
        }

        private List<string> _creaturemessages;
        [Display(Order = 20,Name = "Creature Death Messages", Description ="Random Messages for Death caused by NPC Animals")]
        public List<string> CreatureDeathMessages
        {
            get => _creaturemessages;
            set => SetValue(ref _creaturemessages, value);
        }
        private List<string> _deathstreakmessages;
        [Display(Order = 21, Name = "Death Streak Messages",Description =
            "Random Messages for when players die 5 in row the Keyword COUNT is used to place number of death happend to player which you can use it depending to your message")]
        public List<string> DeathStreakMessages
        {
            get => _deathstreakmessages;
            set => SetValue(ref _deathstreakmessages, value);
        }

        public static Config CreateDefault()
        {
            Config cfg = new Config();
            cfg.VoxelMessages = new List<string>() { "Smashed into a Voxel", "Defeated by Voxels", "Became one with the voxels" };
            cfg.DrillMessages = new List<string>() { "drilled", "mined" };
            cfg.ExplosionMessages = new List<string>() { "blew up", "exploded", "obliterated" };
            cfg.GrindMessages = new List<string>() { "chopped up", "mutilated", "turned into scrap" };
            cfg.WeldMessages = new List<string>() { "electrocuted", "melted", "welded" };
            cfg.ThrusterMessages = new List<string>() { "was cooked by thruster flames", "looked into a hot engine nozzle", "discovered the laws of thermodynamics" };
            cfg.SuicideMessages = new List<string>() { "Committed Suicide", "took their own life", "committed seppuku" };
            cfg.GridMessages = new List<string>() { "Rammed" };
            cfg.SuffocationMessages = new List<string>() { "Choked to Death", "ran out of oxygen", "was claimed by the vacuum of space", "forgot an oxygen bottle" };
            cfg.ProjectileMessages = new List<string>() { "shot", "killed", "neutralized", "murdered" };
            cfg.HungerMessages = new List<string>() { "starved to death", "could not afford food", "is too poor to make food" };
            cfg.RadioactivityMessages = new List<string>() { "died from radiation poisoning" };
            cfg.WeatherMessages = new List<string>() { "caught in bad weather", "was killed by extreme weather", "could not handle a little rain", "failed at surviving the elements" };
            cfg.EnvironmentMessages = new List<string>() { "Defeated by Environment", "Failed in PvE", "was killed by mysterious forces" };
            cfg.CreatureDeathMessages = new List<string>() { "eaten alive", "became wild life food", "became animals poop", "was ripped apart by an angry animal" };
            cfg.DeathStreakMessages = new List<string>()
    {
        " has died COUNT times in a row! a true noob",
        " is conducting intensive respawn testing (COUNT deaths).",
        " has become best friends with the medical room (COUNT deaths).",
        " is speedrunning the afterlife (COUNT deaths).",
        " has become a frequent flyer of the respawn screen (COUNT deaths).",
        " has been promoted to Senior Test Pilot (COUNT deaths).",
        " is farming death statistics (COUNT deaths).",
        " is roleplaying as a disposable drone (COUNT deaths)",
        " is gathering valuable data on what not to do (COUNT deaths).",
        " experiencing PvE tragedy (COUNT deaths)." 
    };
        
            return cfg;
        }
        // Settings
        private bool _enable = true;
        private bool _announceSuicide = true;
        private bool _announceCollisionDeath = true;
        private bool _announceSuffocationDeath = true;
        private bool _announceDeathStreak = true;

        [Display(Order = 1, Name = "Enable Death Counter Recounted", Description = "Enable or disable the Death Counter Recounted.")]
        public bool Enable
        {
            get => _enable;
            set
            {
                _enable = value;
                OnPropertyChanged();
            }
        }

        [Display(Order = 2, Name = "Announce Suicides", Description = "Enable or disable the announcement of suicides.")]
        public bool AnnounceSuicide
        {
            get => _announceSuicide;
            set
            {
                _announceSuicide = value;
                OnPropertyChanged();
            }
        }
        [Display(Order = 3, Name = "Announce Collision Deaths", Description = "Enable or disable the announcement of deaths caused by collisions.")]
        public bool AnnounceCollisionDeath
        {
            get => _announceCollisionDeath;
            set
            {
                _announceCollisionDeath = value;
                OnPropertyChanged();
            }
        }
        [Display(Order = 4, Name = "Announce Suffocation Deaths", Description = "Enable or disable the announcement of deaths caused by suffocation.")]
        public bool AnnounceSuffocationDeath
        {
            get => _announceSuffocationDeath;
            set
            {
                _announceSuffocationDeath = value;
                OnPropertyChanged();
            }
        }
        [Display(Order = 5, Name = "Announce Death Streaks", Description = "Enable or disable the announcement of death streaks.")]
        public bool AnnounceDeathStreak
        {
            get => _announceDeathStreak;
            set
            {
                _announceDeathStreak = value;
                OnPropertyChanged();
            }
        }
    }
}