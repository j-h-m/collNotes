//
//  DeviceInfo.cs
//
//  Author:
//       Jacob Motley <programmingisfunjacmot@gmail.com>
//
//  Copyright (c) 2019 
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Diagnostics;
using Xamarin.Essentials;
namespace collnotes.Plugins
{
    public static class DeviceInfoCP
    {
        public static string GetDeviceInfo()
        {
            string result = "";

            try
            {
                var device = DeviceInfo.Model;
                var version = DeviceInfo.VersionString;

                result = string.Join(";", "model: " + device, "os version: " + version);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
