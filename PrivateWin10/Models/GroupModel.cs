﻿using PrivateWin10.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PrivateWin10
{
    public class GroupModel
    {
        private static GroupModel mInstance = null;
        public static GroupModel GetInstance()
        {
            if (mInstance == null)
                mInstance = new GroupModel();
            return mInstance;
        }

        public ObservableCollection<ContentControl> Groups { get; set; }

        public GroupModel()
        {
            Groups = new ObservableCollection<ContentControl>();

            HashSet<string> knownGroups = new HashSet<string>();
            Dictionary<Guid, List<FirewallRuleEx>> rules = App.client.GetRules();
            foreach (var ruleSet in rules)
            {
                foreach (FirewallRule rule in ruleSet.Value)
                {
                    if (rule.Grouping != null && rule.Grouping.Length > 0)
                        knownGroups.Add(rule.Grouping);
                }
            }

            foreach (string group in knownGroups)
            {
                string temp = group;
                if (temp.Substring(0, 2) == "@{" && App.PkgMgr != null)
                    temp = App.PkgMgr.GetAppResourceStr(temp);
                else if (temp.Substring(0, 1) == "@")
                    temp = MiscFunc.GetResourceStr(temp);

                Groups.Add(new ContentControl() { Tag = group, Content = temp});
            }
        }
    }
}