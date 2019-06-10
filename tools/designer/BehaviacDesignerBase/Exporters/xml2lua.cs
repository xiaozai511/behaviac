/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Tencent is pleased to support the open source community by making behaviac available.
//
// Copyright (C) 2015 THL A29 Limited, a Tencent company. All rights reserved.
//
// Licensed under the BSD 3-Clause License (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at http://opensource.org/licenses/BSD-3-Clause
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;

namespace Behaviac.Design.Exporters
{
    /// <summary>
    /// This exporter generates .cs files which generate a static variable which holds the behaviour tree.
    /// </summary>
    /// 
    /// Lua Format Consult: LuaXml.lua
    /// 
    public class XmlToLua
    {
        public static string Translate(XmlDocument xmlDoc, string filename)
        {
            StringBuilder sbLua = new StringBuilder();
            sbLua.AppendFormat("Behavior.BehaviorDataList[\"{0}\"]=\n", filename.Replace("\\", "/"));
            XmlToLuaNode(sbLua, xmlDoc.DocumentElement, "", 0);
            return sbLua.ToString();
        }

        //  XmlToLuaNode:  Output an XmlElement
        private static void XmlToLuaNode(StringBuilder sbLua, XmlElement node, string indent = "", int subIndex = 0)
        {
            //  table begin
            sbLua.Append(indent);

            sbLua.Append("{\n");
            //if (subIndex == 0)  //±íÊ¾×îÍâ²ã
            //{
            //    sbLua.Append("{\n");
            //}
            //else
            //{
            //    sbLua.AppendFormat("[{0}]{\n", subIndex);
            //}

            string subIndent = indent + "\t";

            //  node name
            sbLua.Append(subIndent);
            sbLua.AppendFormat("[0]=\"{0}\",\n", SafeLua(node.Name));

            //  Add in all node attributes
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    sbLua.Append(subIndent);
                    sbLua.AppendFormat("[\"{0}\"]=\"{1}\",\n", SafeLua(attr.Name), SafeLua(attr.Value));
                }
            }

            //  Add in all nodes
            int index = 1;
            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode is XmlText)
                {
                    // skip
                }

                else if (cnode is XmlElement)
                {
                    XmlToLuaNode(sbLua, (XmlElement)cnode, subIndent, index++);
                }
            }

            //  table end
            sbLua.Append(indent);
            if (subIndex == 0)  //±íÊ¾×îÍâ²ã
            {
                sbLua.Append("}\n");
            }
            else
            {
                sbLua.Append("},\n");
            }
        }

        // Make a string safe for Lua
        static private string SafeLua(string sIn)
        {
            //StringBuilder sbOut = new StringBuilder(sIn.Length);
            //foreach (char ch in sIn)
            //{
            //    if (Char.IsControl(ch) || ch == '\'')
            //    {
            //        int ich = (int)ch;
            //        sbOut.Append(@"\u" + ich.ToString("x4"));
            //        continue;
            //    }
            //    else if (ch == '\"' || ch == '\\' || ch == '/')
            //    {
            //        sbOut.Append('\\');
            //    }
            //    sbOut.Append(ch);
            //}
            //return sbOut.ToString();
            
            // replace "\"" to "&quot;"
            sIn = sIn.Replace("\"", "&quot;");
            return sIn;
        }
    }
}
