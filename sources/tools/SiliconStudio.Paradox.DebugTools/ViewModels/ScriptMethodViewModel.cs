﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Paradox.Framework.MicroThreading;
using SiliconStudio.Paradox.Extensions;
using System.Windows.Input;
using SiliconStudio.Presentation;
using SiliconStudio.Presentation.Commands;

namespace SiliconStudio.Paradox.DebugTools.ViewModels
{
    public class ScriptMethodViewModel : DeprecatedViewModelBase
    {
        public ScriptAssemblyViewModel AssemblyParent { get; private set; }
        public ScriptTypeViewModel TypeParent { get; private set; }

        public ICommand RunScriptCommand { get; private set; }
        public ICommand CloseMicroThreadView { get; private set; }

        private ScriptEntry2 scriptEntry;

        public ScriptMethodViewModel(ScriptEntry2 scriptEntry, ScriptTypeViewModel parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            TypeParent = parent;
            AssemblyParent = TypeParent.Parent;

            this.scriptEntry = scriptEntry;

            RunScriptCommand = new AnonymousCommand(_ => OnRunScriptCommand());
            CloseMicroThreadView = new AnonymousCommand(_ => MicroThread = null);
        }

        public string Name
        {
            get
            {
                return scriptEntry.MethodName;
            }
        }

        public bool IsAssemblyStartup
        {
            get
            {
                return scriptEntry.Flags == ScriptFlags.AssemblyStartup;
            }
        }

        public bool HasNoFlags
        {
            get
            {
                return scriptEntry.Flags == ScriptFlags.None;
            }
        }

        public ScriptFlags FlagsDisplay
        {
            get
            {
                return scriptEntry.Flags;
            }
        }

        private void OnRunScriptCommand()
        {
            MicroThread mt = AssemblyParent.Parent.EngineContext.ScriptManager.RunScript(scriptEntry, null);
            MicroThread = new MicroThreadViewModel(mt);
        }

        private MicroThreadViewModel microThread;
        public MicroThreadViewModel MicroThread
        {
            get
            {
                return microThread;
            }
            private set
            {
                if (microThread != value)
                {
                    microThread = value;
                    OnPropertyChanged("MicroThread");
                }
            }
        }
    }
}
