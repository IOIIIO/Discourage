﻿using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace IOIIIO.Discourage
{
    [Export(typeof(IEncouragements))]
    public class Encouragements : IEncouragements
    {
        const string CollectionPath = "Encouragements";
        const string PropertyName = "AllEncouragements";

        static readonly Random random = new Random();
        static readonly string[] defaultEncouragements = new[]
        {
            "That was pretty meh...",
            "Uh... are you ok?",
            "Not sure about what you just did there...",
            "Do you want me to be nice... or honest?",
            "Maybe you should take the rest of the day off...",
            "I'm not going to say you made a mistake... but...",
            "Was that a typo, or...",
            "Yikes...",
            "Disappointing.",
            "You should get some help...",
            "Maybe programming isn't for you..."
        };

        readonly List<string> encouragements = new List<string>(defaultEncouragements);
        readonly WritableSettingsStore writableSettingsStore;

        public IEnumerable<string> AllEncouragements
        {
            get { return encouragements; }
            set
            {
                encouragements.Clear();
                encouragements.AddRange(value);
                if (encouragements.Count == 0)
                {
                    encouragements.AddRange(defaultEncouragements);
                }
                SaveSettings();
            }
        }

        [ImportingConstructor]
        public Encouragements(SVsServiceProvider vsServiceProvider)
        {
            var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
            writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            LoadSettings();
        }

        public string GetRandomEncouragement()
        {
            int randomIndex = random.Next(0, encouragements.Count);
            return encouragements[randomIndex];
        }

        void LoadSettings()
        {
            try
            {
                if (writableSettingsStore.PropertyExists(CollectionPath, PropertyName))
                {
                    string value = writableSettingsStore.GetString(CollectionPath, PropertyName);
                    AllEncouragements = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        void SaveSettings()
        {
            try
            {
                if (!writableSettingsStore.CollectionExists(CollectionPath))
                {
                    writableSettingsStore.CreateCollection(CollectionPath);
                }

                string value = string.Join(Environment.NewLine, encouragements);
                writableSettingsStore.SetString(CollectionPath, PropertyName, value);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }
    }
}