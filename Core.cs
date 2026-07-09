using NLog;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Session;
using Torch.Utils;
using Torch.Views;
using VRage.Game.ModAPI;

namespace DeathCounterRecounted
{
    public class Core:TorchPluginBase , IWpfPlugin

    {
        public static readonly Logger Log = LogManager.GetLogger("DeathCounterRecounted");
        private TorchSessionManager _sessionManager;
        public static IChatManagerServer ChatManager;

        public static Core Instance { get; private set; }
        private UserControl _control;
        public UserControl GetControl()
        {
            if (_control == null)
            {
                _control = new PropertyGrid()
                {
                    DataContext = _config.Data
                };
            }

            return (UserControl)_control;
        }
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            Log.Info("DeathCounterRecounted initializing.");

            Instance = this;

            _sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            ChatManager = Torch.Managers.GetManager<IChatManagerServer>();

            if (_sessionManager != null)
                _sessionManager.SessionStateChanged += SessionChanged;

            LoadConfig();
        }

        private VeryPersistent<Config> _config;
        public Config Config => _config?.Data;

        public void Save()
        {
            _config.Save();
        }

        private void LoadConfig()
        {
            var configFile = Path.Combine(StoragePath, "DeathCounterPlugin.cfg");

            _config = VeryPersistent<Config>.Load(
                configFile,
                true,
                Config.CreateDefault
            );
            Log.Info("Configuration loaded.");
        }

        private void SessionChanged(ITorchSession session, TorchSessionState newstate)
        {
            switch (newstate)
            {
                case TorchSessionState.Loading:
                    
                    break;
                case TorchSessionState.Loaded:
                    Log.Info("Game session loaded.");
                    DamagePatch.Init();
                    break;
                case TorchSessionState.Unloading:
                    break;
                case TorchSessionState.Unloaded:
                    break;
            }
        }

    }
}
