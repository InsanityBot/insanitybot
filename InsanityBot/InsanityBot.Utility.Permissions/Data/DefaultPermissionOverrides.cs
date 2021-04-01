﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace InsanityBot.Utility.Permissions.Data
{
    public static class DefaultPermissionOverrides
    {
        public static Dictionary<String, Boolean> Deserialize()
        {
            StreamReader reader = new("./config/overrides.default.json");
            String[] overrides = JsonConvert.DeserializeObject<String[]>(reader.ReadToEnd());
            reader.Close();

            Dictionary<String, Boolean> retValue = new();

            foreach(var v in overrides)
            {
                retValue.Add(v.Split(' ')[0], 
                    Convert.ToBoolean(
                        v.Split(' ')[1]));
            }

            return retValue;
        }

        // no serialize. for now, this is user input only. instead, have an extension method to apply these.

        public static DefaultPermissions ApplyOverrides(this DefaultPermissions permissions, Dictionary<String, Boolean> overrides)
        {
            DefaultPermissions value = permissions;

            foreach (var v in overrides)
                permissions[v.Key] = v.Value switch
                {
                    true => PermissionValue.Allowed,
                    false => PermissionValue.Denied
                };

            return value;
        }
    }
}
