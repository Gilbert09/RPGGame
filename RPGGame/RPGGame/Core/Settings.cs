using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RPGGame.Sprites;

namespace RPGGame.Core
{
    static class Settings
    {
        private static Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public static void Init()
        {
            StreamReader reader = new StreamReader("settings.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] split = line.Split('=');
                dictionary.Add(split[0].Replace(":@:", "="), split[1].Replace(":@:", "="));
            }
            reader.Close();
        }

        public static string GetValue(string key)
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            else return null;
        }

        public static void SetValue(string key, string value)
        {
            if (dictionary.ContainsKey(key)) dictionary[key] = value;
            else dictionary.Add(key, value);
            StreamWriter writer = new StreamWriter("settings.txt");
            foreach(KeyValuePair<string, string> kvp in dictionary)
            {
                writer.WriteLine(kvp.Key.Replace("=", ":@:") + "=" + kvp.Value.Replace("=", ":@:"));
            }
            writer.Close();
        }

        public static void ResetSettings()
        {
            dictionary.Clear();
            StreamWriter writer = new StreamWriter("settings.txt");
            foreach (KeyValuePair<string, string> kvp in dictionary)
            {
                writer.WriteLine(kvp.Key.Replace("=", ":@:") + "=" + kvp.Value.Replace("=", ":@:"));
            }
            writer.Close();
        }

        public static void ReloadSettings()
        {
            dictionary.Clear();
            StreamReader reader = new StreamReader("settings.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] split = line.Split('=');
                dictionary.Add(split[0].Replace(":@:", "="), split[1].Replace(":@:", "="));
            }
            reader.Close();
        }

        public static void SaveWizard(Wizard wiz)
        {
            SetValue("health", wiz.Health.ToString());
            SetValue("maxhealth", wiz.MaxHealth.ToString());
            SetValue("exp", wiz.Experiance.ToString());
            SetValue("maxexp", wiz.MaxExperiance.ToString());
            SetValue("mana", wiz.Mana.ToString());
            SetValue("maxmana", wiz.MaxMana.ToString());
            SetValue("damage", wiz.Damage.ToString());
            SetValue("level", wiz.Level.ToString());
            SetValue("x", wiz.Position.X.ToString());
            SetValue("y", wiz.Position.Y.ToString());
            SetValue("defense", wiz.Defense.ToString());
            SetValue("wisdom", wiz.Wisdom.ToString());
            SetValue("agility", wiz.Agility.ToString());
        }
    }
}
