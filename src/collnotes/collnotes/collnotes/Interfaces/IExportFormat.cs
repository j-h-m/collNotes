//
//  IDataExport.cs
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
using System.Collections.Generic;
using collnotes.Data;

namespace collnotes.Interfaces
{
    public interface IExportFormat
    {
        // csv headers with order
        Dictionary<int, string> GetColumnNames();
        // map of column names to object type and property
        Dictionary<int, KeyValuePair<Type, string>> GetCSVHeaderMap();
        // method to generate export text
        string GenerateExportText(Project project);
        // method to verify csv format
        bool VerifyExportText(string exportText);
    }
}
